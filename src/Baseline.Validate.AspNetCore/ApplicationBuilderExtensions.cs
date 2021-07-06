using Microsoft.AspNetCore.Builder;

namespace Baseline.Validate
{
    /// <summary>
    /// Extensions related to the <see cref="IApplicationBuilder"/> interface. 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="JsonValidationFailureMiddleware"/> to the middleware pipeline of the application being
        /// built by the <see cref="applicationBuilder"/> parameter.
        /// </summary>
        /// <param name="applicationBuilder">The application being built.</param>
        public static void UseBaselineValidateJsonMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<JsonValidationFailureMiddleware>();
        }
    }
}