using API.Middlewares;
using Infrastructure;
using Application; 
using API.Extensions.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
