using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Filters
{
    public class AuthLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuthLoggingFilter> _logger;

        public AuthLoggingFilter(ILogger<AuthLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string? email = null;

            foreach (var arg in context.ActionArguments.Values)
            {
                switch (arg)
                {
                    case LoginDto login:
                        email = login.Email;
                        break;
                    case UserRegisterDto reg:
                        email = reg.Email;
                        break;
                }

                if (email is not null) break;
            }

            var executed = await next();
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? "unknown";

            if (string.Equals(actionName, "Login", System.StringComparison.OrdinalIgnoreCase))
            {
                if (executed.Result is UnauthorizedObjectResult)
                {
                    _logger.LogWarning("Failed login attempt for {Email} from {IP}", email ?? "unknown", ip);
                }
                else if (executed.Result is OkObjectResult)
                {
                    _logger.LogInformation("Successful login for {Email} from {IP}", email ?? "unknown", ip);
                }
            }

            if (string.Equals(actionName, "Register", System.StringComparison.OrdinalIgnoreCase))
            {
                if (executed.Result is ConflictObjectResult)
                {
                    _logger.LogWarning("Registration attempt with existing email {Email} from {IP}", email ?? "unknown", ip);
                }
                else if (executed.Result is CreatedAtActionResult)
                {
                    _logger.LogInformation("Created user {Email} from {IP}", email ?? "unknown", ip);
                }
                else if (executed.Result is BadRequestObjectResult)
                {
                    _logger.LogError("Failed to create user {Email} from {IP}", email ?? "unknown", ip);
                }
            }
        }
    }
}