using System;
using FluentAssertions;
using Xunit;

namespace Baseline.Validate.Tests
{
    #region dependencies
    public class ExceptionTestClass
    {
        public string? Name { get; set; }
    }

    public class ExceptionTestClassValidator : SyncValidator<ExceptionTestClass>
    {
        public override ValidationResult Validate(ExceptionTestClass? toValidate)
        {
            if (toValidate?.Name == null)
            {
                return FailureFor("Name", "Name is required.");
            }

            return Success();
        }
    }
    #endregion
    
    public class ExceptionBasedTests
    {
        [Fact]
        public void It_Throws_An_Exception_With_A_Correct_Class_Name()
        {
            // Arrange.
            var testClass = new ExceptionTestClass();

            // Act.
            Action sut = () => new ExceptionTestClassValidator().ValidateAndThrow(testClass);

            // Assert.
            sut.Should().Throw<ValidationFailedException>().WithMessage("Validation failed for object ExceptionTestClass*");
        }

        [Fact]
        public void It_Does_Not_Throw_An_Exception_When_Validation_Succeeds()
        {
            // Arrange.
            var testClass = new ExceptionTestClass {Name = "name"};
            
            // Act.
            new ExceptionTestClassValidator().ValidateAndThrow(testClass);
        }
    }
}