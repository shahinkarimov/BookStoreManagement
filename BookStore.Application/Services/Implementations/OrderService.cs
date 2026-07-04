using AutoMapper;
using BookStore.Application.DTOs.Orders;
using BookStore.Application.Services.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Domain.Exceptions;
using BookStore.Domain.Interfaces;
using FluentValidation;

namespace BookStore.Application.Services.Implementations;

/// <summary>
/// Sifariş biznes məntiqi: stok yoxlanışı və azaldılması, cəmi məbləğin hesablanması.
/// Bütün dəyişikliklər tək SaveChanges ilə atomik yazılır.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrderRequestDto> _validator;

    public OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateOrderRequestDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderRequestDto request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        // Eyni kitab bir neçə sətirdə seçilibsə, sayları birləşdiririk
        var mergedItems = request.Items
            .GroupBy(i => i.BookId)
            .Select(g => new { BookId = g.Key, Quantity = g.Sum(i => i.Quantity) })
            .ToList();

        var order = new Order { CustomerId = customer.Id, OrderDate = DateTime.UtcNow };

        foreach (var item in mergedItems)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(item.BookId)
                ?? throw new NotFoundException(nameof(Book), item.BookId);

            if (book.Stock < item.Quantity)
                throw new DomainException(
                    $"\"{book.Title}\" üçün stok kifayət etmir. Mövcud: {book.Stock}, istənilən: {item.Quantity}.");

            // Stok azaldılır, sifariş anındakı qiymət tarixə yazılır
            book.Stock -= item.Quantity;
            _unitOfWork.Books.Update(book);

            order.Items.Add(new OrderItem
            {
                BookId = book.Id,
                Quantity = item.Quantity,
                UnitPrice = book.Price
            });
        }

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        var saved = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id);
        return _mapper.Map<OrderResponseDto>(saved!);
    }

    public async Task<IReadOnlyList<OrderResponseDto>> GetByCustomerAsync(int customerId)
    {
        _ = await _unitOfWork.Customers.GetByIdAsync(customerId)
            ?? throw new NotFoundException(nameof(Customer), customerId);

        var orders = await _unitOfWork.Orders.GetByCustomerAsync(customerId);
        return _mapper.Map<IReadOnlyList<OrderResponseDto>>(orders);
    }

    public async Task<IReadOnlyList<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();
        return _mapper.Map<IReadOnlyList<OrderResponseDto>>(orders);
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetWithDetailsAsync(id);
        return order is null ? null : _mapper.Map<OrderResponseDto>(order);
    }
}
