using System.Linq;

namespace Omnicx.API.SDK.Models.Catalog
{
    /// <summary>
    /// These properties have been setup in a separate partial class to keep the base ProductDetailModel clean
    /// </summary>
    public partial class ProductDetailModel
    {
        private bool _requiresQuestionnaire;
        private bool _canAddToBag;
        private string _questionnaireCode;
        /// <summary>
        /// Linked based on a value defined in custom attributes
        /// </summary>
        public bool RequiresQuestionnaire
        {
            get
            {
                if (Attributes == null || Attributes.Count == 0) return _requiresQuestionnaire;
                
                var reqQuestionnaireAttr = Attributes.FirstOrDefault(x => x.Key.ToLower() == "requires.questionnaire");
                if(reqQuestionnaireAttr != null)
                {
                    if (!string.IsNullOrEmpty(reqQuestionnaireAttr.Value))
                    {
                        var value = reqQuestionnaireAttr.Value.ToLower();
                        if (value == "true" || value == "1" || value == "y" || value == "yes") _requiresQuestionnaire = true;
                        else _requiresQuestionnaire = false;
                    }
                    
                }
                if (StockCode.Contains("med"))
                {
                    _requiresQuestionnaire = true;
                }
                return _requiresQuestionnaire;
            }
            set { _requiresQuestionnaire = value; }
        }
        
        /// <summary>
        /// determined based on the answers given to the questionnaire
        /// </summary>
        public bool CanAddToBag
        {
            get
            {
                _canAddToBag = true; // by default every item is enabled for "AddToBag"

                //if a product requires a questionnaire its assumed that, "AddToBag" is dependent upon the answers given in questionnaire
                if (_requiresQuestionnaire == true) _canAddToBag = false;
                return _canAddToBag;
            }
            set
            {
                _canAddToBag = value;
            }
        }

        public string QuestionnaireCode {
            get {
                if (Attributes == null || Attributes.Count == 0) return _questionnaireCode;

                var attr = Attributes.FirstOrDefault(x => x.Key.ToLower() == "questionnaire.code");
                if (attr != null)
                {
                    if (!string.IsNullOrEmpty(attr.Value))
                    {
                        _questionnaireCode = attr.Value.ToLower();
                    }
                }
                
                if (StockCode.Contains("med0"))
                {
                    _questionnaireCode = "abc123";
                }
                    return _questionnaireCode;
            }

            set { _questionnaireCode = value; }
        }
    }
}
