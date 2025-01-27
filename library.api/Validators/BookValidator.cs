using FluentValidation;
using library.api.Models;

namespace library.api.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Isbn)
                .Matches(@"^97[89]-\d{10}$")
                .WithMessage("value was not valid 13 digit isbn");
            RuleFor(book => book.Title).NotEmpty();
            RuleFor(book => book.Author).NotEmpty();
            RuleFor(book => book.ShortDescription).NotEmpty();
            RuleFor(book => book.PageCount).GreaterThan(0);
            RuleFor(book => book.ReleaseDate).NotEmpty();


        }


    }
}
