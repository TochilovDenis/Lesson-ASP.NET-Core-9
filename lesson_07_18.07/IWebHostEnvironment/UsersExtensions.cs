namespace IWebHostEnvironment
{
    public static class UsersExtensions
    {
        public static IApplicationBuilder UseUsersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UsersMiddleware>();
        }

        public static IApplicationBuilder UseMainMiddleware(this IApplicationBuilder builder)
        {
            // builder.Map("/api", appBuilder => builder.UseMiddleware<MainsMiddleware>());
            return builder.UseMiddleware<MainMiddleware>();
        }

        public static IApplicationBuilder UseUploadsMiddleware(this IApplicationBuilder builder)
        {
            // builder.Map("/api", appBuilder => builder.UseMiddleware<MainsMiddleware>());
            return builder.UseMiddleware<UploadsMiddleware>();
        }

    }
}
