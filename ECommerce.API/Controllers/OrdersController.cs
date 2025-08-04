using ECommerce.Application.Dtos;
using ECommerce.Application.Enums;
using ECommerce.Application.Responses;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var errors = new List<string>();

            if (!ModelState.IsValid)
            {
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<string>.Fail(ResponseStatusCode.BadRequest, "Validation errors", errors));
            }

            var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                errors.Add("Customer not found");
                return NotFound(ApiResponse<string>.Fail(ResponseStatusCode.NotFound, "Customer not found", errors));
            }

            var order = new Order { CustomerId = dto.CustomerId, Status = "Pending" };

            foreach (var productId in dto.ProductIds)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product != null)
                {
                    order.OrderProducts.Add(new OrderProduct { Order = order, Product = product });
                    order.TotalPrice += product.Price;
                }
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            return Ok(ApiResponse<Order>.Created(order));
        }

        //[HttpGet("{id}")]
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(ApiResponse<string>.Fail(ResponseStatusCode.NotFound, "Order not found"));

            var dto = new OrderDetailsDto
            {
                Id = order.Id,
                CustomerName = order.Customer.Name,
                Status = order.Status,
                ProductCount = order.OrderProducts.Count
            };

            return Ok(ApiResponse<OrderDetailsDto>.Ok(dto));
        }

        [HttpPost("UpdateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderWithProductsAsync(id);

            if (order == null)
            {
                var errors = new List<string> { "Order not found" };
                return NotFound(ApiResponse<string>.Fail(ResponseStatusCode.NotFound, "Order not found", errors));
            }

            order.Status = "Delivered";

            foreach (var op in order.OrderProducts)
            {
                op.Product.Stock -= 1;
            }
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveAsync();

            return Ok(ApiResponse<string>.Ok("Order marked as Delivered"));

        }
        
        //
    }
}
