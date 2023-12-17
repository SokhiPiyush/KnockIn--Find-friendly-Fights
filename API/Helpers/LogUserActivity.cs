
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter//implementing Last active funtionality for a user
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return; // return if the user is not authenticated

            var userId = resultContext.HttpContext.User.GetUserId();

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}