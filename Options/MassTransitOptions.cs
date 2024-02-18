using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted.Options
{
    public record RabbitMQOptions(string Host, string VirtualHost, string Username, string Password);
    public record MassTransitOptions(RabbitMQOptions rabbitMQOptions);
}
