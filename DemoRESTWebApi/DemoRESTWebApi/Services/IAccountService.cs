using DemoRESTWebApi.Models;

namespace DemoRESTWebApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        public string GenerateJwt(LoginDto loginDto);
    }
}