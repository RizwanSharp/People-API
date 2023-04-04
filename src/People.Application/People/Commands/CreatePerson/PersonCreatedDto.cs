using AutoMapper;
using People.Application.Common.Mappings;
using People.Application.Common.Models;
using People.Domain.Entities;

namespace People.Application.People.Commands.CreatePerson;

public class PersonCreatedDto : IMapFrom<Person>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public AddressDto Address { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Person, PersonCreatedDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressDto
            {
                Line1 = src.AddressLine1,
                Line2 = src.AddressLine2,
                City = src.City,
                State = src.State,
                PostalCode = src.PostalCode,
                Country = src.Country
            }));
    }
}