using AutoMapper;
using Gymify.Application.Features.Authentication.Commands.RegisterCustomer;

namespace Gymify.Api.Requests.Authentication.RegisterCustomer;

public class RegisterCustomerRequestMappingProfile : Profile
{
    public RegisterCustomerRequestMappingProfile()
    {
        CreateMap<RegisterCustomerRequest, RegisterCustomerCommand>();
    }
}
