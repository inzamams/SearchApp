using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SearchApp.Business.Interface;
using SearchApp.Business.Services;
using SearchApp.Core.Common;
using SearchApp.Core.Dapper;
using SearchApp.Data.Interface;
using SearchApp.Data.Services;
using SearchApp.Extensions;
using Serilog;
using System.Text;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Async(a =>
    {
        a.File(Constants.ProgramConstants.LogFileName, outputTemplate: Constants.ProgramConstants.LogDateTimeFormat, rollingInterval: RollingInterval.Hour); // <<<<<
    })
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddScoped<IDapperManager, DapperManager>();
builder.Services.AddScoped<ISearchManager, SearchManager>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7240", "http://localhost:5025")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ResponseLoggingMiddleware>();

var logger = app.Services.GetRequiredService<ILogger>();
app.ConfigureExceptionHandler(logger);

app.MapControllers();

app.Run();
