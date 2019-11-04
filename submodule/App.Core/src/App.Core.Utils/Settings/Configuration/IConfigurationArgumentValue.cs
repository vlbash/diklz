using System;

namespace App.Core.Utils.Settings.Configuration
{
    interface IConfigurationArgumentValue
    {
        object ConvertTo(Type toType);
    }
}
