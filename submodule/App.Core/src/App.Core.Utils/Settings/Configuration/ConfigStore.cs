using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace App.Core.Utils.Settings.Configuration
{
    public static class ConfigStore
    {
        public static IConfigurationRoot Configuration { get; set; }
    }
}
