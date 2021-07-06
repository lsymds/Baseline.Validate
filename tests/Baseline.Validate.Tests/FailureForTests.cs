using FluentAssertions;
using Xunit;

namespace Baseline.Validate.Tests
{
    #region dependencies
    public class FailureClass
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public class FailureClassValidator : SyncValidator<FailureClass>
    {
        public override ValidationResult Validate(FailureClass toValidate)
        {
            if (toValidate.Name == null)
            {
                return FailureFor("Name", "Name is required.");
            }

            if (toValidate.Age == 0)
            {
                return FailureFor("Age", "Age must be greater than 0.");
            }
            
            return Success();
        }
    }

    public class FailureClassWithExpressionValidator : SyncValidator<FailureClass>
    {
        public override ValidationResult Validate(FailureClass toValidate)
        {
            if (toValidate.Name == null)
            {
                return FailureFor(x => x.Name, "Name is required.");
            }

            if (toValidate.Age == 0)
            {
                return FailureFor(x => x.Age, "Age must be greater than 0.");
            }
            
            return Success();
        }
    }
    #endregion
    
    public class FailureForTests
    {
        [Fact]
        public void It_Successfully_Returns_The_First_Failure()
        {
            // Arrange.
            var failureClass = new FailureClass();
            
            // Act.
            var validationResult = new FailureClassValidator().Validate(failureClass);

            // Assert.
            validationResult.Failures.Should().HaveCount(1);
            validationResult.Failures.Should().ContainKey("Name");
        }

        [Fact] 
        public void It_Returns_Success_When_There_Are_No_Failures()
        {
            // Arrange.
            var failureClass = new FailureClass {Name = "foo", Age = 32};
            
            // Act.
            var validationResult = new FailureClassValidator().Validate(failureClass);

            // Assert.
            validationResult.Success.Should().BeTrue();
            validationResult.Failures.Should().HaveCount(0);
        }

        [Fact]
        public void It_Correctly_Computes_The_Property_Names_From_An_Expression()
        {
            // Arrange.
            var failureClass = new FailureClass {Name = "foo", Age = 0};
            
            // Act.
            var validationResult = new FailureClassWithExpressionValidator().Validate(failureClass);

            // Assert.
            validationResult.Success.Should().BeFalse();
            validationResult.Failures.Should().HaveCount(1);
            validationResult.Failures.Should().ContainKey("Age");
        }
    }
}