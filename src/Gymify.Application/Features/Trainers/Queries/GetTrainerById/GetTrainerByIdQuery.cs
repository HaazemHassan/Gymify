using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Trainers.Queries.GetTrainerById
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    public class GetTrainerByIdQuery : IQuery<GetTrainerByIdQueryResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }

        public GetTrainerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
