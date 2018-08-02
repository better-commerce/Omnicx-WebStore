namespace Omnicx.WebStore.Models.Enums
{
    public enum OrderStatus
    {

        Unknown = 0,
        Incomplete = 1,
        /// <summary>
        /// Pending
        /// </summary>
        PreOrderUntrusted = 10,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40,
        Pending = 2,
        Approved = 3,
        SentToWarehose = 4,
        AcceptedInWarehouse = 5,
        OrderSettled = 7,
        Dispatch = 9,
        ReadyToDispatch = 12,
        IdRequired = 107,
        AwaitingSettlement = 6,
        PreOrderApproved = 11,
        CancelledByStore = 102,
        CancelledByStorePaymentIssue = 103,
        CancelledByStoreStockIssue = 104,
        CancelledByCustomer = 105,
        CancelledFailedFraudScreening = 110,
    }
}
