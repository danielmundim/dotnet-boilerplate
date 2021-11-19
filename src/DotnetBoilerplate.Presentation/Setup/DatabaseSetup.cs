using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DotnetBoilerplate.API.Setup
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IDbConnection>(sp => new MySqlConnection(configuration.GetSection("ConnectionStrings:MyConnectionString").Value));
        }
    }
}