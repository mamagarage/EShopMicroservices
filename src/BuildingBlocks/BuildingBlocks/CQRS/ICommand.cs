using MediatR;

namespace BuildingBlocks.CQRS
{
    // This interface represents a command in the CQRS pattern
    // that does not expect a response (or expects a response of type Unit).
    // It inherits from ICommand<Unit>, which means it is a command that does not return any meaningful data.
    public interface ICommand : ICommand<Unit>
    {

    }

    // This interface represents a command in the CQRS pattern,
    // which is a request that expects a response of type TResponse.
    public interface ICommand<TResponse> : IRequest<TResponse>
    {

    }
}
