using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Site;
using System;

namespace Omnicx.API.SDK.Api.Site
{
    public interface ISurveyApi
    {
        ResponseModel<SurveyModel> GetSurvey(Guid surveyId);

        ResponseModel<SurveyModel> GetSurveyByCode(string surveyCode);

        ResponseModel<SurveyModel> GetSurveyProfile(Guid surveyProfileId);

        ResponseModel<BoolResponse> SaveAnswer(Guid visitorId, Guid userId, Guid surveyId, Guid questionId, string selectedAnswer);

        ResponseModel<BoolResponse> SaveAnswers(SurveyProfileModel surveyProfile);
    }
}
