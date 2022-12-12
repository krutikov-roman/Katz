/*

By: Roman Krutikov

Description: This class is used set up the API before it is built, and includes defining where
             databases are, which kind of database is used, CORS settings, security, controllers,
             and more.
             
*/
using System.Text;
using API.Services;
using API.Models;
using API.Authorizations;
using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors Config
builder.Services.AddCors(options => {
    options.AddPolicy("AcceptAllPolicy", policy => {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


// https://www.red-gate.com/simple-talk/development/dotnet-development/policy-based-authorization-in-asp-net-core-a-deep-dive/
builder.Services.AddSingleton<IAuthorizationHandler, IsAdminAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>  
    {  
        bool adminRequirement = true;
        policy.Requirements.Add(new IsAdminRequirement(adminRequirement));
    });
});

builder.Services.AddEntityFrameworkSqlite();

builder.Services.AddDbContext<Database>();
builder.Services.AddDbContext<DatabaseIdentities>();

builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<DatabaseIdentities>()
.AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddScoped<TokenService>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Katz Veryz Securez Keyz 123z"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AcceptAllPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
