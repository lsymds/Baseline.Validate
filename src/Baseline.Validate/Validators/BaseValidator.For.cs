using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Baseline.Validate
{
    public abstract partial class BaseValidator<TToValidate>
    {
        /// <summary>
        /// Gets and returns an immediate child validation result with the keys combined with the key defined by the
        /// expression.
        /// </summary>
        /// <param name="expression">The expression which returns the child field that has failed validation.</param>
        /// <param name="childValidationResult">The result from a nested validation result.</param>
        protected ValidationResult FailureFor<TField>(
            Expression<Func<TToValidate, TField>> expression, 
            ValidationResult childValidationResult
        )
        {
            return FailureFor(GetNameOfField(expression), childValidationResult);
        }

        /// <summary>
        /// Gets and returns an immediate child validation result with the keys combined with the key specified.
        /// </summary>
        /// <param name="property">The property which has failed validation.</param>
        /// <param name="childValidationResult">The result from a nested validation result.</param>
        protected ValidationResult FailureFor(
            string property, 
            ValidationResult childValidationResult
        )
        {
            if (childValidationResult.Success)
            {
                return Success();
            }

            var validationResult = new ValidationResult(ValidatingTypeName);

            foreach (var (key, value) in childValidationResult.Failures)
            {
                validationResult.Failures.Add(
                    $"{(string.IsNullOrWhiteSpace(property) ? string.Empty : $"{property}.")}{key}", 
                    value
                );
            }

            return validationResult;
        }

        /// <summary>
        /// Immediately returns a failure for a request property defined by the expression.
        /// </summary>
        /// <param name="expression">The expression which returns the field that has failed validation.</param>
        /// <param name="message">The message to return.</param>
        protected ValidationResult FailureFor<TField>(Expression<Func<TToValidate, TField>> expression, string message)
        {
            return FailureFor(GetNameOfField(expression), message);
        }

        /// <summary>
        /// Immediately returns a failure for a request property.
        /// </summary>
        /// <param name="property">The property which has failed validation.</param>
        /// <param name="message">The message to return.</param>
        protected ValidationResult FailureFor(string property, string message)
        {
            var validationResult = new ValidationResult(ValidatingTypeName);
            validationResult.Failures.Add(property, new List<string> { message.Replace(":property", property) });
            return validationResult;
        }
    }
}