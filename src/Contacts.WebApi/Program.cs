using Contacts.Application.Extensions;
using Contacts.Persistence.Extensions;
using Contacts.WebApi.Infrastructure.Extensions;
using Contacts.WebApi.Infrastructure.Filters;
using Contacts.WebApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;

const string corsPolicyName = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(dispose: true);

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("Contacts")!);
builder.Services.AddApplication();
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            corsPolicyName,
            policyBuilder => policyBuilder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = ApiVersion.Default;
    opt.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
    options.Filters.Add<ApiResponseResultFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In case of complex domain validation each validator should be registered separately and used from the commands
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseRouting();
app.RunHost(args);

public partial class Program { }
