using AutoMapper;
using Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote;

namespace Gymify.API.Requests.Customers.CreateCustomerBehaviorNote
{
    public class CreateCustomerBehaviorNoteRequestMappingProfile : Profile
    {
        public CreateCustomerBehaviorNoteRequestMappingProfile()
        {
            CreateMap<CreateCustomerBehaviorNoteRequest, CreateCustomerBehaviorNoteCommand>();
        }
    }
}
