using FluentAssertions;
using Xunit;

namespace Baseline.Validate.Tests
{
    #region dependencies
    public class BagFailureClass
    {
        public string? PropertyOne { get; set; }
        public string? PropertyTwo { get; set; }
    }

    public class BagFailureClassValidator : SyncValidator<BagFailureClass>
    {
        public override ValidationResult Validate(BagFailureClass toValidate)
        {
            if (toValidate.PropertyOne == null)
            {
                BagFailureFor("PropertyOne", "PropertyOne is required.");
            }

            if (toValidate.PropertyTwo == null)
            {
                BagFailureFor("PropertyTwo", "PropertyTwo is required.");
            }

            return BaggedValidationResult();
        }
    }

    public class BagFailureClassWithExpressionValidator : SyncValidator<BagFailureClass>
    {
        public override ValidationResult Validate(BagFailureClass toValidate)
        {
            if (toValidate.PropertyOne == null)
            {
                BagFailureFor(fc => fc.PropertyOne, "PropertyOne is required.");
                BagFailureFor(fc => fc.PropertyOne, "PropertyOne is required, again.");
            }

            if (toValidate.PropertyTwo == null)
            {
                BagFailureFor(fc => fc.PropertyTwo, "PropertyTwo is required.");
            }

            return BaggedValidationResult();
        }
    }
    #endregion
    
    public class BagFailureForTests
    {
        [Fact]
        public void It_Successfully_Bags_Multiple_Failures_Together()
        {
            // Arrange.
            var bagFailureClass = new BagFailureClass();

            // Act.
            var validationResult = new BagFailureClassValidator().Validate(bagFailureClass);

            // Assert.
            validationResult.Success.Should().BeFalse();
            validationResult.Failures.Should().ContainKey("PropertyOne");
            validationResult.Failures.Should().ContainKey("PropertyTwo");
            validationResult.Failures["PropertyOne"].Should().HaveCount(2);
        }

        [Fact]
        public void It_Successfully_Bags_A_Single_Failure()
        {
            // Arrange.
            var bagFailureClass = new BagFailureClass { PropertyOne = "propertyOne" };

            // Act.
            var validationResult = new BagFailureClassValidator().Validate(bagFailureClass);

            // Assert.
            validationResult.Success.Should().BeFalse();
            validationResult.Failures.Should().HaveCount(1);
            validationResult.Failures.Should().ContainKey("PropertyTwo");
        }

        [Fact]
        public void It_Returns_A_Successful_Result_If_No_Failures_Occurred()
        {
            // Arrange.
            var bagFailureClass = new BagFailureClass { PropertyOne = "propertyOne", PropertyTwo = "propertyTwo" };

            // Act.
            var validationResult = new BagFailureClassValidator().Validate(bagFailureClass);

            // Assert.
            validationResult.Success.Should().BeTrue();
            validationResult.Failures.Should().HaveCount(0);
        }

        [Fact]
        public void It_Successfully_Determines_Simple_Bagged_Property_Names_Via_An_Expression()
        {
            // Arrange.
            var bagFailureClass = new BagFailureClass();

            // Act.
            var validationResult = new BagFailureClassWithExpressionValidator().Validate(bagFailureClass);

            // Assert.
            validationResult.Success.Should().BeFalse();
            validationResult.Failures.Should().ContainKey("PropertyOne");
            validationResult.Failures.Should().ContainKey("PropertyTwo");
        }
    }
}