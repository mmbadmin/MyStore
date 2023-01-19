namespace MyStore.Common.Application.Exceptions
{
    using System;

    public class BaseException : Exception
    {
        public BaseException(int status)
            : this(status, string.Empty)
        {
        }

        public BaseException(int status, string message)
            : this(status, message, null)
        {
        }

        public BaseException(int status, string message, Exception? innerException)
            : base(message, innerException)
        {
            Status = status;
        }

        public int Status { get; set; }

        public static BaseException BadRequest() => new BaseException(400);

        public static BaseException BadRequest(string message) => new BaseException(400, message);

        public static BaseException NotFound() => new BaseException(404, "No information found");

        public static BaseException NotFound(string message) => new BaseException(404, message);

        public static BaseException NotFound(string name, object key) => new BaseException(404, $"Entity \"{name}\" ({key}) was not found.");

        public static BaseException UnAuthorize() => new BaseException(401);

        public static BaseException Forbidden() => new BaseException(403, Const.BaseExceptionText.Forbidden);

        public static BaseException ErrorMessage(string message) => new BaseException(404, message);
    }
}
