using FluentAssertions;
using NUnit.Framework;
using People.Application.Common.Exceptions;
using People.Application.People.Queries.GetPersonById;

namespace People.Application.IntegrationTests.People.Queries;

using static Testing;

public class GetPersonByIdTests
{
    [Test]
    public async Task ShouldReturnAnExistingPerson()
    { 
        var query = new GetPersonByIdQuery(1);

        var result = await SendAsync(query);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Test]
    public async Task ShouldReturnANonExistingPerson()
    {
        var query = new GetPersonByIdQuery(11);

        var actTask = async () => { await SendAsync(query); };

        await actTask.Should().ThrowAsync<NotFoundException>();
    }
}