using DemoRESTWebApi.Entities;
using FluentValidation;
using System.Runtime.CompilerServices;

namespace DemoRESTWebApi.Models.Validators
{
    public class LibraryQueryValidator : AbstractValidator<LibraryQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = { nameof(Library.Name)
        , nameof(Library.Description)
        , nameof(Library.Category)
        };

        public LibraryQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", allowedPageSizes)}]");
                }
            });

            RuleFor(x => x.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"SortBy is optional or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
