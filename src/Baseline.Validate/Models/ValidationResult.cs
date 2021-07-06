using System.Collections.Generic;
using System.Linq;

namespace Baseline.Validate
{
    /// <summary>
    /// Represents a validation result returned from validators.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets the target of validation.
        /// </summary>
        public string ValidationTarget { get; }
        
        /// <summary>
        /// Gets whether the validation result is a success or not.
        /// </summary>
        public bool Success => !Failures.Any();

        /// <summary>
        /// Gets the validation failures. Empty if there aren't any.
        /// </summary>
        public Dictionary<string, string> Failures { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationResult"/> object, setting the target or object name
        /// that is being validated.
        /// </summary>
        /// <param name="target">The name of the object that is being validated.</param>
        public ValidationResult(string target)
        {
            ValidationTarget = target;
        }

        /// <summary>
        /// Throws a <see cref="ValidationFailedException"/> if there are any failures or else returns.
        /// </summary>
        public void ThrowIfValidationFailed()
        {
            if (Success)
            {
                return;
            }

            throw new ValidationFailedException(this);
        }
    }
}