using Newtonsoft.Json;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Site;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api.Site
{
    public class SurveyApi : ApiBase, ISurveyApi
    {
        public ResponseModel<SurveyModel> GetSurvey(Guid surveyId)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.Survey, surveyId), "", Method.GET, apiBaseUrl: ConfigKeys.MktApiBaseUrl, isAuthenticationEnabled: true);
        }

        public ResponseModel<SurveyModel> GetSurveyByCode(string surveyCode)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.SurveyByCode, surveyCode), "", Method.GET);
        }

        public ResponseModel<SurveyModel> GetSurveyProfile(Guid surveyProfileId)
        {
            return CallApi<SurveyModel>(string.Format(ApiUrls.SurveyProfile, surveyProfileId), "", Method.GET);
        }
        
        public ResponseModel<BoolResponse> SaveAnswers(SurveyAnswerModel surveyAnswer)
        {
            return CallApi<BoolResponse>(string.Format(ApiUrls.SurveySaveAnswer), JsonConvert.SerializeObject(surveyAnswer), Method.POST, apiBaseUrl: ConfigKeys.MktApiBaseUrl, isAuthenticationEnabled: true);
        }
        public ResponseModel<List<SurveyAnswerModel>> UserResponse(string userName, Guid surveyId)
        {
            return CallApi<List<SurveyAnswerModel>>(string.Format(ApiUrls.SurveyUserResponse, userName, surveyId), "", Method.GET, apiBaseUrl: ConfigKeys.MktApiBaseUrl, isAuthenticationEnabled: true);
        }
        public ResponseModel<List<SurveyToolData>> SurveyToolBoxData()
        {
            return CallApi<List<SurveyToolData>>(string.Format(ApiUrls.ToolboxDataUrl, ConfigKeys.OmnicxOrgId, ConfigKeys.OmnicxDomainId), "", Method.GET, apiBaseUrl: ConfigKeys.BussinessHubApiBaseUrl, isAuthenticationEnabled: true);
        }
    }
}
