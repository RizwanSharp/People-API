using AutoMapper;
using MediatR;
using People.Application.Common.Exceptions;
using People.Application.Common.Interfaces;
using People.Application.People.EventHandlers;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.Commands.UpdatePerson;

public record class UpdatePersonCommand(long PersonId, PersonUpdateDto Person) : IRequest;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdatePersonCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        Person person = await _context.People.FindAsync(request.PersonId, cancellationToken);

        if (person == null)
            throw new NotFoundException(nameof(Person), request.PersonId);
        
        PersonUpdateDto personDto = request.Person;

        person.Name = personDto.Name;
        person.AddressLine1 = personDto.Address.Line1;
        person.AddressLine2 = personDto.Address.Line2;
        person.City = personDto.Address.City;
        person.State = personDto.Address.State;
        person.PostalCode = personDto.Address.PostalCode;
        person.Country = personDto.Address.Country;
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        PersonCreatedOrUpdatedEvent personCreatedOrUpdatedEvent  = new PersonCreatedOrUpdatedEvent();
        await _mediator.Publish(personCreatedOrUpdatedEvent, cancellationToken);
    }
}