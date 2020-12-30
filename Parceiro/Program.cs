using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Shared;

namespace Parceiro
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inputQueueName = "Parceiro";
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            var connectionString = rabbitMqConfiguration.ToConnectionString();

            // 1. Service registration pipeline...
            var services = new ServiceCollection();
            services.AutoRegisterHandlersFromAssemblyOf<Handler2>();
            services.AddSingleton<Producer>();

            // 1.1. Configure Rebus
            services.AddRebus(configure => configure
                .Logging(l => l.ColoredConsole())
                .Transport(t => t.UseRabbitMq(connectionString, inputQueueName))
                .Options(o => o.UseCustomTopicNameConvention(prefix: $"{inputQueueName}_"))
            );

            // 1.2. Potentially add more service registrations for the application, some of which
            //      could be required by handlers.

            // 2. Application starting pipeline...
            // Make sure we correctly dispose of the provider (and therefore the bus) on application shutdown
            using (var provider = services.BuildServiceProvider())
            {
                // 3. Application started pipeline...

                // 3.1. Now application is running, lets trigger the 'start' of Rebus.
                provider.UseRebus(a => a.Subscribe<Ping>());
                //optionally...
                //provider.UseRebus(async bus => await bus.Subscribe<Message1>());

                // 3.2. Begin the domain work for the application
                var producer = provider.GetRequiredService<Producer>();
                producer.Produce();
            }
        }
    }
}
