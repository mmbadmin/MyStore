namespace MyStore.Application.Commons
{
    public sealed class Texts
    {
        public sealed class Setting
        {
            public const string UserMaxFailAttemptCount = "User_MaxFailAttemptCount";
            public const string UserLockDuration = "User_LockDuration";
            public const string UserTryDuration = "User_TryDuration";
            public const string UserSingleSession = "User_SingleSession";
            public const string UserAutomaticUnlock = "User_AutomaticUnlock";
            public const string UserSessionDuration = "User_SessionDuration";
        }

        public sealed class Users
        {
            public const string UserNameValidation = "Username is invalid";
            public const string IsUserNameUnique = "Username can not be duplicated";
            public const string InvalidUserNameOrPassword = "Your username or password is invalid";
            public const string TooManyFailedLoginAttempt = "Your username has been locked due to excessive password repetition";
            public const string OldPasswordIsRequired = "Enter the current password";
            public const string NewPasswordIsRequired = "Enter the new password";
            public const string PasswordCannotBeLessThanEightLetter = "The password should not be less than eight characters";
            public const string NewPasswordAndConfirmPasswordInconsistent = "Repeat password does not match new password";
            public const string UserIsRequired = "Enter the user";
            public const string InvalidPassword = "Your password is invalid";
            public const string InvalidCaptcha = "Your password is invalid";
            public const string InvalidLengthCaptcha = "Enter the complete security code";
        }

        public sealed class Command
        {
            public const string IsTitleUnique = "The Title is duplicate";
            public const string ErrorOccurred = "An Error has occurred";
            public const string DeleteImpossible = "It is not possible to delete this option";
            public const string UpdateImpossible = "It is not possible to Update this option";

        }

        public sealed class SLADBForm
        {
            public const string IsEntryDateUnique = "The EntryDate  is duplicate";
        }
    }
}
