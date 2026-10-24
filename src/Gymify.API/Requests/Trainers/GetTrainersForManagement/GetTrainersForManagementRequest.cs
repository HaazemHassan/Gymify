using Gymify.Api.Requests.Common.Pagination;

namespace Gymify.API.Requests.Trainers.GetTrainersForManagement
{
    public class GetTrainersForManagementRequest : BasicPaginationRequest
    {
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
