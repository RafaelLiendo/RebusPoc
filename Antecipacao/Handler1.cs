using System;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Antecipacao
{
    public class Handler1 : IHandleMessages<Pong>
    {
        public Task Handle(Pong message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Handler1 received : Pong {message.Bar}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
