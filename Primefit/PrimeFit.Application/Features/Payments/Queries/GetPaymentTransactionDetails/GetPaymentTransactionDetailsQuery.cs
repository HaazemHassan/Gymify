
namespace PrimeFit.Application.Features.Payments.Queries.GetPaymentTransactionDetails
{
    public class GetPaymentTransactionDetailsQuery : IQuery<GetPaymentTransactionDetailsQueryResponse>
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
    }
}
