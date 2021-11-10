using System;
using System.Text;

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
        public ValidationFailedException(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        /// <inheritdoc />
        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(
                    $"Validation failed for object {ValidationResult.ValidationTarget} with " +
                    $"{ValidationResult.Failures.Count} failed " +
                    $"{(ValidationResult.Failures.Count == 1 ? "property" : "properties")}."
                );

                foreach (var failure in ValidationResult.Failures)
                {
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.AppendLine($"{failure.Key}:");

                    foreach (var failureMessage in failure.Value)
                    {
                        stringBuilder.AppendLine($"\t{failureMessage}");
                    }
                }

                return stringBuilder.ToString();
            }
        }
    }
}