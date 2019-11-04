using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace App.Core.Business.Providers
{
    public interface IRedisdatabaseProvider
    {
        IDatabase GetDatabase();
    }
}
