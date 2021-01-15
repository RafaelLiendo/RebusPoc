using System;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Parceiro
{
    public class Handler3 : IHandleMessages<Ping>
    {
        public Task Handle(Ping message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Handler3 received : Ping {message.Foo}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
