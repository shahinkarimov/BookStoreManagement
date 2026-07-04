using AutoMapper;
using BookStore.Application.DTOs.Books;
using BookStore.Application.DTOs.Genres;
using BookStore.Application.Services.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Domain.Exceptions;
using BookStore.Domain.Interfaces;
using FluentValidation;

namespace BookStore.Application.Services.Implementations;

/// <summary>
/// Janr biznes məntiqi: dublikat ad yoxlanışı, janra görə kitablar.
/// </summary>
public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateGenreRequestDto> _validator;

    public GenreService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateGenreRequestDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<GenreResponseDto> CreateAsync(CreateGenreRequestDto request)
    {
        await _validator.ValidateAndThrowAsync(request);

        if (await _unitOfWork.Genres.ExistsByNameAsync(request.Name.Trim()))
            throw new DomainException($"\"{request.Name}\" adlı janr artıq mövcuddur.");

        var genre = _mapper.Map<Genre>(request);
        genre.Name = genre.Name.Trim();

        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GenreResponseDto>(genre);
    }

    public async Task<IReadOnlyList<GenreResponseDto>> GetAllAsync()
    {
        var genres = await _unitOfWork.Genres.GetAllAsync();
        return _mapper.Map<IReadOnlyList<GenreResponseDto>>(genres);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetBooksAsync(int genreId)
    {
        _ = await _unitOfWork.Genres.GetByIdAsync(genreId)
            ?? throw new NotFoundException(nameof(Genre), genreId);

        var books = await _unitOfWork.Books.GetByGenreAsync(genreId);
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }
}
