using Newtonsoft.Json;
using Omnicx.API.SDK.Models;
using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Models.Site;
using RestSharp;
using System;

namespace Omnicx.API.SDK.Api.Site
{
    public class SurveyApi : ApiBase, ISurveyApi
    {
        public ResponseModel<SurveyModel> GetSurvey(Guid surveyId)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.Survey, surveyId), "", Method.GET);
        }

        public ResponseModel<SurveyModel> GetSurveyByCode(string surveyCode)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.SurveyByCode, surveyCode), "", Method.GET);
        }

        public ResponseModel<SurveyModel> GetSurveyProfile(Guid surveyProfileId)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.SurveyProfile, surveyProfileId), "", Method.GET);
        }

        public ResponseModel<BoolResponse> SaveAnswer(Guid visitorId, Guid userId, Guid surveyId, Guid questionId, string selectedAnswer)
        {
            //return CallApi<Survey>(string.Format(ApiUrls.Survey, surveyProfileId), "", Method.GET);
            throw new NotImplementedException();
        }

        public ResponseModel<BoolResponse> SaveAnswers(SurveyProfileModel surveyProfile)
        {
            return CallApi<BoolResponse>(string.Format(ApiUrls.SurveySaveAllAnswers), JsonConvert.SerializeObject(surveyProfile), Method.POST);
        }
    }
}
