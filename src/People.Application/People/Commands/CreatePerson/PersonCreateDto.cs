using People.Application.Common.Mappings;
using People.Application.Common.Models;
using People.Domain.Entities;

namespace People.Application.People.Commands.CreatePerson;

public class PersonCreateDto : IMapFrom<Person>
{
    public string Name { get; set; }

    public AddressDto Address { get; set; }
}