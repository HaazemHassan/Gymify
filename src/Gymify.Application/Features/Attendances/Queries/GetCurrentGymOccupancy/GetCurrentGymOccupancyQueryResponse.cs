namespace Gymify.Application.Features.Attendances.Queries.GetCurrentGymOccupancy
{
    public class GetCurrentGymOccupancyQueryResponse
    {
        public int Count { get; set; }

        public GetCurrentGymOccupancyQueryResponse()
        {
        }

        public GetCurrentGymOccupancyQueryResponse(int count)
        {
            Count = count;
        }
    }
}
