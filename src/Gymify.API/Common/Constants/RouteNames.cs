namespace Gymify.API.Common.Constants
{
    public static class RouteNames
    {
        public static class Users
        {
            public const string GetUserById = nameof(GetUserById);
        }

        public static class Packages
        {
            public const string GetPackageById = nameof(GetPackageById);
            public const string GetPackageByIdForManagement = nameof(GetPackageByIdForManagement);
        }

        public static class Customers
        {
            public const string GetCustomerById = nameof(GetCustomerById);
        }
    }
}
