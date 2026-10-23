using Ardalis.Specification;
using Gymify.Application.Features.Kpis.Specifications;
using Gymify.Application.Specifications;

namespace Gymify.Application.Features.Kpis.Queries.GetKpisPaginated.Specifications
{
    public class KpisPaginatedSpec : KpisFilteredSpec
    {
        public KpisPaginatedSpec(int pageNumber, int pageSize, string? search = null, bool? isActive = null)
            : base(search, isActive)
        {
            Query.OrderBy(k => k.Name);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
