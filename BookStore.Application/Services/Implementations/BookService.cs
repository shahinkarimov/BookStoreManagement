using AutoMapper;
using BookStore.Application.DTOs.Books;
using BookStore.Application.Services.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Domain.Exceptions;
using BookStore.Domain.Interfaces;
using FluentValidation;

namespace BookStore.Application.Services.Implementations;

/// <summary>
/// Kitab biznes məntiqi: validasiya, müəllif/janr mövcudluğu yoxlanışı, CRUD.
/// </summary>
public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateBookRequestDto> _createValidator;
    private readonly IValidator<UpdateBookRequestDto> _updateValidator;

    public BookService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateBookRequestDto> createValidator,
        IValidator<UpdateBookRequestDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookRequestDto request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId)
            ?? throw new NotFoundException(nameof(Author), request.AuthorId);
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.GenreId)
            ?? throw new NotFoundException(nameof(Genre), request.GenreId);

        var book = _mapper.Map<Book>(request);
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        book.Author = author;
        book.Genre = genre;
        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetAllAsync()
    {
        var books = await _unitOfWork.Books.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }

    public async Task<BookResponseDto?> GetByIdAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdWithDetailsAsync(id);
        return book is null ? null : _mapper.Map<BookResponseDto>(book);
    }

    public async Task<IReadOnlyList<BookResponseDto>> SearchAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Array.Empty<BookResponseDto>();

        var books = await _unitOfWork.Books.SearchAsync(term.Trim());
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetByGenreAsync(int genreId)
    {
        var books = await _unitOfWork.Books.GetByGenreAsync(genreId);
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetByAuthorAsync(int authorId)
    {
        var books = await _unitOfWork.Books.GetByAuthorAsync(authorId);
        return _mapper.Map<IReadOnlyList<BookResponseDto>>(books);
    }

    public async Task<BookResponseDto> UpdateAsync(UpdateBookRequestDto request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var book = await _unitOfWork.Books.GetByIdWithDetailsAsync(request.Id)
            ?? throw new NotFoundException(nameof(Book), request.Id);

        book.Title = request.Title;
        book.Price = request.Price;
        book.Stock = request.Stock;

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Book), id);

        _unitOfWork.Books.Delete(book);
        await _unitOfWork.SaveChangesAsync();
    }
}
