using Gymify.Application.Common.CQRS;

namespace Gymify.Application.Features.Packages.Queries.GetPackageById
{
    public class GetPackageByIdQuery : IQuery<GetPackageByIdQueryResponse>
    {
        public int Id { get; set; }

        public GetPackageByIdQuery()
        {
        }

        public GetPackageByIdQuery(int id)
        {
            Id = id;
        }
    }
}
