using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Validate
{
    /// <summary>
    /// Base asynchronous validation class which provides validation helper methods.
    /// </summary>
    /// <typeparam name="TToValidate">The type that is being validated.</typeparam>
    public abstract class AsyncValidator<TToValidate> : BaseValidator<TToValidate>, IAsyncValidator<TToValidate>
    {
        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> asynchronously and returns a validation result.
        /// As this returns a <see cref="ValueTask{TResult}"/>, returning a synchronous result at any point will not
        /// incur any significant performance penalties.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public abstract ValueTask<ValidationResult> ValidateAsync(
            TToValidate toValidate,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> asynchronously and throws an exception if the
        /// validation fails.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async ValueTask ValidateAndThrowAsync(
            TToValidate toValidate,
            CancellationToken cancellationToken = default
        )
        {
            var validationResult = await ValidateAsync(toValidate, cancellationToken);
            validationResult.ThrowIfValidationFailed();
        }
    }
}