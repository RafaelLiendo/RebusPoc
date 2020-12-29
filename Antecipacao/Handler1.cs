using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Shared;

namespace Antecipacao
{
    public class Handler1 : IHandleMessages<Pong>
    {
        public Task Handle(Pong message)
        {
            Console.WriteLine($"Handler1 received : Pong {message.Bar}");

            return Task.CompletedTask;
        }
    }
}
