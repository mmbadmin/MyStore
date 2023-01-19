namespace MyStore.Common.API.Mvc
{
    using Microsoft.AspNetCore.Builder;

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder MyStoreUseExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
