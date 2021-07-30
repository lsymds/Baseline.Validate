using FluentAssertions;
using Xunit;

namespace Baseline.Validate.Tests
{
    #region dependencies
    public class FailureMessageSubstitutionClass
    {
        public string? Name { get; set; }
    }

    public class FailureMessageFailureForSubstitutionClassValidator : SyncValidator<FailureMessageSubstitutionClass>
    {
        public override ValidationResult Validate(FailureMessageSubstitutionClass toValidate)
        {
            if (toValidate.Name == null)
            {
                return FailureFor(fc => fc.Name, ":property is required.");
            }

            return Success();
        }
    }

    public class FailureMessageBaggedFailureForSubstitutionClassValidator : SyncValidator<FailureMessageSubstitutionClass>
    {
        public override ValidationResult Validate(FailureMessageSubstitutionClass toValidate)
        {
            if (toValidate.Name == null)
            {
                BagFailureFor(fc => fc.Name, ":property is required.");
            }

            return BaggedValidationResult();
        }
    }
    #endregion
    
    public class FailureMessageSubstitutionTests
    {
        [Fact]
        public void It_Substitutes_The_Property_Name_In_A_Failure_For_Validation_Result()
        {
            // Arrange.
            var failureMessageSubstitutionClass = new FailureMessageSubstitutionClass();
            
            // Act.
            var validationResult = new FailureMessageFailureForSubstitutionClassValidator()
                .Validate(failureMessageSubstitutionClass);
            
            // Assert.
            validationResult.Failures["Name"].Should().Contain("Name is required.");
        }
        
        [Fact]
        public void It_Substitutes_The_Property_Name_In_A_Bagged_Failure_For_Validation_Result()
        {
            // Arrange.
            var failureMessageSubstitutionClass = new FailureMessageSubstitutionClass();
            
            // Act.
            var validationResult = new FailureMessageBaggedFailureForSubstitutionClassValidator()
                .Validate(failureMessageSubstitutionClass);
            
            // Assert.
            validationResult.Failures["Name"].Should().Contain("Name is required.");
        }
    }
}