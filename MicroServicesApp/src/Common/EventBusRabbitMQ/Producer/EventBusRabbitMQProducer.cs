using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        public EventBusRabbitMQProducer(IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
        }

        public void PublishBasketCheckOut(string queueName, BasketCheckoutEvent basketCheckoutEvent)
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(basketCheckoutEvent);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;
                basicProperties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish("", queueName, true, basicProperties, body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) => {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }
    }
}
