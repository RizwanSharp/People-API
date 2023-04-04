using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using People.Domain.Entities;

namespace People.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    public async Task TrySeedAsync()
    {
        if (!_context.People.Any())
        {
            _context.People.AddRange(new[]
            {
                new Person
                {
                    Id = 1,
                    Name = "Rizwan",
                    AddressLine1 = "515 Bosque Vis",
                    AddressLine2 = null,
                    City = "San Antonio",
                    State = "TX",
                    PostalCode = "78258",
                    Country = "USA"
                },
                new Person
                {
                    Id = 2,
                    Name = "Jane",
                    AddressLine1 = "456 Maple Ave",
                    AddressLine2 = null,
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA"
                },
                new Person
                {
                    Id = 3,
                    Name = "John",
                    AddressLine1 = "789 Oak St",
                    AddressLine2 = null,
                    City = "Chicago",
                    State = "IL",
                    PostalCode = "60601",
                    Country = "USA"
                },
                new Person
                {
                    Id = 4,
                    Name = "Emily",
                    AddressLine1 = "1010 Elm St",
                    AddressLine2 = null,
                    City = "San Francisco",
                    State = "CA",
                    PostalCode = "94101",
                    Country = "USA"
                },
                new Person
                {
                    Id = 5,
                    Name = "Michael",
                    AddressLine1 = "555 Elm St",
                    AddressLine2 = null,
                    City = "Boston",
                    State = "MA",
                    PostalCode = "02101",
                    Country = "USA"
                },
                new Person
                {
                    Id = 6,
                    Name = "Sarah",
                    AddressLine1 = "1234 Oak Ave",
                    AddressLine2 = null,
                    City = "Dallas",
                    State = "TX",
                    PostalCode = "75201",
                    Country = "USA"
                },
                new Person
                {
                    Id = 7,
                    Name = "Tom",
                    AddressLine1 = "567 Maple St",
                    AddressLine2 = null,
                    City = "Seattle",
                    State = "WA",
                    PostalCode = "98101",
                    Country = "USA"
                },
                new Person
                {
                    Id = 8,
                    Name = "Amy",
                    AddressLine1 = "789 Oak Ave",
                    AddressLine2 = null,
                    City = "Atlanta",
                    State = "GA",
                    PostalCode = "30301",
                    Country = "USA"
                },
                new Person
                {
                    Id = 9,
                    Name = "David",
                    AddressLine1 = "2345 Maple St",
                    AddressLine2 = null,
                    City = "Miami",
                    State = "FL",
                    PostalCode = "33101",
                    Country = "USA"
                },
                new Person
                {
                    Id = 10,
                    Name = "Emma",
                    AddressLine1 = "6789 Oak Ave",
                    AddressLine2 = null,
                    City = "Denver",
                    State = "CO",
                    PostalCode = "80201",
                    Country = "USA"
                }
        });

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}