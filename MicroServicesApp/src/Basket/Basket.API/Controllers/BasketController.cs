using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _rabbitMQProducer;

        private readonly ILogger<BasketController> _logger;

        public BasketController(ILogger<BasketController> logger,IMapper mapper,
             EventBusRabbitMQProducer rabbitMQProducer,IBasketRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _rabbitMQProducer = rabbitMQProducer;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasketCart(userName);
            return Ok(basket ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody]BasketCart basketCart)
        {
            var basket = await _repository.UpdateBasket(basketCart);
            return Ok(basket);
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            return Ok(await _repository.DeleteBasket(username));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout checkout)
        {
            var findBasket = await _repository.GetBasketCart(checkout.UserName);
            if (findBasket == null) return BadRequest();

            var removeBasket = await _repository.DeleteBasket(checkout.UserName);
            if (!removeBasket) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(checkout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = findBasket.TotalPrice;

            try
            {
                _rabbitMQProducer.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Accepted();
        }
    }
}

