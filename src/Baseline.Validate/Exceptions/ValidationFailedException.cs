using System;

namespace Baseline.Validate
{
    /// <summary>
    /// Exception that is thrown when validation fails and a consumer asks for an exception to be thrown.
    /// </summary>
    public class ValidationFailedException : Exception
    {
        /// <summary>
        /// Gets the validation result that failed.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationFailedException"/> class, setting a helpful message
        /// that informs readers of the exception what validation has failed for and why.
        /// </summary>
        /// <param name="validationResult">The validation result that was a failure.</param>
        public ValidationFailedException(ValidationResult validationResult) : base(
            $"Validation failed for object {validationResult.ValidationTarget} with {validationResult.Failures.Count} " +
            $"{(validationResult.Failures.Count > 1 ? "failures" : "failure")}."
        )
        {
            ValidationResult = validationResult;
        }
    }
}