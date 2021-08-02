using System;
using System.Linq;
using System.Reflection;
using Baseline.Validate.Internal.Extensions;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Baseline.Validate
{
    /// <summary>
    /// Extension methods related to dependency injection/IoC containers.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds all validators and their interface registrations from the assemblies specified to the service
        /// collection. 
        /// </summary>
        /// <param name="serviceCollection">The service collection to add the validators to.</param>
        /// <param name="assemblies">The assemblies to retrieve the validators from.</param>
        public static IServiceCollection AddBaselineValidate(
            this IServiceCollection serviceCollection, 
            params Assembly[] assemblies
        )
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            
            foreach (var assembly in assemblies)
            {
                var validatorTypesInAssembly = assembly
                    .GetTypes()
                    .Where(a => a.IsRegisterableValidator())
                    .Select(t => new
                    {
                        ValidatorType = t,
                        AsyncValidator = t.ValidatorBaseType()!.GetGenericTypeDefinition() == typeof(AsyncValidator<>),
                        ValidatingType = t.ValidatorBaseType()!.GenericTypeArguments.First()
                    });

                foreach (var validatorTypeInAssembly in validatorTypesInAssembly)
                {
                    // Register the plain ol' validation instance so it can be resolved i.e. UserInstance.
                    serviceCollection.AddTransient(validatorTypeInAssembly.ValidatorType);
                    
                    // Register the interface implementation (i.e. ISyncValidator<T>, IAsyncValidator<T>) dependent
                    // on whether it's an async validator or not.
                    serviceCollection.AddTransient(
                        validatorTypeInAssembly.AsyncValidator
                            ? typeof(IAsyncValidator<>).MakeGenericType(validatorTypeInAssembly.ValidatingType)
                            : typeof(ISyncValidator<>).MakeGenericType(validatorTypeInAssembly.ValidatingType),
                        validatorTypeInAssembly.ValidatorType
                    );
                }
            }

            return serviceCollection;
        }
    }
}