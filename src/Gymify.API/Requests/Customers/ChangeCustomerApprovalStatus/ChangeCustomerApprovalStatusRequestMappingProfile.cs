using AutoMapper;
using Gymify.Application.Features.Customers.Commands.ChangeCustomerApprovalStatus;

namespace Gymify.API.Requests.Customers.ChangeCustomerApprovalStatus
{
    public class ChangeCustomerApprovalStatusRequestMappingProfile : Profile
    {
        public ChangeCustomerApprovalStatusRequestMappingProfile()
        {
            CreateMap<ChangeCustomerApprovalStatusRequest, ChangeCustomerApprovalStatusCommand>();
        }
    }
}
