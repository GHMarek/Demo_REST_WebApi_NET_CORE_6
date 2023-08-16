using DemoRESTWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DemoRESTWebApi.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Library>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Library resource)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read 
                || requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;


        }
    }
}
