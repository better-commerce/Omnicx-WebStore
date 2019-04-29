using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Site;
using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api.Site
{
    public interface ISurveyApi
    {
        ResponseModel<SurveyModel> GetSurvey(Guid surveyId);

        ResponseModel<SurveyModel> GetSurveyByCode(string surveyCode);

        ResponseModel<SurveyModel> GetSurveyProfile(Guid surveyProfileId);     

        ResponseModel<BoolResponse> SaveAnswers(SurveyAnswerModel surveyAnswer);

        ResponseModel<List<SurveyAnswerModel>> UserResponse(string userName, Guid surveyId);
        ResponseModel<List<SurveyToolData>> SurveyToolBoxData();
    }
}
