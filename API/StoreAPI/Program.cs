using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Repositories;
using Serilog;
using StoreAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region JIRA STOREREPO - 10 JWT START
var userPoolId  = Environment.GetEnvironmentVariable("UserPoolId");
var awsRegion = builder.Configuration["AWS:Region"];
var authority = $"https://cognito-idp.{awsRegion}.amazonaws.com/{userPoolId}";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = authority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authority,
        ValidateAudience = true,
        ValidAudience=Environment.GetEnvironmentVariable("ClientID_new_userpool"),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    Log.Information($"User Pool ID = {userPoolId}\nAWS Region = {awsRegion}\nAuthority URL = {authority}");
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Log.Warning($"Authentication Failed:{context.Exception.Message}");
            //Console.WriteLine($"Authentication Failed:{context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Log.Information("Authentication validated Successfully");
           // Console.WriteLine($"Authentication Validated Sucessfully");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            if (string.IsNullOrEmpty(context.Token))
            {
                Log.Warning("No JWT token received in the request.");
            }
            else
            {
                Log.Information("JWT Token received.");
            }
            return Task.CompletedTask;
        }
    };
});

// Enforce authorization globally
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

#endregion JIRA STOREREPO - 10 JWT END

// Add services to the container.
// Configure Serilog

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IServiceS3, AmazonS3Repository>();
builder.Services.AddScoped<AWSCognitoService, AWSCognitoService>();



builder.Services.AddDbContext<DatabaseDetails>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(m => m.AllowAnyHeader().AllowAnyOrigin().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
