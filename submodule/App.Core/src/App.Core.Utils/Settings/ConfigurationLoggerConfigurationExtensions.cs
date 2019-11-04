using System;
using System.Reflection;
using App.Core.Utils.Settings.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using Serilog.Configuration;

namespace App.Core.Utils.Settings
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration"/> with support for System.Configuration appSettings elements.
    /// </summary>
    public static class ConfigurationLoggerConfigurationExtensions
    {
        const string DefaultSectionName = "Serilog";

        /// <summary>
        /// Reads logger settings from the provided configuration object using the default section name.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">A configuration object with a Serilog section.</param>
        /// <param name="dependencyContext">The dependency context from which sink/enricher packages can be located. If not supplied, the platform
        /// default will be used.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration Configuration(
            this LoggerSettingsConfiguration settingConfiguration,
            IConfiguration configuration,
            DependencyContext dependencyContext = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            return settingConfiguration.ConfigurationSection(configuration.GetSection(DefaultSectionName), dependencyContext);
        }

        /// <summary>
        /// Reads logger settings from the provided configuration section.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">The Serilog configuration section</param>
        /// <param name="dependencyContext">The dependency context from which sink/enricher packages can be located. If not supplied, the platform
        /// default will be used.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration ConfigurationSection(
            this LoggerSettingsConfiguration settingConfiguration,
            IConfigurationSection configuration,
            DependencyContext dependencyContext = null)
        {
            if (settingConfiguration == null) throw new ArgumentNullException(nameof(settingConfiguration));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            return settingConfiguration.Settings(
                new ConfigurationReader(
                    configuration,
                    dependencyContext ?? (Assembly.GetEntryAssembly() != null ? DependencyContext.Default : null)));
        }
    }
}
