using DotnetBoilerplate.Infrastructure.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetBoilerplate.Infrastructure.Mediator.Behavior
{
    public class ValidateCommandPipelineBehavior<TRequest, Unit> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IRequest<Unit>
    {
        private readonly INotificationContext _notificationContext;
        private readonly IEnumerable<IValidator> _validators;

        public ValidateCommandPipelineBehavior(INotificationContext notificationContext, IEnumerable<IValidator<TRequest>> validators)
        {
            _notificationContext = notificationContext;
            _validators = validators;
        }

        public Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            return failures.Any()
                ? Notify(failures)
                : next();
        }

        private Task<Unit> Notify(IEnumerable<ValidationFailure> failures)
        {
            foreach (var failure in failures)
            {
                _notificationContext.NotifyError(failure.ErrorMessage);
            }
            return Task.FromResult(default(Unit));
        }
    }
}
