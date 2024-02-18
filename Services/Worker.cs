using MassTransit;
namespace GettingStarted.Services;

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GettingStarted.Contracts;


public class Worker(IBus bus) : BackgroundService
{
    readonly IBus _bus = bus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}

