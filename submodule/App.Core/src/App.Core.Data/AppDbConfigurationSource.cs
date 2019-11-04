using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using App.Core.Data.Providers;

namespace App.Core.Data
{
    public class AppDbConfigurationSource<TContext>: IConfigurationSource where TContext: CoreDbContext
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public AppDbConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AppDbConfigurationProvider<TContext>(_optionsAction);
        }
    }
}
