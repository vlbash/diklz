using System;
using System.Reflection;
using Serilog;
using Serilog.Configuration;

namespace App.Core.Utils.Settings.Configuration
{
    class ConfigurationSectionArgumentValue : IConfigurationArgumentValue
    {
        readonly IConfigurationReader _configReader;

        public ConfigurationSectionArgumentValue(IConfigurationReader configReader)
        {
            _configReader = configReader ?? throw new ArgumentNullException(nameof(configReader));
        }

        public object ConvertTo(Type toType)
        {
            var typeInfo = toType.GetTypeInfo();
            if (!typeInfo.IsGenericType || 
                 typeInfo.GetGenericTypeDefinition() is Type genericType && genericType != typeof(Action<>))
            {
                throw new InvalidOperationException("Argument value should be of type Action<>.");
            }

            var configurationType = typeInfo.GenericTypeArguments[0];
            if (configurationType == typeof(LoggerSinkConfiguration))
            {
                return new Action<LoggerSinkConfiguration>(_configReader.ApplySinks);
            }

            if (configurationType == typeof(LoggerConfiguration))
            {
                return new Action<LoggerConfiguration>(_configReader.Configure);
            }

            throw new ArgumentException($"Handling {configurationType} is not implemented.");
        }
    }
}
