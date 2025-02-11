using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Common;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;
using ProphecyInternational.Server.Services;

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

//Add Controllers
builder.Services.AddControllers();

//Add Controllers
builder.Services.AddLogging();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
app.UseAuthorization();
app.MapControllers();

app.Run();
