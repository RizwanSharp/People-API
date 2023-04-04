using People.Application.Common.Models;

namespace People.Application.People.Commands.UpdatePerson;

public class PersonUpdateDto
{ 
    public string Name { get; set; }

    public AddressDto Address { get; set; }
}