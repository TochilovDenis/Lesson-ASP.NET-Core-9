namespace Creating_API_Use
{
    public static class UsersExtensions
    {
        public static IApplicationBuilder UseUsersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UsersMiddleware>();
        }
    }
}
