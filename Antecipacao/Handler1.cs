using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Shared;

namespace Antecipacao
{
    public class Handler1 : IHandleMessages<Shared.Pong>
    {
        public Task Handle(Shared.Pong message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Handler1 received : Pong {message.Bar}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
