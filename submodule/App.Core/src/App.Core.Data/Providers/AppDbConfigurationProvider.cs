using System;
using System.Linq;
using App.Core.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace App.Core.Data.Providers
{
    public class AppDbConfigurationProvider<TContext>: ConfigurationProvider where TContext: CoreDbContext
    {
        private readonly Action<DbContextOptionsBuilder> _options;


        public AppDbConfigurationProvider(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
            ChangeToken.OnChange(() => ConfigurationChangeHelper.ApplicationDbSettingsToken, Load);
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            _options(builder);

            try
            {
                using (var context =
                    (TContext)Activator.CreateInstance(typeof(TContext), new object[] {builder.Options}))
                {
                    context.Database.EnsureCreated();

                    var items = from value in context.ApplicationSettingValue
                        join setting in context.ApplicationSetting
                            on new {value.ApplicationSettingId, IsEnabled = true}
                            equals new {ApplicationSettingId = setting.Id, setting.IsEnabled}
                        select new {setting.Name, value.Value};

                    Data.Clear();
                    foreach (var item in items)
                    {
                        Data.Add(item.Name, item.Value.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
