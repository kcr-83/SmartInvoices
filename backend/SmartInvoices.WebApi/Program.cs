using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartInvoices.Application;
using SmartInvoices.Infrastructure;
using SmartInvoices.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Dodanie serwisów z warstwowej architektury
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

// Dodanie usług FastEndpoints
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

// Konfiguracja uwierzytelniania JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "defaultDevelopmentKey123!@#"))
        };
    });

builder.Services.AddAuthorization();

// Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder
                .WithOrigins(
                    "https://api.smart-invoices.example.com",
                    "https://api-staging.smart-invoices.example.com",
                    "http://localhost:4200" // Dla lokalnego developmentu Angular
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure HTTP request pipeline
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");

// Konfiguracja FastEndpoints
app.UseFastEndpoints(c => 
{
    c.Endpoints.RoutePrefix = "api/v1";
    c.Serializer.Options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    c.Versioning.Prefix = "v";
    c.Versioning.DefaultVersion = 1;
});

app.UseAuthentication();
app.UseAuthorization();

// Konfiguracja Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();
