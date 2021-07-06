using System;
using System.Linq;
using System.Linq.Expressions;

namespace Baseline.Validate
{
    /// <summary>
    /// Base validator used by other validators in the library. Contains common functionality between them all.
    /// </summary>
    public abstract partial class BaseValidator<TToValidate>
    {
        private readonly ValidationResult _baggedValidationResult;
        
        protected string ValidatingTypeName { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseValidator{TToValidate}"/> class.
        /// </summary>
        protected BaseValidator()
        {
            ValidatingTypeName = GetType().BaseType!.GenericTypeArguments.First().Name;
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

        /// <summary>
        /// Gets and returns the name of a field from an expression. Allows consumers of the library to do funky things
        /// like <code>FailureFor(user => user.Name, "This failed!");</code>
        /// </summary>
        /// <param name="expression">The expression to retrieve the field name from.</param>
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