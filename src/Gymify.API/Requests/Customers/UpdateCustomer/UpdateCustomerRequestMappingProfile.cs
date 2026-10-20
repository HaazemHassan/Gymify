using AutoMapper;
using Gymify.Application.Features.Customers.Commands.UpdateCustomer;

namespace Gymify.API.Requests.Customers.UpdateCustomer
{
    public class UpdateCustomerRequestMappingProfile : Profile
    {
        public UpdateCustomerRequestMappingProfile()
        {
            CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();
        }
    }
}
