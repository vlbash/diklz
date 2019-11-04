using System;
using System.Linq;
using App.Data.DTO.FDB;
using App.Data.Models.FDB;
using App.Core.Business.Services;
using Microsoft.AspNetCore.Authorization;
using App.Core.Security.Entities;
using App.Data.Contexts;
using App.Core.Business.Services.ObjectMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Business.Services.Common
{
    public class FeedBackService: IFeedBackService
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _userInfoService;
        private readonly MigrationDbContext _context;
        private readonly IObjectMapper _objectMapper;

        public FeedBackService(ICommonDataService dataService, MigrationDbContext context, IUserInfoService userInfoService, ISecurityDbContext dbcontext, IObjectMapper objectMapper)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _context = context;
            _objectMapper = objectMapper;
        }

        public async Task<Guid> CreateFeedback(Guid orgEmployeeId, Guid orgId, string appSort, Guid appId)
        {
            var model = new Feedback
            {
                AppId = appId,
                AppSort = appSort,
                OrgId = orgId,
                OrgEmployeeId = orgEmployeeId,
                Rating = 0,
                Comment = "",
                IsRated = false
            };

            try
            {
                model.Id = _dataService.Add<Feedback>(model);
                _dataService.SaveChanges();
            }
            catch
            {
                model.Id = Guid.Empty;
            }

            return model.Id;
        }

        public async Task<FeedBackDetailsDTO> GetFeedback(Guid id)
        {
            var feedBackDetailsDTO = new FeedBackDetailsDTO();
            var baseBackDetailsDTO = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);

            if (baseBackDetailsDTO == null)
            {
                return feedBackDetailsDTO;
            }

            feedBackDetailsDTO.Id = baseBackDetailsDTO.Id;
            feedBackDetailsDTO.AppId = baseBackDetailsDTO.AppId;
            feedBackDetailsDTO.AppSort = baseBackDetailsDTO.AppSort;
            feedBackDetailsDTO.Rating = baseBackDetailsDTO.Rating;
            feedBackDetailsDTO.Comment = baseBackDetailsDTO.Comment;
            feedBackDetailsDTO.IsRated = baseBackDetailsDTO.IsRated;

            if (baseBackDetailsDTO.OrgId != null)
            {
                feedBackDetailsDTO.OrgId = baseBackDetailsDTO.OrgId;
            }

            if (baseBackDetailsDTO.OrgEmployeeId != null)
            {
                feedBackDetailsDTO.OrgEmployeeId = baseBackDetailsDTO.OrgEmployeeId;
            }            

            return feedBackDetailsDTO;
        }

        public async Task<bool> CloseFeedback(Guid id, string comment, int rate)
        {
            var baseFeedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);

            if (baseFeedback == null)
            {
                return false;
            }

            baseFeedback.IsRated = true;
            baseFeedback.Comment = comment;
            baseFeedback.Rating = rate;

            try
            {
                // _context.Update(baseFeedback);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
