using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using DotnetBoilerplate.Application;
using DotnetBoilerplate.Infrastructure.Interfaces;
using DotnetBoilerplate.Infrastructure.Mediator.Behavior;
using DotnetBoilerplate.Infrastructure.Mediator.Notifications;

namespace DotnetBoilerplate.API.Setup
{
    public static class MediatorSetup
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AssemblyScanner.FindValidatorsInAssembly(typeof(ApplicationInfo).Assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidateCommandPipelineBehavior<,>));
            services.AddMediatR(typeof(ApplicationInfo).Assembly);

            return services;
        }
    }
}