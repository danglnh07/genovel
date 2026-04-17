using FluentValidation;
using Genovel.Shared.CQRS;
using MediatR;

namespace Genovel.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Get validation context
        var context = new ValidationContext<TRequest>(request);

        // Validate against the request object using all the registered validators
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Check for any failures
        var failures = results
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        // Throw validation exception if found any validation failure
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // If no failure, proceed to the next pipeline
        return await next();
    }
}
