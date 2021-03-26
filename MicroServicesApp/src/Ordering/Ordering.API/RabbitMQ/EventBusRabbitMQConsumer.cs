using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly IRabbitMQConnection _rabbitMQConnection;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _rabbitMQConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Consume()
        {
            var channel = _rabbitMQConnection.CreateModel();
            channel.QueueDeclare(EventBusConstants.BasketCheckoutQueue, false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;  //delegate : placeholder for an EVENT , method pointer 

            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue,autoAck: true,consumer: consumer, noLocal: false, exclusive: false, arguments: null);
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if(e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _rabbitMQConnection.Dispose();
        }
    }
}
