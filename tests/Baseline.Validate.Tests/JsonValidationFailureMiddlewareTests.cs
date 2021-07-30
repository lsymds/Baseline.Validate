using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Baseline.Validate.Tests
{
    public class MiddlewareClass
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public class MiddlewareValidationClass : SyncValidator<MiddlewareClass>
    {
        public override ValidationResult Validate(MiddlewareClass toValidate)
        {
            if (toValidate.Name == null)
            {
                BagFailureFor(x => x.Name, ":property is required.");
            }

            if (toValidate.Age == 0)
            {
                BagFailureFor(x => x.Age, ":property must be greater than 0.");
            }

            return BaggedValidationResult();
        }
    }
    
    public class JsonValidationFailureMiddlewareTests : IAsyncLifetime
    {
        private IHost _host = null!;
        private HttpClient _client = null!;

        public async Task InitializeAsync()
        {
            _host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseBaselineValidateJsonMiddleware();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", context => context.Response.WriteAsync("Alive!"));
                                endpoints.MapGet("/exception", _ =>
                                {
                                    new MiddlewareValidationClass().ValidateAndThrow(new MiddlewareClass());
                                    return Task.CompletedTask;
                                });
                            });
                        });
                })
                .StartAsync();
            _client = _host.GetTestClient();
        }

        public async Task DisposeAsync()
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        [Fact]
        public async Task It_Creates_A_Test_Server_Correctly()
        {
            // Act.
            var response = await _host.GetTestClient().GetAsync("/");
            
            // Assert.
            (await response.Content.ReadAsStringAsync()).Should().Be("Alive!");
        }
        
        [Fact]
        public async Task It_Handles_And_Returns_Json_Validation_Errors_Correctly()
        {
            // Act.
            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            
            var response = await _client.GetAsync("/exception");
            
            // Assert.
            (await response.Content.ReadAsStringAsync())
                .Should()
                .Be(
                    @"{""reason"":""Validation failure."",""validationFailures"":[{""property"":""Name"",""messages"":[""Name is required.""]},{""property"":""Age"",""messages"":[""Age must be greater than 0.""]}]}"
                );
            response.StatusCode.Should().Be(422);
            response.Headers.CacheControl.NoCache.Should().BeTrue();
            response.Headers.CacheControl.NoStore.Should().BeTrue();
            response.Headers.Pragma.First().Name.Should().Be("no-cache");
            response.Headers.ETag.Should().BeNull();
        }
    }
}