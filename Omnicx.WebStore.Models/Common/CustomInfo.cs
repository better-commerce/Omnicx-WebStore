using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Omnicx.WebStore.Models.Common
{
    
    public class LineCustomInfo
    {
        public string ProductId { get; set; } 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo1 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo2 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo3 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo4 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo5 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo1Formatted { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo2Formatted { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo3Formatted { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo4Formatted { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CustomInfo5Formatted { get; set; }
        public int Qty { get; set; }
        public decimal AdditionalCharge { get; set; }
        public string StockCode { get; set; }

        public string ServiceType { get; set; }
        public int LengthMm { get; set; }
        public decimal AdditionalServiceCost { get; set; }
    }

    public class HeaderCustomInfo
    {
        public Guid BasketId { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }
        public List<LineCustomInfo> LineInfo { get; set; }
        public bool UserSelection { get; set; }
    }
}
