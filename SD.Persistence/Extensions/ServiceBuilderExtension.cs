using Microsoft.Extensions.DependencyInjection;
using SD.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SD.Persistence.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.Scan(scan =>
            {
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(c => c.WithAttribute<MapServiceDependencyAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
    }
}
