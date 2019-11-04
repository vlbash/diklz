using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Core.Data.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddAppDbProvider<TContext>(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup) where TContext: CoreDbContext
        {
            configuration.Add(new AppDbConfigurationSource<TContext>(setup));
            return configuration;
        }
    }
}
