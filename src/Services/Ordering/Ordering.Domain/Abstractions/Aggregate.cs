namespace Ordering.Domain.Abstractions;

/// <summary>
/// Represents an aggregate root in the domain model. 
/// An aggregate is a cluster of domain objects that can be treated as a single unit. 
/// The aggregate root is the main entity that controls access to the other entities within the aggregate.
/// </summary>
/// <typeparam name="ValueObjectId"></typeparam>
/// what about the ValueObjectId? it is a generic type parameter that represents 
/// the type of the identifier for the aggregate root. 
/// why a ValueObjectId? because in DDD, the identifier of an aggregate root is often a value object,
/// yes, it is a value object, which means it is immutable and has value-based equality.
/// why is it immutable? because the identity of an aggregate root should not change over time,
/// what does it mean "should not change over time"? 
/// "what kind of time?" it means that once an aggregate root is created, its identity should remain the same throughout its lifecycle.

public abstract class Aggregate<ValueObjectId> : Entity<ValueObjectId>,  IAggregate<ValueObjectId>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }


    public IDomainEvent[] ClearDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }
}
