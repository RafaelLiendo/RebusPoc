
using System;
using Rebus.Bus;
using RebusExtensions;

namespace Parceiro
{
    public class Producer
    {
        private readonly IBus _bus;

        public Producer(IBus bus)
        {
            _bus = bus;
        }

        public void Produce()
        {
            var keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine(@"
a) Send Pong
q) Quit
");
                var key = char.ToLower(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'a':
                        Send(_bus);
                        break;
                    case 'q':
                        Console.WriteLine("Quitting");
                        keepRunning = false;
                        break;
                }
            }

            Console.WriteLine("Consumer listening - press ENTER to quit");
            Console.ReadLine();
        }

        static void Send(IBus bus)
        {
            Console.WriteLine("Publishing Pong");

            //bus.Publish("Parceiro:Pong", new Pong { Bar = "Zini" }).Wait();
            bus.Publish(new Pong { Bar = "Zini2" }).Wait();
        }
    }
}
