using System;

namespace Omnicx.API.SDK.Models.Common
{
    [Serializable]
    public class Amount
    {
        public Amount()
        {
            Formatted = new AmountFormatted();
            Raw = new AmountRaw();
        }
        public AmountFormatted Formatted { get; set; }
        public AmountRaw Raw { get; set; }
    }
    [Serializable]
    /// <summary>
    /// Contains the currency symbol & rounded number
    /// </summary>
    public class AmountFormatted
    {
        public string WithTax { get; set; }
        public string WithoutTax { get; set; }
        public string Tax { get; set; }
    }
    [Serializable]
    /// <summary>
    /// the raw amount figures
    /// </summary>
    public class AmountRaw
    {
        public decimal WithTax { get; set; }
        public decimal WithoutTax { get; set; }
        public decimal Tax { get; set; }
    }
}