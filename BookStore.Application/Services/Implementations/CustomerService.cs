using AutoMapper;
using BookStore.Application.DTOs.Customers;
using BookStore.Application.Services.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Domain.Exceptions;
using BookStore.Domain.Interfaces;
using FluentValidation;

namespace BookStore.Application.Services.Implementations;

/// <summary>
/// Müştəri biznes məntiqi: email unikallığı, qeydiyyat.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerRequestDto> _validator;

    public CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCustomerRequestDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CustomerResponseDto> RegisterAsync(CreateCustomerRequestDto request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _unitOfWork.Customers.ExistsByEmailAsync(email))
            throw new DomainException($"\"{email}\" email-i ilə müştəri artıq qeydiyyatdan keçib.");

        var customer = _mapper.Map<Customer>(request);
        customer.Email = email;
        customer.FullName = customer.FullName.Trim();

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<IReadOnlyList<CustomerResponseDto>> GetAllAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return _mapper.Map<IReadOnlyList<CustomerResponseDto>>(customers);
    }
}
