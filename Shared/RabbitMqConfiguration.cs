using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; } = "localhost";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public int Port { get; set; } = 5672;
        public string VHost { get; set; } = "";

        public string ToConnectionString() => $"amqp://{UserName}:{Password}@{HostName}:{Port}/{VHost}";
    }
}
