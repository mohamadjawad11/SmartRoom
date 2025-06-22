using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Data;
using Microsoft.OpenApi.Models;  // This is necessary for OpenAPI types
using Swashbuckle.AspNetCore.SwaggerGen;  // This is necessary for SchemaFilter
using WebApi;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Register AppDbContext with SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173", // React frontend
                "http://localhost:5091"  // Swagger UI running from API port
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// ✅ Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-app", // ✅ Replace with your issuer
            ValidAudience = "your-app", // ✅ Replace with your audience
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key")) // ✅ Replace with a secure key
        };
    });

// Swagger (OpenAPI) Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
    // Add custom schema filter to handle enums as strings in Swagger UI
    // c.SchemaFilter<EnumSchemaFilter>();
    // Optional: If you have XML documentation for your API
   // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.xml"));
});

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
        c.RoutePrefix = string.Empty; // Optional: Set Swagger UI to appear at the root URL
    });
}

app.UseHttpsRedirection();

// Enable CORS before controllers
app.UseCors(MyAllowSpecificOrigins);

// ✅ Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers (this is where the routing for your API happens)
app.MapControllers(); // Enables API routing via controllers

app.Run();
