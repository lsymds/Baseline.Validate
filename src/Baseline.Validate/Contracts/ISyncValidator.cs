namespace Baseline.Validate
{
    /// <summary>
    /// Contract that defines what synchronous validators must implement.
    /// </summary>
    /// <typeparam name="TToValidate">The type that is being validated.</typeparam>
    public interface ISyncValidator<in TToValidate>
    {
        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> and returns a validation result.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        ValidationResult Validate(TToValidate toValidate);

        /// <summary>
        /// Validates the object of type <see cref="TToValidate" /> and throws an exception if the validation fails.
        /// </summary>
        /// <param name="toValidate">The object to validate.</param>
        void ValidateAndThrow(TToValidate toValidate);
    }
}