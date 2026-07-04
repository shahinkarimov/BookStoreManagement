using AutoMapper;
using BookStore.Application.DTOs.Authors;
using BookStore.Application.DTOs.Books;
using BookStore.Application.Services.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Domain.Exceptions;
using BookStore.Domain.Interfaces;
using FluentValidation;

namespace BookStore.Application.Services.Implementations;

/// <summary>
/// Müəllif biznes məntiqi: dublikat ad yoxlanışı, kitabların siyahısı.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateAuthorRequestDto> _validator;

    public AuthorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateAuthorRequestDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<AuthorResponseDto> CreateAsync(CreateAuthorRequestDto request)
    {
        await _validator.ValidateAndThrowAsync(request);

        if (await _unitOfWork.Authors.ExistsByNameAsync(request.FullName.Trim()))
            throw new DomainException($"\"{request.FullName}\" adlı müəllif artıq mövcuddur.");

        var author = _mapper.Map<Author>(request);
        author.FullName = author.FullName.Trim();

        await _unitOfWork.Authors.AddAsync(author);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AuthorResponseDto>(author);
    }

    public async Task<IReadOnlyList<AuthorResponseDto>> GetAllAsync()
    {
        var authors = await _unitOfWork.Authors.GetAllAsync();
        return _mapper.Map<IReadOnlyList<AuthorResponseDto>>(authors);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetBooksAsync(int authorId)
    {
        _ = await _unitOfWork.Authors.GetByIdAsync(authorId)
            ?? throw new NotFoundException(nameof(Author), authorId);

        var books = await _unitOfWork.Books.GetByAuthorAsync(authorId);
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }
}
