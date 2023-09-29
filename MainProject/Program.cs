using MainProject;
using Polly;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var policyBuilder = new MainProject.PolicyBuilder();

        var circuitBreakerPolicy = policyBuilder.GetCircuitBreakerPolicy();

        HttpStatusCode[] httpStatusCodesWorthRetrying = {
           HttpStatusCode.RequestTimeout, // 408
           HttpStatusCode.InternalServerError, // 500
           HttpStatusCode.BadGateway, // 502
           HttpStatusCode.ServiceUnavailable, // 503
           HttpStatusCode.GatewayTimeout // 504
        };

        var retryPolicy = policyBuilder.GetRetryrPolicy(httpStatusCodesWorthRetrying);

        var wrappedPolicy = retryPolicy.WrapAsync(circuitBreakerPolicy);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddHttpClient("providerApiClient", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7099");
        }).AddTransientHttpErrorPolicy(p => retryPolicy);
        builder.Services.AddHttpClient("providerApiBClient", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7081");
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}