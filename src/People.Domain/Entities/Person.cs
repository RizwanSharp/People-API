namespace People.Domain.Entities;

public class Person
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }
}