using AutoMapper;
using DemoRESTWebApi;
using DemoRESTWebApi.Authorization;
using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Middleware;
using DemoRESTWebApi.MIddleware;
using DemoRESTWebApi.Models;
using DemoRESTWebApi.Models.Validators;
using DemoRESTWebApi.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Reflection.Metadata.Ecma335;
using System.Text;


// Nlog.
var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Nlog.
builder.Host.UseNLog();

// JwtBearer

ConfigurationManager configuration = builder.Configuration;

var authenticationSettings = new AuthenticationSettings();

builder.Services.AddSingleton(authenticationSettings);

configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    options.DefaultScheme = "Bearer";
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

// Custom Authorization Policy, does user have claim Nationality?
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
    o.AddPolicy("AtLeast20yo", builder => builder.AddRequirements(new MinimumAgeRequirment(20) ));

});


// Only Nationality claim with certain values:
//builder.Services.AddAuthorization(o => o.AddPolicy("HasNationality"
//    , builder => builder.RequireClaim("Nationality", "German", "Polish")));

// FluentValidation
builder.Services.AddFluentValidation();

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

builder.Services.AddDbContext<LibraryDbContext>();

builder.Services.AddScoped<LibraryDataSeeder>();

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<LibraryQuery>, LibraryQueryValidator>();
// HttpContextAccessor is needed to implement access to HttpContext out of controllers.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();


// Register Swagger services.
builder.Services.AddSwaggerGen();

// Register CORS policy. It's required to allow any requests from third party applications.
builder.Services.AddCors(o =>
    {
        o.AddPolicy("FrontEndClient", builder =>
            builder.AllowAnyMethod()
            .AllowAnyHeader()
            //.AllowAnyOrigin());
            // Allowed origins configuration from appsettings.json
            .WithOrigins(configuration["AllowedOrigins"]));

    }
);

// Registration of Service for dependency. For every type of registration we pass abstraction (Interface)
// and particular implementation.

// Register service as Singleton, to create only one instance of service.
//builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

// Register service as Scoped, to create only one instance of service per request.
//builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

// Register as Transient, to create instance of service every time we call service contructor
// (one instance for every call, may indicate plenty of new instances).
builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();

// Add AutoMapper to services container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();


// App builder !!
var app = builder.Build();

// Cache control - allows caching answers.
app.UseResponseCaching();

// Use static files. Microsoft.AspNetCore.StaticFiles is deprecated.
app.UseStaticFiles();

// Add CORS policy.
app.UseCors("FrontEndClient");

app.UseAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseHttpsRedirection();

// Use Swagger.
app.UseSwagger();
// Setup endpoint for swagger ui.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Web Api; ASP.NET CORE 6"));

app.UseRouting();

// Configure the HTTP request pipeline.

app.UseAuthorization();

//app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllers();

app.Run();

