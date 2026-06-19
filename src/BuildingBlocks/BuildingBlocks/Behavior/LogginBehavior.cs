using BuildingBlocks.CQRS;
using FluentValidation;
using FluentValidation.Internal;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behavior;

public class LogginBehavior<TRequest, TResponse>
    (ILogger<LogginBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"[START] Handle request={typeof(TRequest)} - Response={typeof(TResponse)}");
        
        var timer = new Stopwatch();
        timer.Start();

        var response = await next();
        
        timer.Stop();

        var timeTaken = timer.Elapsed;

        if(timeTaken > TimeSpan.FromSeconds(3))
        {
            logger.LogWarning($"Handling request={typeof(TRequest)} took {timeTaken.TotalSeconds} seconds, which exceeds the threshold.");
        }

        logger.LogInformation($"[END] Handle request={typeof(TRequest)} - Response={typeof(TResponse)}");
        
        return response;
    }
}