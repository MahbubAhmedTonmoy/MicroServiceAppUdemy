using Microsoft.AspNetCore.Builder;
using Ordering.API.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Ordering.API.Extention
{
    public static class ApplicationBuilderExtention
    {
        public static EventBusRabbitMQConsumer Listner { get; set; }
        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            Listner = app.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            Listner.Disconnect();
        }

        private static void OnStarted()
        {
            Listner.Consume();
        }
    }
}
