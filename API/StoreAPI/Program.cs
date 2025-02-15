using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Repositories;
using Serilog;
using StoreAPI.Service;

var builder = WebApplication.CreateBuilder(args);

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
app.UseAuthorization();

app.MapControllers();

app.Run();
