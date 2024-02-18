using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using GettingStarted.Services;
using Microsoft.Extensions.Configuration;
using GettingStarted.Options;

namespace GettingStarted;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    // By default, sagas are in-memory, but should be changed to a durable
                    // saga repository.
                    x.SetInMemorySagaRepositoryProvider();

                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddConsumers(entryAssembly);
                    x.AddSagaStateMachines(entryAssembly);
                    x.AddSagas(entryAssembly);
                    x.AddActivities(entryAssembly);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        MassTransitOptions massTransitOptions = hostContext.Configuration.GetSection("").Get<MassTransitOptions>();

                        //string host = hostContext.Configuration["MassTransit:RabbitMQ:Host"];
                        //string virtualHost = hostContext.Configuration["MassTransit:RabbitMQ:VirtualHost"];
                        //string userName = hostContext.Configuration["MassTransit:RabbitMQ:UserName"];
                        //string password = hostContext.Configuration["MassTransit:RabbitMQ:Password"];

                        string host = massTransitOptions.rabbitMQOptions.Host;
                        string virtualHost = massTransitOptions.rabbitMQOptions.VirtualHost;
                        string userName = massTransitOptions.rabbitMQOptions.Username;
                        string password = massTransitOptions.rabbitMQOptions.Password;

                        cfg.Host(host, virtualHost, h => {
                            h.Username(userName);
                            h.Password(password);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });

                services.AddHostedService<Worker>();
            });



}

