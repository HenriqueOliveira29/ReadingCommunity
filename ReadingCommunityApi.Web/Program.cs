
using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Infrastructure.Data;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Infrastructure.Repositories;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Services;
using ReadingCommunityApi.Application.Middleware;
using ReadingCommunityApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Application.mappers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "otelcol-contrib";
var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://localhost:4317";

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: serviceName, serviceVersion: "1.0.0")
        .AddAttributes(new Dictionary<string, object>
        {
            ["deployment.environment"] = builder.Environment.EnvironmentName,
            ["host.name"] = Environment.MachineName
        }))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.EnrichWithHttpRequest = (activity, httpRequest) =>
            {
                activity.SetTag("http.client_ip", httpRequest.HttpContext.Connection.RemoteIpAddress);
                activity.SetTag("http.request_content_type", httpRequest.ContentType);
            };
            options.EnrichWithHttpResponse = (activity, httpResponse) =>
            {
                activity.SetTag("http.response_content_type", httpResponse.ContentType);
            };
        })
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation() // If using EF Core
        .AddSource(serviceName)
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);
        }))
    .WithMetrics(metrics => metrics
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        .AddMeter(serviceName)
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);
        }));

// Configure logging with OpenTelemetry
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;
    logging.AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri(otlpEndpoint);
    });
});

// Register custom services
builder.Services.AddSingleton<ActivitySource>(sp => new ActivitySource(serviceName));
builder.Services.AddSingleton<Meter>(sp => new Meter(serviceName));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var jwtSecret = builder.Configuration["JwtSettings:Secret"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JwtSettings:Secret is not configured in appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:Secret"] 
                ?? throw new InvalidOperationException("JWT Secret is not configured")
            )
        )
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ReadCommunityApp_";
});

builder.Services.AddAuthorization();

//Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IWishListCollectionRepository, WishListCollectionReposotory>();
builder.Services.AddScoped<IWishListItemRepository, WishListItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFollowRepository, UserFollowRepository>();

//Mappers
builder.Services.AddScoped<IBookMapper, BookMapper>();
builder.Services.AddScoped<IReviewMapper, ReviewMapper>();
builder.Services.AddScoped<IAuthorMapper, AuthorMapper>();
builder.Services.AddScoped<IWishListCollectionMapper, WishListCollectionMapper>();
builder.Services.AddScoped<IWishListItemMapper, WishListItemMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IWishListCollectionService, WishlistCollectionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Reading Community API",
        Version = "v1",
        Description = "API for Reading Community Application"
    });

    // Define the JWT security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token (without 'Bearer' prefix).\n\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });

    // Require JWT for all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestTelemetryMiddleware>();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();