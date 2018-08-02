using System;

namespace Omnicx.WebStore.Models.Infrastructure
{
    public partial class LogEntry
    {
        public Guid LogId { get; set; }
        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public int? LogLevelId { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the object identifier attached with the log entry
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the request data
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// Gets or sets the page URL
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer URL
        /// </summary>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the User Guid Identification
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// Gets or sets the User Name 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Additionalinfo1 level
        /// </summary>
        public string Additionalinfo1 { get; set; }

        /// <summary>
        /// Gets or sets the Additionalinfo2 level
        /// </summary>
        public string Additionalinfo2 { get; set; }
    }
}
