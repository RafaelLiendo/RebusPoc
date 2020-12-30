using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Shared;

namespace Parceiro
{
    public class Handler2 : IHandleMessages<Ping>
    {
        public Task Handle(Ping message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Handler2 received : Ping {message.Foo}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
