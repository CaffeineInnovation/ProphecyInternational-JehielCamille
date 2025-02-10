using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Common;
using ProphecyInternational.Database.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString(Constants.DEFAULT_CONNECTION);

// Register ApplicationDbContext
builder.Services.AddDbContext<CallCenterManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CallCenterManagementDbContext>();
    dbContext.Database.Migrate();  // This is to make sure database is updated
}

app.Run();
