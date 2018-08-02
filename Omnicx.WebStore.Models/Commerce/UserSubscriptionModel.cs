using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System;
using System.Collections.Generic;


namespace Omnicx.WebStore.Models.Commerce
{
    public class UserSubscriptionModel
    {
        public Guid SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public SubscriptionTypes SubscriptionType { get; set; }
        public int Terms { get; set; }
        public bool IsActive { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public int OrderCount { get; set; }
        public DateTime NextOrderDate { get; set; }
        public List<SubscriptionItem> Items { get; set; }
    }
    public class SubscriptionItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int ProductIId { get; set; }
        public string ProductName { get; set; }
        public int DisplayOrder { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
    }
}