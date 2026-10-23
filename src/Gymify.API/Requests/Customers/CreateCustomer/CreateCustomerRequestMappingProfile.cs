using AutoMapper;
using Gymify.Application.Features.Customers.Commands.CreateCustomer;

namespace Gymify.API.Requests.Customers.CreateCustomer
{
    public class CreateCustomerRequestMappingProfile : Profile
    {
        public CreateCustomerRequestMappingProfile()
        {
            CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
        }
    }
}
