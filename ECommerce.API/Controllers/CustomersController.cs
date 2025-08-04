using ECommerce.Application.Dtos;
using ECommerce.Application.Enums;
using ECommerce.Application.Responses;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<Customer>>.Ok(customers));
        }

        //[HttpGet("{id}")]
        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                return NotFound(ApiResponse<string>.Fail(ResponseStatusCode.NotFound, "Customer not found"));

            return Ok(ApiResponse<Customer>.Ok(customer));
        }

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<string>.Fail(ResponseStatusCode.BadRequest, "Validation errors", errors));
            }

            var customer = new Customer { Name = dto.Name, Email = dto.Email, Phone = dto.Phone };
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveAsync();

            return Ok(ApiResponse<Customer>.Created(customer));
        }
    }
}
