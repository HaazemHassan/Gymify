namespace Gymify.Application.Security.Policies
{
    public static class AuthorizationPolicies
    {
        public const string SelfOrAdmin = nameof(SelfOrAdmin);
        public const string SelfOnly = nameof(SelfOnly);
        public const string SelfCustomerOnly = nameof(SelfCustomerOnly);
        public const string SelfTrainerOnly = nameof(SelfTrainerOnly);
        public const string SelfSubscriptionOnly = nameof(SelfSubscriptionOnly);
    }
}
