using MediatR;
using Microsoft.EntityFrameworkCore;
using People.Application.Common.Interfaces;
using People.Domain.Entities;
using System.Reflection;

namespace People.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}