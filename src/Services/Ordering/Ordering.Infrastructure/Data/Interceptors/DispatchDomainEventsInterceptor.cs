using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // What is ChangeTracker? ChangeTracker is a property of the DbContext class in Entity Framework Core
        // that keeps track of the state of entities being tracked by the context.
        // It monitors changes made to entities, such as additions, modifications, and deletions, and provides information about their current state (e.g., Added, Modified, Deleted, Unchanged). This allows EF Core to generate the appropriate SQL commands to persist those changes to the database when SaveChanges() is called.
        var aggregates = context.ChangeTracker
           .Entries<IAggregate>()
           .Where(a => a.Entity.DomainEvents.Any())
           .Select(a => a.Entity);

        var domainEvents = aggregates
           .SelectMany(a => a.DomainEvents)
           .ToList();

        // What does it mean this instruction? This instruction is using the MediatR library to publish domain events.
        // The MediatR library is a popular .NET library that implements the Mediator pattern,
        // which allows for decoupling the sender of a request from its handler.
        // In this case, it is used to publish domain events to their respective handlers.
        aggregates.ToList().ForEach(agg => agg.ClearDomainEvents());


        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }

}
