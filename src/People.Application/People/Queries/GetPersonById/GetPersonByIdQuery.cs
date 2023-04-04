using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using People.Application.Common.Exceptions;
using People.Application.Common.Interfaces;
using People.Application.People.Queries.GetPeopleWithPagination;
using People.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.Queries.GetPersonById;

public record GetPersonByIdQuery(long PersonId) : IRequest<PersonReadDto>;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonReadDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public GetPersonByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cache = cache ?? throw new ArgumentNullException(nameof(_cache));
    }

    public async Task<PersonReadDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Person-{request.PersonId}";
        if (!_cache.TryGetValue<Person>(cacheKey, out Person person))
        {
            person = await _context.People.FindAsync(request.PersonId, cancellationToken).ConfigureAwait(false);

            if(person == null)
                throw new NotFoundException(nameof(Person), request.PersonId);
            
            _cache.Set(cacheKey, person);
        }

        var result = _mapper.Map<PersonReadDto>(person);

        return result;
    }
}