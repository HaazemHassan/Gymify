using Ardalis.Specification;
using Gymify.Application.Common.Pagination;

namespace Gymify.Application.Specifications
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> Paginate<T>(
            this ISpecificationBuilder<T> builder, int? pageNumber, int? pageSize)
        {

            if (pageNumber is null || pageNumber <= 0)
                pageNumber = 1;


            if (pageSize is null || pageSize < PaginationConstants.MinimumPageSize)
            {
                pageSize = PaginationConstants.DefaultPageSize;

            }
            else if (pageSize > PaginationConstants.MaximumPageSize)
            {
                pageSize = PaginationConstants.MaximumPageSize;

            }

            return builder.Skip((pageNumber.Value - 1) * pageSize.Value)
                          .Take(pageSize.Value);
        }

        public static ISpecificationBuilder<T, TResult> Paginate<T, TResult>(
            this ISpecificationBuilder<T, TResult> builder, int? pageNumber, int? pageSize)
        {
            if (pageNumber is null || pageNumber <= 0)
                pageNumber = 1;

            if (pageSize is null || pageSize < PaginationConstants.MinimumPageSize)
            {
                pageSize = PaginationConstants.DefaultPageSize;
            }
            else if (pageSize > PaginationConstants.MaximumPageSize)
            {
                pageSize = PaginationConstants.MaximumPageSize;
            }

            return builder.Skip((pageNumber.Value - 1) * pageSize.Value)
                          .Take(pageSize.Value);
        }
    }
}