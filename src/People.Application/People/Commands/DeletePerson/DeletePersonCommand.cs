using MediatR;
using People.Application.Common.Interfaces;
using People.Application.People.EventHandlers;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.Commands.DeletePerson;

public record class DeletePersonCommand(long PersonId) : IRequest;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public DeletePersonCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        Person person = await _context.People.FindAsync(request.PersonId, cancellationToken);

        if (person == null) return;

        _context.People.Remove(person);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        PersonDeletedEvent personDeletedEvent = new PersonDeletedEvent();
        await _mediator.Publish(personDeletedEvent, cancellationToken);
    }
}