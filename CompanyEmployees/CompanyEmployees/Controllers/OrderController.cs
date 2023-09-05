﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public OrderController(IRepositoryManager repository, ILoggerService logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOrdersForCompany(Guid companyId)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var ordersFromDb = _repository.Order.GetOrders(companyId, false);
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(ordersFromDb);
            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var order = _repository.Order.GetOrder(companyId, id, false);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
        }
    }
}
