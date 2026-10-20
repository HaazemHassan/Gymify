
namespace PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment
{
    public class InitializeSubscriptionPaymentCommand : ICommand<InitializeSubscriptionPaymentCommandResponse>
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int PackageId { get; set; }
    }
}
