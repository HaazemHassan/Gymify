using PrimeFit.Application.Common.Idempotency;
using PrimeFit.Application.Common.Transaction;

namespace PrimeFit.Application.Features.Payments.Commands.FulfillPayment
{
    public class FulfillPaymentCommand : IdempotentCommand, ITransactionalRequest
    {
        public FulfillPaymentCommand(Guid requestId) : base(requestId) { }

        public string StripePaymentIntentId { get; set; } = null!;
    }
}
