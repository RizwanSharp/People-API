using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using People.API.Filters;
using People.Application.Common.Interfaces;
using People.Application.IntegrationTests.Mocks;
using People.Domain.Entities;
using System.Data.Entity.Infrastructure;

namespace People.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddApplicationServices();
            services.AddInfrastructureServices(_configuration);
            services.AddMemoryCache();
            services.AddLogging();
            services.AddControllers(options =>
            {
                options.Filters.Add(new ApiExceptionFilterAttribute());
                options.SuppressAsyncSuffixInActionNames = false;
            });

            ReplaceDbContext(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        private void ReplaceDbContext(IServiceCollection services)
        {
            var serviceDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(IApplicationDbContext));
            services.Remove(serviceDescriptor);
            //services.AddScoped<IApplicationDbContext, FakeDbContext>();
            var data = GetData().AsQueryable();
            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IDbAsyncEnumerable<Person>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Person>(data.GetEnumerator()));
            mockSet.Setup(b => b.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(ids => ValueTask.FromResult(mockSet.Object.FirstOrDefault(x => x.Id == (long)ids[0])));

            //.Returns(ids => mockSet.Object.FirstOrDefaultAsync(b => b.Id == 1)).;

            mockSet.As<IQueryable<Person>>()
            .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Person>(data.Provider));

            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.People).Returns(mockSet.Object);
            services.AddScoped<IApplicationDbContext>(x => mockContext.Object);
            var con = services.BuildServiceProvider().CreateScope().ServiceProvider.GetService<IApplicationDbContext>();
        }

        private IEnumerable<Person> GetData()
        {
           return new[]
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
        };
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }
    }
}