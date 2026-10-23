namespace Gymify.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandResponse
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }

        public CreateCustomerCommandResponse()
        {
        }

        public CreateCustomerCommandResponse(int customerId, int userId)
        {
            CustomerId = customerId;
            UserId = userId;
        }
    }
}
