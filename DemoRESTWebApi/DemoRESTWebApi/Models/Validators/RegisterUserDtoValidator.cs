using DemoRESTWebApi.Entities;
using FluentValidation;

namespace DemoRESTWebApi.Models.Validators
{
    // FluentValidation validator.
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(LibraryDbContext dbContext) 
        {
            // Field Email can't be empty and have to be email format.
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(y => y.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        
        }
    }
}
