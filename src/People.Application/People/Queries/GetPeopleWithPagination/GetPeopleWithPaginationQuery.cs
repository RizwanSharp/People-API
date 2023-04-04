using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using People.Application.Common.Interfaces;
using People.Application.Common.Mappings;
using People.Application.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.People.Queries.GetPeopleWithPagination;

public record GetPeopleWithPaginationQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<PersonReadDto>>;

public class GetPeopleWithPaginationQueryHandler : IRequestHandler<GetPeopleWithPaginationQuery, PaginatedList<PersonReadDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPeopleWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PersonReadDto>> Handle(GetPeopleWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.People
            .ProjectTo<PersonReadDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize).ConfigureAwait(false);
    }
}