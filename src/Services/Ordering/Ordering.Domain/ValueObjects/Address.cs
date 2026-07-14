namespace Ordering.Domain.ValueObjects;

// why record? because it is immutable and we can use it as a value object 
// what kind of object is this for .Net definition? This is a value object. A value object is an object that represents a descriptive aspect of the domain with no conceptual identity. Value objects are immutable, meaning that their state cannot be modified after they are created.
// They are defined by their properties and are often used to represent concepts such as money, dates, or addresses.
// What is internally used for? Internally, this record is used to represent an address in the ordering domain. It encapsulates the properties of an address, such as first name, last name, email address, address line, country, state, and zip code. The record is immutable, meaning that once an instance is created, its properties cannot be changed. This ensures that the address remains consistent and reliable throughout its lifecycle in the application.
// how can be rapresented in the database? In a database,
// this record can be represented as a table with columns corresponding to each property of the Address record. For example, the table could have columns for FirstName, LastName, EmailAddress, AddressLine, Country, State, and ZipCode.
// Each row in the table would represent a unique address instance. The immutability of the record can be enforced at the application level by ensuring that once an address is created and stored in the database, it cannot be modified directly; instead, a new address instance would need to be created if changes are required.

public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;
    protected Address()
    {
    }

    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        //ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        //ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        //ArgumentException.ThrowIfNullOrWhiteSpace(country);
        //ArgumentException.ThrowIfNullOrWhiteSpace(state);
        //ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);

        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }
}