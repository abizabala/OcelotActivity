using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// I-load ang ocelot.json configuration file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 1. I-configure ang JWT Bearer Authentication
builder.Services.AddAuthentication()
    .AddJwtBearer("GatewayKey", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "AuthService",
            ValidAudience = "GatewayClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey1234567890123456"))
        };
    });

// 2. I-register ang Ocelot Services
builder.Services.AddOcelot();

var app = builder.Build();

// 3. KRITIKAL: Authentication & Authorization Middlewares
app.UseAuthentication();
app.UseAuthorization();

// 4. Patakbuhin ang Ocelot Gateway Middleware
await app.UseOcelot();

app.Run();