using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interface;
using WebApi.Repositories;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using Shared.Entities;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            //.WithOrigins("https://localhost:7254") // Blazor WASM URL
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


//builder.Services.AddControllers()
//    .AddOData(opt =>
//    {
//        var odataBuilder = new ODataConventionModelBuilder();
//        odataBuilder.EntitySet<LeaveRequestDto>("LeaveRequests");
//        opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
//            .Filter()
//            .Select()
//            .OrderBy()
//            .Expand()
//            .Count();
//    });


var secretKey = builder.Configuration["JwtSettings:SecretKey"] ??
    "ThisIsASuperLongSecretKeyForJWTToken1234567891011121312345678";
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowBlazor");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
