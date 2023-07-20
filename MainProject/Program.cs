using MainProject;

var builder = WebApplication.CreateBuilder(args);

var policyBuilder = new CircuitBreakerPolicy();

var policy = policyBuilder.GetCircuitBreakerPolicy();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient("providerApiClient", c =>
{
    c.BaseAddress = new Uri("https://localhost:7099");
}).AddTransientHttpErrorPolicy(p => policy);

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