namespace SnowFox_Net.Common.Middleware
{
    using Microsoft.AspNetCore.Http;
    using SnowFox_Net.Shared.DTOs;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UserContextMiddleware" />
    /// </summary>
    public class UserContextMiddleware
    {
        /// <summary>
        /// Defines the _next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/></param>
        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// The Invoke
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <param name="userContext">The userContext<see cref="UserContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Invoke(HttpContext context, UserContext userContext)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var userNameClaim = context.User.FindFirst(ClaimTypes.Name);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int Id))
            {
                userContext.Id = (uint)Id;
            }

            if (userNameClaim != null)
            {
                userContext.UserName = userNameClaim.Value;
            }

            await _next(context);
        }
    }
}
