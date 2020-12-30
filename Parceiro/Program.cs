﻿using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Serialization.Json;
using Rebus.ServiceProvider;
using Shared;
using System.Linq;
using System.Threading.Tasks;

namespace Parceiro
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inputQueueName = "Parceiro";
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            var connectionString = rabbitMqConfiguration.ToConnectionString();

            var topicsDictionary = new TopicsDictionary()
            {
                ["Antecipacao_Ping"] = typeof(Ping),
            };

            // 1. Service registration pipeline...
            var services = new ServiceCollection();
            services.AddSingleton(topicsDictionary);
            services.AutoRegisterHandlersFromAssemblyOf<Program>();
            services.AddSingleton<Producer>();

            // 1.1. Configure Rebus
            services.AddRebus(configure => configure
                .Logging(l => l.ColoredConsole())
                .Transport(t => t.UseRabbitMq(connectionString, inputQueueName))
                .Serialization(s => s.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson))
                .Options(o => o
                    .UseCustomTopicPipeline()
                    .UseCustomMessageDeserializer(topicsDictionary)
                    .UseCustomTopicNameConvention(prefix: $"{inputQueueName}_"))
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
                    Task.WaitAll(
                        topicsDictionary.Keys.Select(topic =>
                            rebus.Advanced.Topics.Subscribe(topic)
                        ).ToArray());
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
