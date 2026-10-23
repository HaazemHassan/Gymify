using Gymify.Application.Common.CQRS;
using Gymify.Application.Security;
using Gymify.Application.Security.Markers;
using Gymify.Domain.Common.Enums;

namespace Gymify.Application.Features.Kpis.Queries.GetKpiById
{
    [Authorize(Permissions = [Permission.TrainersRead])]
    public class GetKpiByIdQuery : IQuery<GetKpiByIdQueryResponse>, IAuthorizedRequest
    {
        public int Id { get; set; }

        public GetKpiByIdQuery(int id)
        {
            Id = id;
        }

        public GetKpiByIdQuery()
        {
        }
    }
}
