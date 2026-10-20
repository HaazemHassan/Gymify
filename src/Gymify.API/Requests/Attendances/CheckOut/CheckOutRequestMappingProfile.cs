using AutoMapper;
using Gymify.Application.Features.Attendances.Commands.CheckOut;

namespace Gymify.API.Requests.Attendances.CheckOut
{
    public class CheckOutRequestMappingProfile : Profile
    {
        public CheckOutRequestMappingProfile()
        {
            CreateMap<CheckOutRequest, CheckOutCommand>();
        }
    }
}
