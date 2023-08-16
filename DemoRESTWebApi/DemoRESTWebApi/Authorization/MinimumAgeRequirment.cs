using Microsoft.AspNetCore.Authorization;

namespace DemoRESTWebApi.Authorization
{
    public class MinimumAgeRequirment : IAuthorizationRequirement
    {
        public int MinimumAge { get; set; }

        public MinimumAgeRequirment(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
