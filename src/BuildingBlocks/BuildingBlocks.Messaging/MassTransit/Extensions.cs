using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static  class Extensions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            // Use Kebab Case for endpoint names
            config.SetKebabCaseEndpointNameFormatter();

            // Register all consumers from the specified assembly
            if (assembly != null)
                config.AddConsumers(assembly);

            // Configure RabbitMQ as the message broker
            config.UsingRabbitMq((context, configurator) =>
            {
                // Configure the RabbitMQ host and credentials from configuration
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                // Configure endpoints for all registered consumers
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}