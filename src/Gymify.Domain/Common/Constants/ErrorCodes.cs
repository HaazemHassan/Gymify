namespace Gymify.Domain.Common.Constants;

public static class ErrorCodes
{
    public static class User
    {
        public const string EmailAlreadyExists = "USERS_EMAIL_ALREADY_EXISTS";
        public const string PhoneAlreadyExists = "USERS_PHONE_ALREADY_EXISTS";
        public const string UserNotFound = "USERS_USER_NOT_FOUND";
    }

    public static class Customer
    {
        public const string CustomerNotFound = "CUSTOMER_NOT_FOUND";
    }

    public static class Package
    {
        public const string PackageNotFound = "PACKAGE_NOT_FOUND";
        public const string PackageNameAlreadyExists = "PACKAGE_NAME_ALREADY_EXISTS";
    }

    public static class Authentication
    {
        public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
        public const string InvalidAccessToken = "AUTH_INVALID_ACCESS_TOKEN";
        public const string InvalidRefreshToken = "AUTH_INVALID_REFRESH_TOKEN";
        public const string AlreadySignedOut = "AUTH_ALREADY_SIGNED_OUT";
        public const string InvalidPassword = "AUTH_PASSWORD_CHANGE_FAILED";
        public const string EmailNotConfirmed = "AUTH_EMAIL_NOT_VERIFIED";
        public const string EmailAlreadyConfirmed = "AUTH_EMAIL_ALREADY_CONFIRMED";
        public const string InvalidEmailConfirmationCode = "AUTH_INVALID_EMAIL_CONFIRMATION_CODE";
        public const string EmailCodeAttemptsExceeded = "AUTH_EMAIL_CODE_ATTEMPTS_EXCEEDED";
        public const string AccountInactive = "AUTH_ACCOUNT_INACTIVE";
        public const string AccountPending = "AUTH_ACCOUNT_PENDING";
        public const string AccountRejected = "AUTH_ACCOUNT_REJECTED";
    }

    public static class Authorization
    {
        public const string MissingPermissions = "AUTHZ_MISSING_PERMISSIONS";
        public const string MissingRoles = "AUTHZ_MISSING_ROLES";
        public const string InvalidUserType = "AUTHZ_INVALID_USER_TYPE";
        public const string NotAllowed = "AUTHZ_NOT_ALLOWED";
        public const string InvalidRequest = "AUTHZ_INVALID_REQUEST";
        public const string UnknownPolicy = "AUTHZ_UNKNOWN_POLICY";
        public const string NotAuthenticated = "AUTHZ_NOT_AUTHENTICATED";
    }

    public static class Cloudinary
    {
        public const string ImageUploadFailed = "CLOUDINARY_IMAGE_UPLOAD_FAILED";
        public const string ImageDeleteFailed = "CLOUDINARY_IMAGE_DELETE_FAILED";
    }

    public static class Payment
    {
        public const string TransactionNotFound = "PAYMENT_TRANSACTION_NOT_FOUND";
        public const string PaymentIntentCreationFailed = "PAYMENT_INTENT_CREATION_FAILED";
        public const string TransactionAlreadyProcessed = "PAYMENT_TRANSACTION_ALREADY_PROCESSED";
    }
}
