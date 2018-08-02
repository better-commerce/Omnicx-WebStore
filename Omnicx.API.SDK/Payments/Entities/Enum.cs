namespace Omnicx.API.SDK.Payments.Entities
{
    public enum PaymentMethodTypes
    {
        COD,
        MasterCard,
        Paypal,
        Realex,
        Givex,
        AccountCredit,
        Cheque,
        Worldpay,
        Klarna
    }
    public enum PaymentStatus : int
    {
        Pending = 0,
        Authorized = 1,
        Paid = 2,
        Declined = 3,
        ProblemInPostAuth = 4
    }
    public enum ErrorCodesPayment
    {
        InvalidConfigInfo = 1001,
        InvalidResponseFromPSP = 1002,
        BlankResponseFromPSP = 1003,
        RejectedByPSP = 1004,
        ExceptionInServiceCall = 1005,
        UnknownPSP = 1006,
        EndPointNotFound = 1007
    }
}
