using AutoMapper;
using MediatR;
using People.Application.Common.Interfaces;
using People.Application.People.EventHandlers;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.Commands.CreatePerson;

public record class CreatePersonCommand : IRequest<PersonCreatedDto>
{
    public PersonCreateDto Person { get; init; }
}

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonCreatedDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreatePersonCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<PersonCreatedDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        Person person = new ();
        PersonCreateDto personDto = request.Person;

        person.Name = personDto.Name;
        person.AddressLine1 = personDto.Address.Line1;
        person.AddressLine2 = personDto.Address.Line2;
        person.City = personDto.Address.City;
        person.State = personDto.Address.State;
        person.PostalCode = personDto.Address.PostalCode;
        person.Country = personDto.Address.Country;

        _context.People.Add(person);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        PersonCreatedOrUpdatedEvent personCreatedOrUpdatedEvent = new PersonCreatedOrUpdatedEvent();
        await _mediator.Publish(personCreatedOrUpdatedEvent, cancellationToken);

        PersonCreatedDto result = _mapper.Map<PersonCreatedDto>(person);

        return result;
    }
}