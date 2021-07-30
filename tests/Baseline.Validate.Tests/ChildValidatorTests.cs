using FluentAssertions;
using Xunit;

namespace Baseline.Validate.Tests
{
    #region dependencies
    public class Parent
    {
        public Child? Child { get; set; }
    }

    public class Child
    {
        public string? Name { get; set; }
    }

    public class ParentValidator : SyncValidator<Parent>
    {
        public override ValidationResult Validate(Parent toValidate)
        {
            var childValidationResult = new ChildValidator().Validate(toValidate?.Child);
            if (!childValidationResult.Success)
            {
                return FailureFor(tv => tv.Child, childValidationResult);
            }

            return Success();
        }
    }

    public class ChildValidator : SyncValidator<Child>
    {
        public override ValidationResult Validate(Child? toValidate)
        {
            if (toValidate!.Name == null)
            {
                return FailureFor(tv => tv.Name, ":property is required.");
            }

            return Success();
        }
    }
    #endregion
    
    public class ChildValidatorTests
    {
        [Fact]
        public void It_Successfully_Validates_A_Child_Validator_And_Returns_Immediately_If_Required()
        {
            // Arrange.
            var toValidate = new Parent {Child = new Child()};
            
            // Act.
            var validationResult = new ParentValidator().Validate(toValidate);
            
            // Assert.
            validationResult.Success.Should().BeFalse();
            validationResult.Failures.Should().ContainKey("Child.Name");
            validationResult.Failures["Child.Name"].Should().Contain("Name is required.");
        }
    }
}