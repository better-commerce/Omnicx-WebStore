
using Omnicx.WebStore.Core.Services.Authentication;
using System;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Models.Infrastructure;
using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Entities;
namespace Omnicx.WebStore.Core.Services.Log
{
    public partial class LogService: ILogService
    {
        #region "Feilds"
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly ILogApi _repository;

        public LogService(ILogApi repository, IAuthenticationService authenticationService, ISessionContext sessionContext)
        {
            _repository = repository;
            _authenticationService = authenticationService;
            _sessionContext = sessionContext;
        }
        #endregion

        public BoolResponse InsertLog(LogLevel logLevel, string shortMessage, string fullMessage, string userName, string userId)
        {
            var log = new LogEntry()
            {
                LogLevelId = logLevel.GetHashCode(),
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = Utils.GetCurrentIpAddress(),
                UserId = _sessionContext.CurrentUser == null ? new Guid() : _sessionContext.CurrentUser.UserId,
                UserName = userName,
                Additionalinfo1 = Environment.MachineName,
                Additionalinfo2 = "",
                PageUrl = Utils.GetThisPageUrl(true),
                ReferrerUrl = Utils.GetUrlReferrer(),
                Created = DateTime.UtcNow
            };
            var response= _repository.InsertLog(log);
            return response.Result;
        }


    }
}