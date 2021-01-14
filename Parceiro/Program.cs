using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.ServiceProvider;
using RebusExtensions;
using System;
using System.Collections.Generic;

namespace Parceiro
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inputQueueName = "Antecipacao";
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            var connectionString = rabbitMqConfiguration.ToConnectionString();

            var topicsDictionary = new TopicsDictionary(new Dictionary<string, Type>
            {
                ["Antecipacao_Ping"] = typeof(Ping),
                ["Parceiro_Pong"] = typeof(Pong)
            });

            // 1. Service registration pipeline...
            var services = new ServiceCollection();
            services.AddSingleton(topicsDictionary);
            services.AutoRegisterHandlersFromAssemblyOf<Program>();
            services.AddSingleton<Producer>();

            // 1.1. Configure Rebus
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq(connectionString, inputQueueName))
                .UseNoCodeSharing(topicsDictionary)
            );

            // 1.2. Potentially add more service registrations for the application, some of which
            //      could be required by handlers.

            // 2. Application starting pipeline...
            // Make sure we correctly dispose of the provider (and therefore the bus) on application shutdown
            using (var provider = services.BuildServiceProvider())
            {
                // 3. Application started pipeline...

                // 3.1. Now application is running, lets trigger the 'start' of Rebus.
                provider.UseRebus(rebus =>
                {
                    rebus.Subscribe(topicsDictionary).Wait();
                });
                //optionally...
                //provider.UseRebus(async bus => await bus.Subscribe<Message1>());

                // 3.2. Begin the domain work for the application
                var producer = provider.GetRequiredService<Producer>();
                producer.Produce();
            }
        }
    }
}
