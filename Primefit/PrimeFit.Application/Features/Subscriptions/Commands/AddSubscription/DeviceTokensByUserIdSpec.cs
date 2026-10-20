using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class DeviceTokensByUserIdSpec : Specification<UserDeviceToken>
    {
        public DeviceTokensByUserIdSpec(int userId)
        {
            Query.Where(t => t.UserId == userId);
        }
    }
}
