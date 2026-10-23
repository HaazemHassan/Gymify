using AutoMapper;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.CustomerBehaviorNotes.Commands.CreateCustomerBehaviorNote
{
    public class CreateCustomerBehaviorNoteCommandMappingProfile : Profile
    {
        public CreateCustomerBehaviorNoteCommandMappingProfile()
        {
            CreateMap<CustomerBehaviorNote, CreateCustomerBehaviorNoteCommandResponse>();
        }
    }
}
