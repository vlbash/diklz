using System;
using System.Threading.Tasks;
using App.Data.DTO.FDB;

namespace App.Business.Services.Common
{
    public interface IFeedBackService
    {
        Task<Guid>                  CreateFeedback(Guid orgEmployeeId, Guid orgId, string appSort, Guid appId);
        Task<FeedBackDetailsDTO>    GetFeedback(Guid id);
        Task<bool>                  CloseFeedback(Guid id, string comment, int rate);
    }
}
