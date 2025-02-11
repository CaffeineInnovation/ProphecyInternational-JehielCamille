using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProphecyInternational.Common.Common;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;
using ProphecyInternational.Server.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get configurations from appsettings.json
var connectionString = builder.Configuration.GetConnectionString(Constants.DEFAULT_CONNECTION);

// Register DbContext with the connection string
builder.Services.AddDbContext<CallCenterManagementDbContext>(options => options.UseSqlServer(connectionString));

// Register Services
builder.Services.AddScoped<IGenericService<AgentModel, int>, AgentService>();
builder.Services.AddScoped<IGenericService<CallModel, int>, CallService>();
builder.Services.AddScoped<IGenericService<TicketModel, int>, TicketService>();
builder.Services.AddScoped<IGenericService<CustomerModel, string>, CustomerService>();

// Add Controllers and Logging
builder.Services.AddControllers();
builder.Services.AddLogging();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(Constants.Labels.V1, new OpenApiInfo { Title = Constants.Labels.TITLE, Version = Constants.Labels.V1 });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition(Constants.BEARER, new OpenApiSecurityScheme
    {
        Name = Constants.AUTHORIZATION,
        Type = SecuritySchemeType.Http,
        Scheme = Constants.BEARER,
        BearerFormat = Constants.JWT,
        In = ParameterLocation.Header,
        Description = Constants.ENTER_JWT_DESC
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = Constants.BEARER }
            },
            new string[] {}
        }
    });
});


// Configure JWT Authentication **before** builder.Build()
var key = builder.Configuration[Constants.JWT_KEY];
var issuer = builder.Configuration[Constants.JWT_ISSUER];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build(); // Build after all service registrations

// Apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CallCenterManagementDbContext>();
    dbContext.Database.Migrate();  // This is to make sure database is updated
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
