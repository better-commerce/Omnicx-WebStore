using Omnicx.API.SDK.Models.Catalog;
using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Site
{
    public class SurveyModel
    {
        public SurveyModel()
        {
            //init the questions when the class instance is created
            Questions = new List<SurveyQuestionModel>();
        }
        public Guid RecordId { get; set; }
        public string Name { get; set; }
        public List<SurveyQuestionModel> Questions { get; set; }
        /// <summary>
        /// if set as false, the user details are captured at the end.
        /// </summary>
        public bool CaptureUserDetailsAtStart { get; set; }
        /// <summary>
        /// Present the survey as a wizard or all questions in single shot.
        /// </summary>
        public bool WizardStyle { get; set; }
        /// <summary>
        /// What happens after the survey questions have been answered.
        /// </summary>
        public SurveyOutputStyle Output { get; set; }

        public List<KeyValuePair<string, int>> DisplayGroups { get; set; }

        public string HelpText { get; set; }
        public string ThankYouText { get; set; }

        public string MainImage { get; set; }
        public string BackgroundImage { get; set; }
        public bool SubmitSurvey { get; set; }
        public int WrongOptionCount { get; set; }
        public string ImageBaseUrl { get; set; }
    }
    public enum SurveyOutputStyle
    {
        SearchProducts = 1,
        BuildABundle = 2,
        QualifyingQuestionnaire = 3
    }
    public class SurveyProfileModel
    {
        public SurveyProfileModel()
        {
            SelectedAnswers = new List<SurveyProfileAnswerModel>();
        }
        /// <summary>
        /// Unique id of the visitor filling up the survey 
        /// </summary>
        public Guid VisitorId { get; set; }

        /// <summary>
        /// UserId of the visitor, after user provides their credentials
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Email address in case that is requested at some point in survey.
        /// </summary>
        public string Email { get; set; }

        public Guid SurveyId { get; set; }

        public List<SurveyProfileAnswerModel> SelectedAnswers { get; set; }

        public Guid ProductId { get; set; }
    }
    public class SurveyProfileAnswerModel
    {
        public Guid QuestionId { get; set; }
        public string Question { get; set; }

        public string SelectedAnswer { get; set; }
        public List<KeyValuePair<string, int>> LinkedStockCodes { get; set; }

    }
    public class SurveyQuestionModel
    {
        public SurveyQuestionModel()
        {
            Preconditions = new List<QuestionPreconditionModel>();
            InputOptions = new List<SurveyInputOptionModel>();
            LinkedStockCodes = new List<KeyValuePair<string, int>>();
            
        }
        public Guid RecordId { get; set; }
        public string Question { get; set; }
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Used in case we want to group certain questions to be displayed together.
        /// </summary>
        public string DisplayGroup { get; set; }
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Set of conditions to be met for displaying teh question to the user.
        /// For ex. a question is to eb displayed ONLY if the answer to one of the previous question matches a specific value.
        /// Either there is no pre-condition or all conditions have to be met for a quetsion to be displayed.
        /// </summary>
        public List<QuestionPreconditionModel> Preconditions { get; set; }
        /// <summary>
        /// is it compulsory for user to answer
        /// </summary>
        public bool IsMandatory { get; set; }
        /// <summary>
        /// Image displayed along with the question
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Input type 
        /// </summary>
        public SurveyInputDataTypes InputDataType { get; set; }
        /// <summary>
        /// the way input type is presented to the user
        /// </summary>
        public SurveyInputStyle InputStyle { get; set; }
        /// <summary>
        /// Optiosn from which the user can select depending upon the input type selected
        /// </summary>
        public List<SurveyInputOptionModel> InputOptions { get; set; }
        /// <summary>
        /// Attribute code associated with the question
        /// </summary>
        public string LinkedAttributeCode { get; set; }
        /// <summary>
        /// if there are certain stock codes linked at the question level itself. List of stock code(string) and their respective qty (int, default=1)
        /// </summary>
        public List<KeyValuePair<string, int>> LinkedStockCodes { get; set; }
        /// <summary>
        /// Min number for the range in case inputType = NumberRange
        /// </summary>
        public decimal RangeMinNumber { get; set; }
        /// <summary>
        /// Max number for the range in case inputType = NumberRange
        /// </summary>
        public decimal RangeMaxNumber { get; set; }
        /// <summary>
        /// Help text for the question
        /// </summary>
        public string HelpText { get; set; }
        /// <summary>
        /// Default value for the answer, in case its been defined. 
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// The answer selected at the time of survey being answered by a visitor. 
        /// This field NOT to be used at the time of defining the question / answers
        /// </summary>
        public string SelectedOptionValue { get; set; }

        public string CssClass { get; set; }

        public SurveyInputDataAndStyle InputDataAndStyle { get; set; }

        public bool ShowHelpText { get; set; }
    }
    /// <summary>
    /// Represents the pre-condition for displaying a question
    /// </summary>
    public class QuestionPreconditionModel
    {
        public Guid QuestionId { get; set; }
        public string SelectedAnswer { get; set; }
    }

    public class SurveyInputOptionModel
    {
        public SurveyInputOptionModel()
        {
            LinkedStockCodes = new List<KeyValuePair<string, int>>();
        }
        public Guid RecordId { get; set; }
        public string OptionText { get; set; }

        public string OptionValue { get; set; }

        /// <summary>
        /// Attribute option value associated with the specific option
        /// </summary>
        public string LinkedAttributeOptionValue { get; set; }

        /// <summary>
        /// if there are certain stock codes linked at the question level itself. List of stock code(string) and their respective qty (int, default=1)
        /// </summary>
        public List<KeyValuePair<string, int>> LinkedStockCodes { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// If any image associated with the option
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Help Text for the option
        /// </summary>
        public string HelpText { get; set; }

        public string CssClass { get; set; }

        public bool StopAddToBag { get; set; }

    }

    public class SurveyResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SurveyProductModel> Products { get; set; }
        public decimal Price
        {
            get
            {
                if (Products == null) return 0;
                decimal totalPrice = 0;
                foreach(var item in Products)
                {
                    if(item.Product!=null)
                        totalPrice = totalPrice + (item.Product.Price.Raw.WithTax * ((decimal)item.qty));
                }
                return totalPrice;
            }
        }
        public decimal PriceExcludingVat
        {
            get
            {
                if (Products == null) return 0;
                decimal totalPrice = 0;
                foreach (var item in Products)
                {
                    if (item.Product != null)
                        totalPrice = totalPrice + (item.Product.Price.Raw.WithoutTax * ((decimal)item.qty));
                }
                return totalPrice;
            }
        }
       
    }
    public class SurveyProductModel
    {
        public int qty { get; set; }
        public ProductModel Product{ get; set; }
}
    public enum SurveyInputStyle
    {
        TextInput = 1, // for number & string
        Multiline = 1, // for string
        AsDropdown = 2, // for SingleSelect & Multiselect
        AsImage = 3, // for SingleSelect & Multiselect
        AsCarousel = 4, // for SingleSelect & Multiselect
        AsText = 5, // for SingleSelect & Multiselect
        RatingScale = 6, // for Number 
        /// <summary>
        /// allows to capture a single number from min & max.
        /// </summary>
        SingleSlider = 7, // for Number 
        /// <summary>
        /// allows to accept min & max (range) from the overall min & max
        /// </summary>
        DoubleSlider = 8 // for 2 numbers, basically a range. generally useful for a price range to capture minimum & maximum 
    }
    public enum SurveyInputDataTypes
    {
        String = 1,
        Number = 2,
        YesNo = 3,
        OptionsSingleSelect = 4,
        OptionsMultipleSelect = 5,

    }
    public enum SurveyInputDataAndStyle
    {
        String_TextInput = 1,
        String_Multiline = 2,
        Number_TextInput = 3,
        Number_RatingScale = 4,
        Number_SingleSlider = 5,
        Number_DoubleSlider = 6,
        OptionsSingleSelect_AsDropdown = 7,
        OptionsSingleSelect_AsImage = 8,
        OptionsSingleSelect_AsCarousel = 9,
        OptionsSingleSelect_AsText = 10,
        OptionsMultipleSelect_AsDropdown = 11,
        OptionsMultipleSelect_AsImage = 12,
        OptionsMultipleSelect_AsCarousel = 13,
        OptionsMultipleSelect_AsText = 14,
    }
}
