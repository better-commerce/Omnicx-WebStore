using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Omnicx.WebStore.Core.Social
{
    public class TwitterResponse : TableEntity
    {
        public string SocialId { get; set; }
        public string RecordId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Source { get; set; }

        public TwitterResponse()
        {
            //partition key of same partition should be same, need to change this
            this.PartitionKey = Convert.ToString(Guid.NewGuid());
            this.RowKey = Convert.ToString(Guid.NewGuid());
        }
    }

    public class FacebookResponse : TableEntity
    {
        public string SocialId { get; set; }
        public string RecordId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string HomeTown { get; set; }
        public string DateOfBirth { get; set; }
        public string Source { get; set; }


        public FacebookResponse()
        {
            //partition key of same partition should be same, need to change this
            this.PartitionKey = Convert.ToString(Guid.NewGuid());
            this.RowKey = Convert.ToString(Guid.NewGuid());
        }
    }

    public class GoogleResponse: TableEntity
    {
        public string SocialId { get; set; }
        public string RecordId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Source { get; set; }

        public GoogleResponse()
        {
            //partition key of same partition should be same, need to change this
            this.PartitionKey = Convert.ToString(Guid.NewGuid());
            this.RowKey = Convert.ToString(Guid.NewGuid());
        }
    }

    
}