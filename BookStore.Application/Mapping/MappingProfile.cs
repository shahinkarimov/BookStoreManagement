using AutoMapper;
using BookStore.Application.DTOs.Authors;
using BookStore.Application.DTOs.Books;
using BookStore.Application.DTOs.Customers;
using BookStore.Application.DTOs.Genres;
using BookStore.Application.DTOs.Orders;
using BookStore.Domain.Entities;

namespace BookStore.Application.Mapping;

/// <summary>
/// AutoMapper profili — Domain entity ↔ DTO çevrilmə qaydaları.
/// Naviqasiya property-lərindən düz sahələrə (flattening) map edilir.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Kitab
        CreateMap<CreateBookRequestDto, Book>();
        CreateMap<Book, BookResponseDto>()
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.FullName))
            .ForMember(d => d.GenreName, o => o.MapFrom(s => s.Genre.Name));

        // Müəllif
        CreateMap<CreateAuthorRequestDto, Author>();
        CreateMap<Author, AuthorResponseDto>()
            .ForMember(d => d.BookCount, o => o.MapFrom(s => s.Books.Count));

        // Janr
        CreateMap<CreateGenreRequestDto, Genre>();
        CreateMap<Genre, GenreResponseDto>()
            .ForMember(d => d.BookCount, o => o.MapFrom(s => s.Books.Count));

        // Müştəri
        CreateMap<CreateCustomerRequestDto, Customer>();
        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(d => d.OrderCount, o => o.MapFrom(s => s.Orders.Count));

        // Sifariş
        CreateMap<Order, OrderResponseDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName));
        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Book.Title))
            .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.UnitPrice * s.Quantity));
    }
}
