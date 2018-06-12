using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Common
{
    public class BoolResponse 
    {
        public string RecordId { get; set; }
        public AcknowledgeType Acknowledge { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public string ReturnUrl { get; set; }
        
    }
}