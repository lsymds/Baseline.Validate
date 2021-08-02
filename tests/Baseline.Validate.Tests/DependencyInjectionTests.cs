using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Baseline.Validate.Tests
{
    public class TestClass
    {
        
    }

    public class TestClassSyncValidator : SyncValidator<TestClass?>
    {
        public override ValidationResult Validate(TestClass? toValidate)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestClassAsyncValidator : AsyncValidator<TestClass>
    {
        public override ValueTask<ValidationResult> ValidateAsync(
            TestClass toValidate, 
            CancellationToken cancellationToken = default
        )
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class MultiLevelTestClass
    {}

    public abstract class BaseBaseBaseMultiLevelTestClassValidator : AsyncValidator<MultiLevelTestClass>
    {
    }

    public abstract class BaseBaseMultiLevelTestClassValidator : BaseBaseBaseMultiLevelTestClassValidator
    {
    }

    public abstract class BaseMultiLevelTestClassValidator : BaseBaseMultiLevelTestClassValidator
    {
    }

    public class MultiLevelTestClassValidator : BaseMultiLevelTestClassValidator
    {
        public override ValueTask<ValidationResult> ValidateAsync(
            MultiLevelTestClass toValidate, 
            CancellationToken cancellationToken = default
        )
        {
            throw new System.NotImplementedException();
        }
    }

    public class DependencyInjectionTests
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        
        [Fact]
        public void It_Registers_Synchronous_Validators_Correctly()
        {
            // Arrange.
            _serviceCollection.AddBaselineValidate(typeof(DependencyInjectionTests).Assembly);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            // Act.
            var testClassValidator = serviceProvider.GetService<TestClassSyncValidator>();
            
            // Assert.
            testClassValidator.Should().NotBeNull();
        }

        [Fact]
        public void It_Registers_Asynchronous_Validators_Correctly()
        {
            // Arrange.
            _serviceCollection.AddBaselineValidate(typeof(DependencyInjectionTests).Assembly);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            // Act.
            var testClassAsyncValidator = serviceProvider.GetService<TestClassAsyncValidator>();
            
            // Assert.
            testClassAsyncValidator.Should().NotBeNull();
        }
        
        [Fact]
        public void It_Registers_Synchronous_Validators_With_The_Sync_Interface_Correctly()
        {
            // Arrange.
            _serviceCollection.AddBaselineValidate(typeof(DependencyInjectionTests).Assembly);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            // Act.
            var testClassValidator = serviceProvider.GetService<ISyncValidator<TestClass>>();
            
            // Assert.
            testClassValidator.Should().NotBeNull();
        }

        [Fact]
        public void It_Registers_Asynchronous_Validators_With_The_Async_Interface_Correctly()
        {
            // Arrange.
            _serviceCollection.AddBaselineValidate(typeof(DependencyInjectionTests).Assembly);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            // Act.
            var testClassAsyncValidator = serviceProvider.GetService<IAsyncValidator<TestClass>>();
            
            // Assert.
            testClassAsyncValidator.Should().NotBeNull();
        }

        [Fact]
        public void It_Registers_Validators_With_Nested_Base_Classes()
        {
            // Arrange.
            _serviceCollection.AddBaselineValidate(typeof(DependencyInjectionTests).Assembly);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            // Act.
            var multiLevelValidatorRegistration = serviceProvider.GetService<MultiLevelTestClassValidator>();
            var multiLevelValidatorInterfaceRegistration = serviceProvider.GetService<IAsyncValidator<MultiLevelTestClass>>();
            
            // Assert.
            multiLevelValidatorRegistration.Should().NotBeNull();
            multiLevelValidatorInterfaceRegistration.Should().NotBeNull();
        }
    }
}