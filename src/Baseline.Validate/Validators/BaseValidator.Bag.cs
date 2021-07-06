using System;
using System.Linq.Expressions;

namespace Baseline.Validate
{
    public abstract partial class BaseValidator<TToValidate>
    {
        /// <summary>
        /// Bags a failure for a child validation result and merges the keys in it with the property name retrieved from
        /// the expression.
        /// </summary>
        /// <param name="expression">The expression used to retrieve the property name that is being validated.</param>
        /// <param name="childValidationResult">The validation result to bag and merge.</param>
        protected void BagFailureFor<TField>(
            Expression<Func<TToValidate, TField>> expression,
            ValidationResult childValidationResult
        )
        {
            BagFailureFor(GetNameOfField(expression), childValidationResult);
        }
        
        /// <summary>
        /// Bags a failure for a child validation result and merges the keys in it with the property name.
        /// </summary>
        /// <param name="property">The name of the parent property that is being validated.</param>
        /// <param name="childValidationResult">The validation result to bag and merge.</param>
        protected void BagFailureFor(
            string property,
            ValidationResult childValidationResult
        )
        {
            if (childValidationResult.Success)
            {
                return;
            }

            foreach (var (key, value) in childValidationResult.Failures)
            {
                _baggedValidationResult.Failures.Add(
                    $"{(string.IsNullOrWhiteSpace(property) ? string.Empty : $"{property}.")}{key}", 
                    value
                );
            }
        }
        
        /// <summary>
        /// Bags a failure for a request property defined by the expression.
        /// </summary>
        /// <param name="expression">The expression which returns the field that has failed validation.</param>
        /// <param name="message">The message to return.</param>
        protected void BagFailureFor<TField>(Expression<Func<TToValidate, TField>> expression, string message)
        {
            BagFailureFor(GetNameOfField(expression), message);
        }

        /// <summary>
        /// Bags a failure for a request property.
        /// </summary>
        /// <param name="property">The property which has failed validation.</param>
        /// <param name="message">The message to return.</param>
        protected void BagFailureFor(string property, string message)
        {
            _baggedValidationResult.Failures.Add(property, message.Replace(":property", property));
        }

    }
}