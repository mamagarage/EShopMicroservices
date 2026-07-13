namespace Ordering.Domain.Models;

internal class Customer : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
}