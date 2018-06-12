using System;
using System.Collections.Generic;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Commerce
{
     public class UserActivityModel
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        /// <summary>
        /// dervided from IP Address
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// derived from IP address
        /// </summary>
        public string Country { get; set; }
        public string BrowserInfo { get; set; }
        public string Channel { get; set; }
        /// <summary>
        /// derived from 1st activity & Last activity created
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Highlight the session if an order was placed
        /// </summary>
        public bool Highlight { get; set; }
        public int OrdersPlacedCount { get; set; }
        public int ActivityCount { get; set; }
        public DateTime FirstActivityCreated { get; set; }
        public DateTime LastActivityCreated { get; set; }
        /// <summary>
        /// Derived based on Created - morning, noon, evening, night for display icons. 
        /// </summary>
        public List<Activity> Activities { get; set; }
        public string UrlReferrer { get; set; }
        public int TotalRecord { get; set; }
    }

    public class Activity {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string CustomerId { get; set; }
        public string EntityId { get; set; }
        public string MicrositeId { get; set; }
        public string AppId { get; set; }


        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string LangCulture { get; set; }
        public string CurrencyCode { get; set; }

        public string IpAddress { get; set; }
        public string SearchText { get; set; }

        public string Channel { get; set; }
        public string BrowserInfo { get; set; }
        
        public DateTime Created { get; set; }
        public Object Data { get; set; }
        public string UrlReferrer { get; set; }
        public WebhookEntityTypes EntityType { get; set; }

    }
}
