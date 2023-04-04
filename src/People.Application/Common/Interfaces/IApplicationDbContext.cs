using Microsoft.EntityFrameworkCore;
using People.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace People.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Person> People { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}