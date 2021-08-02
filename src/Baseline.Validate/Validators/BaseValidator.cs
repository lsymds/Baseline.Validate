using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Baseline.Validate.Internal.Extensions;

namespace Baseline.Validate
{
    /// <summary>
    /// Base validator used by other validators in the library. Contains common functionality between them all.
    /// </summary>
    public abstract partial class BaseValidator<TToValidate>
    {
        private readonly ValidationResult _baggedValidationResult;

        private string ValidatingTypeName { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseValidator{TToValidate}"/> class.
        /// </summary>
        protected BaseValidator()
        {
            ValidatingTypeName = GetType().ValidatorBaseType()!.GetGenericArguments().First().Name;
            _baggedValidationResult = new ValidationResult(ValidatingTypeName);
        }

        /// <summary>
        /// Returns a successful validation result with the relevant validating type name set.
        /// </summary>
        protected ValidationResult Success()
        {
            return new ValidationResult(ValidatingTypeName);
        }

        /// <summary>
        /// Returns the bagged validation result composed from any bagged failures.
        /// </summary>
        protected ValidationResult BaggedValidationResult()
        {
            return _baggedValidationResult;
        }

        private void InitialisePropertyErrorsIfRequired(string property)
        {
            if (_baggedValidationResult.Failures.ContainsKey(property))
            {
                return;
            }

            _baggedValidationResult.Failures[property] = new List<string>();
        }

        private static string GetNameOfField<TField>(Expression<Func<TToValidate, TField>> expression)
        {
            return expression.Body switch
            {
                MemberExpression memberExpression => memberExpression.Member.Name,
                UnaryExpression {Operand: MemberExpression m} => m.Member.Name,
                _ => string.Empty
            };
        }
    }
}