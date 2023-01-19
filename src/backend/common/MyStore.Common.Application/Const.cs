namespace MyStore.Common.Application
{
    public sealed class Const
    {
        public sealed class BaseExceptionText
        {
            public const string Forbidden = "You do not have access to this section";
        }

        public sealed class RequestAuthorizationPipelineText
        {
            public const string UndefinedAccess = "The command in question is not defined correctly";
        }
    }
}
