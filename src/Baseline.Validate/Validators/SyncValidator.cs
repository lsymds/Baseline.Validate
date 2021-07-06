namespace Baseline.Validate
{
    /// <summary>
    /// Base synchronous validation class which provides validation helper methods.
    /// </summary>
    /// <typeparam name="TToValidate">The type that is being validated.</typeparam>
    public abstract class SyncValidator<TToValidate> : BaseValidator<TToValidate>, ISyncValidator<TToValidate>
    {
        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> and returns a validation result.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        public abstract ValidationResult Validate(TToValidate toValidate);

        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> and throws an exception if the validation fails.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        public void ValidateAndThrow(TToValidate toValidate)
        {
            Validate(toValidate).ThrowIfValidationFailed();
        }
    }
}