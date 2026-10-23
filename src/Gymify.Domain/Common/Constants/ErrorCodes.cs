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
        public const string CodeAlreadyExists = "CUSTOMER_CODE_ALREADY_EXISTS";
    }

    public static class Package
    {
        public const string PackageNotFound = "PACKAGE_NOT_FOUND";
        public const string PackageNameAlreadyExists = "PACKAGE_NAME_ALREADY_EXISTS";
    }

    public static class Locker
    {
        public const string LockerNotFound = "LOCKER_NOT_FOUND";
        public const string LockerInactive = "LOCKER_INACTIVE";
        public const string LockerAlreadyOccupied = "LOCKER_ALREADY_OCCUPIED";
        public const string AlreadyActive = "LOCKER_ALREADY_ACTIVE";
        public const string AlreadyInactive = "LOCKER_ALREADY_INACTIVE";
        public const string InvalidNumber = "LOCKER_INVALID_NUMBER";
        public const string NumberAlreadyExists = "LOCKER_NUMBER_ALREADY_EXISTS";
    }

    public static class Trainer
    {
        public const string TrainerNotFound = "TRAINER_NOT_FOUND";
        public const string TrainerInactive = "TRAINER_INACTIVE";
        public const string InvalidExperience = "TRAINER_INVALID_EXPERIENCE";
    }

    public static class Attendance
    {
        public const string CustomerAlreadyCheckedIn = "ATTENDANCE_CUSTOMER_ALREADY_CHECKED_IN";
        public const string NoActiveCheckInFound = "ATTENDANCE_NO_ACTIVE_CHECK_IN_FOUND";
        public const string AlreadyCheckedOut = "ATTENDANCE_ALREADY_CHECKED_OUT";
        public const string InvalidCheckOutTime = "ATTENDANCE_INVALID_CHECK_OUT_TIME";
        public const string AttendanceNotFound = "ATTENDANCE_NOT_FOUND";
        public const string LockerNotAvailable = "ATTENDANCE_LOCKER_NOT_AVAILABLE";
        public const string CustomerHasNoActiveSubscription = "ATTENDANCE_CUSTOMER_HAS_NO_ACTIVE_SUBSCRIPTION";
    }

    public static class Subscription
    {
        public const string SubscriptionNotFound = "SUBSCRIPTION_NOT_FOUND";
        public const string CustomerAlreadyHasActiveSubscription = "SUBSCRIPTION_CUSTOMER_ALREADY_HAS_ACTIVE_SUBSCRIPTION";
        public const string PackageInactive = "SUBSCRIPTION_PACKAGE_INACTIVE";
        public const string TrainerRequired = "SUBSCRIPTION_TRAINER_REQUIRED";
        public const string TrainerMismatch = "SUBSCRIPTION_TRAINER_MISMATCH";
        public const string PackageNotFound = "SUBSCRIPTION_PACKAGE_NOT_FOUND";
    }

    public static class Authentication
    {
        public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
        public const string InvalidAccessToken = "AUTH_INVALID_ACCESS_TOKEN";
        public const string InvalidRefreshToken = "AUTH_INVALID_REFRESH_TOKEN";
        public const string AlreadySignedOut = "AUTH_ALREADY_SIGNED_OUT";
        public const string InvalidPassword = "AUTH_PASSWORD_CHANGE_FAILED";
        public const string PasswordChangeFailed = "AUTH_PASSWORD_CHANGE_FAILED";
        public const string IncorrectCurrentPassword = "AUTH_INCORRECT_CURRENT_PASSWORD";
        public const string PasswordsDoNotMatch = "AUTH_PASSWORDS_DO_NOT_MATCH";
        public const string NewPasswordSameAsCurrent = "AUTH_NEW_PASSWORD_SAME_AS_CURRENT";
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

    public static class Kpi
    {
        public const string KpiNotFound = "KPI_NOT_FOUND";
        public const string KpiNameAlreadyExists = "KPI_NAME_ALREADY_EXISTS";
    }

    public static class TrainerKpi
    {
        public const string AlreadyEvaluatedToday = "TRAINER_KPI_ALREADY_EVALUATED_TODAY";
        public const string ScoreNotFound = "TRAINER_KPI_SCORE_NOT_FOUND";
        public const string InvalidScore = "TRAINER_KPI_INVALID_SCORE";
        public const string DuplicateKpiInSubmission = "TRAINER_KPI_DUPLICATE_KPI_IN_SUBMISSION";
    }
}
