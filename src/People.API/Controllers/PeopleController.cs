using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.People.Commands.CreatePerson;
using People.Application.People.Commands.DeletePerson;
using People.Application.People.Commands.UpdatePerson;
using People.Application.People.Queries.GetPeopleWithPagination;
using People.Application.People.Queries.GetPersonById;

namespace People.API.Controllers;

[Route(ApiRoutes.People.RoutePrefix)]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    // GET: people
    [HttpGet(ApiRoutes.People.GetList)]
    public async Task<ActionResult> GetPeopleAsync(int pageNumber = 1, int pageSize = 5)
    {
        var query = new GetPeopleWithPaginationQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // GET people/{id}
    [HttpGet(ApiRoutes.People.GetOne)]
    public async Task<ActionResult> GetPersonAsync(long id)
    {
        var query = new GetPersonByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // POST people
    [HttpPost(ApiRoutes.People.Create)]
    public async Task<IActionResult> CreatePersonAsync([FromBody] PersonCreateDto person)
    {
        var command = new CreatePersonCommand() { Person = person };
        var createdResource = await _mediator.Send(command);
        var routeValues = new { id = createdResource.Id};

        return CreatedAtAction(nameof(GetPersonAsync), routeValues, createdResource);
    }

    // PUT people/{id}
    [HttpPut(ApiRoutes.People.Update)]
    public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpdateDto person)
    {
        var command = new UpdatePersonCommand(id, person);
        await _mediator.Send(command);

        return NoContent();
    }

    // DELETE people/5
    [HttpDelete(ApiRoutes.People.Delete)]
    public async Task<IActionResult> DeletePersonAsync(long id)
    {
        var command = new DeletePersonCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }
}