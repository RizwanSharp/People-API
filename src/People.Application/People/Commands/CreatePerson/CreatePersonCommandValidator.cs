using FluentValidation;
using Microsoft.EntityFrameworkCore;
using People.Application.Common.Interfaces;
using People.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace People.Application.People.Commands.CreatePerson
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePersonCommandValidator(IApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(x => x.Person)
                .NotNull()
                .MustAsync((x, cancellation) => IsPersonNameUniqueAsync(x)).WithMessage("User with this name already exists")
                .SetValidator(new PersonValidator());
        }

        private async Task<bool> IsPersonNameUniqueAsync(PersonCreateDto personDto)
        {
            bool alreadyTaken = await _context.People.AnyAsync(p => p.Name == personDto.Name).ConfigureAwait(false);
            return !alreadyTaken;
        }

        class PersonValidator : AbstractValidator<PersonCreateDto>
        {
            public PersonValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Address).SetValidator(new AddressValidator());
            }
        }

        class AddressValidator : AbstractValidator<AddressDto>
        {
            public AddressValidator()
            {
                RuleFor(x => x.Line1).NotEmpty().MinimumLength(1).MaximumLength(50);
                RuleFor(x => x.Line2).MaximumLength(50);
                RuleFor(x => x.City).NotEmpty().MinimumLength(1).MaximumLength(20);
                RuleFor(x => x.State).NotEmpty().MinimumLength(1).MaximumLength(20);
                RuleFor(x => x.PostalCode).NotEmpty().MinimumLength(1).MaximumLength(20);
                RuleFor(x => x.Country).NotEmpty().MinimumLength(1).MaximumLength(20);
            }
        }
    }
}