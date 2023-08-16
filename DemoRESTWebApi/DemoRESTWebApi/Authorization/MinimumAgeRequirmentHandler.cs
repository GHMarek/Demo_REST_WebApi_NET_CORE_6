using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DemoRESTWebApi.Authorization
{
    public class MinimumAgeRequirmentHandler : AuthorizationHandler<MinimumAgeRequirment>
    {
        private readonly ILogger<MinimumAgeRequirmentHandler> _logger;

        public MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirment requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            
            if(dateOfBirth.AddDays(requirement.MinimumAge) <= DateTime.Today)
            {
                context.Succeed(requirement);
                _logger.LogInformation($"User {userEmail} authorization success.");
            }
            else
            {
                _logger.LogInformation($"User {userEmail} authorization fail.");
            }

            return Task.CompletedTask;
        }
    }
}
