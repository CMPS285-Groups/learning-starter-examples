using System;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public OrdersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new Response();

            var orders = _dataContext
                .Orders
                .Select(order => new OrderGetDto
                {
                    Id = order.Id,
                    PaymentType = order.PaymentType,
                    DatePurchased = order.DatePurchased,
                })
                .ToList();

            response.Data = orders;
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var response = new Response();

            var orderToReturn = _dataContext
                .Orders
                .Select(order => new OrderGetDto
                {
                    Id = order.Id,
                    DatePurchased = order.DatePurchased,
                    PaymentType = order.PaymentType,
                    Products = order.OrderProducts
                        .Select(orderProduct => orderProduct.Product)
                        .Select(product => new ProductGetDto
                        {
                            Id = product.Id,
                            Cost = product.Cost,
                            Name = product.Name,
                            Quantity = product.Quantity,
                        })
                        .ToList()
                })
                .FirstOrDefault(order => order.Id == id);

            if (orderToReturn == null)
            {
                response.AddError("id", "Order not found.");
                return BadRequest(response);
            }

            response.Data = orderToReturn;
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] OrderCreateDto orderCreateDto)
        {
            var response = new Response();
            if (string.IsNullOrEmpty(orderCreateDto.PaymentType))
            {
                response.AddError("paymentType", "Payment type cannot be empty");
            }

            if (response.HasErrors)
            {
                return BadRequest(response);
            }

            var orderToAdd = new Order
            {
                DatePurchased = DateTimeOffset.Now,
                PaymentType = orderCreateDto.PaymentType
            };

            _dataContext.Orders.Add(orderToAdd);
            _dataContext.SaveChanges();

            var orderToReturn = new OrderGetDto
            {
                Id = orderToAdd.Id,
                DatePurchased = orderToAdd.DatePurchased,
                PaymentType = orderToAdd.PaymentType,
            };

            response.Data = orderToReturn;
            return Created("", response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] OrderUpdateDto orderUpdateDto)
        {
            var response = new Response();

            var orderToUpdate = _dataContext
                .Orders
                .FirstOrDefault(order => order.Id == id);

            if (orderToUpdate == null)
            {
                response.AddError("id", "Order not found.");
                return BadRequest(response);
            }

            orderToUpdate.PaymentType = orderUpdateDto.PaymentType;
            _dataContext.SaveChanges();

            var orderToReturn = new OrderGetDto
            {
                Id = orderToUpdate.Id,
                DatePurchased = orderToUpdate.DatePurchased,
                PaymentType = orderToUpdate.PaymentType,
            };

            response.Data = orderToReturn;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = new Response();

            var orderToDelete = _dataContext
                .Orders
                .FirstOrDefault(order => order.Id == id);

            if (orderToDelete == null)
            {
                response.AddError("id", "Order not found.");
                return BadRequest(response);
            }

            _dataContext.Remove(orderToDelete);
            _dataContext.SaveChanges();

            response.Data = true;
            return Ok(response);
        }
    }
}
