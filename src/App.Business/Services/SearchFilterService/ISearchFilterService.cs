using System;

namespace App.Business.Services.SearchFilterService
{
    public interface ISearchFilterService
    {
        string GenerateInputConfig(Type dtoType);
    }
}
