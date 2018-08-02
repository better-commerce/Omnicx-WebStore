using System;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class B2BSettings : ISettings
    {
        public bool EnableB2B { get; set; }
        public string DefaultCustomerOwner { get; set; }
        public bool RegistrationAllowed { get; set; }
        public bool ConfirmationRequired { get; set; }
        public bool RequireCompanyName { get; set; }
        public bool ShowRegistrationInstructions { get; set; }

        public bool ShowBulkOrderPad { get; set; }
        public bool AllowReorder { get; set; }
    }
}
