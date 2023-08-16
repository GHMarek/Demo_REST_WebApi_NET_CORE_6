using DemoRESTWebApi.Models;
using DemoRESTWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoRESTWebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) { _accountService = accountService; }

        // http://localhost:5001/api/account/register
        /* {"Email" : "test@test.pl",
        * "Password" : "password123",
        * "Nationality" : "Italian",
        * "dateOfBirth" : "1985-05-06",
        * "ConfirmPassword" : "password123"}
        */
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            _accountService.RegisterUser(registerUserDto);

            return Ok();
        }

        /*
         * http://localhost:5001/api/account/login
         * returns token to use in headers: "key" : "Bearer authtokenPaste"
         */
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            string token = _accountService.GenerateJwt(loginDto);

            return Ok(token);
        }
    }
}
