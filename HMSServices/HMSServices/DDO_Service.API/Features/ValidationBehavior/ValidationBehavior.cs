
using ValidationException = FluentValidation.ValidationException;

namespace DDO_Service.API.Features.ValidationBehavior
{



    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators != null)
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(result => !result.IsValid)
                    .SelectMany(result => result.Errors)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);  // This prevents the handler from being called if validation fails
                }
            }

            // If validation succeeds, call the next delegate/middleware in the pipeline
            return await next();
        }
    }
}