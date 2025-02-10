using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Common;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;

namespace ProphecyInternational.Database.DbContexts
{
    public class CallCenterManagementDbContext : DbContext
    {
        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<CallModel> Calls { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }

        public CallCenterManagementDbContext(DbContextOptions<CallCenterManagementDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define foreign keys
            modelBuilder.Entity<TicketModel>()
           .HasOne<CustomerModel>()
           .WithMany()
           .HasForeignKey(t => t.CustomerId);

            modelBuilder.Entity<TicketModel>()
                .HasOne<AgentModel>()
                .WithMany()
                .HasForeignKey(t => t.AgentId);

            modelBuilder.Entity<CallModel>()
                .HasOne<CustomerModel>()
                .WithMany()
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<CallModel>()
                .HasOne<AgentModel>()
                .WithMany()
                .HasForeignKey(c => c.AgentId);

            // Seed data
            modelBuilder.Entity<AgentModel>().HasData(
                new AgentModel { Id = 1, Name = "Zeylee Geronimo", Email = "zgeronimo@testdomain.com", PhoneExtension = "1001", Status = AgentStatus.Available },
                new AgentModel { Id = 2, Name = "Xanlaneron Nerier", Email = "xnerier@testdomain.com", PhoneExtension = "1002", Status = AgentStatus.Busy },
                new AgentModel { Id = 3, Name = "Jehiel Balla", Email = "jballa@testdomain.com", PhoneExtension = "1003", Status = AgentStatus.Offline }
            );

            modelBuilder.Entity<CustomerModel>().HasData(
                new CustomerModel { Id = "CUST001", Name = "Maria Clara", Email = "mclara@test.com", PhoneNumber = "1234567890", LastContactDate = null },
                new CustomerModel { Id = "CUST002", Name = "Crisostomo Ibarra", Email = "cibarra@test.com", PhoneNumber = "1234567891", LastContactDate = Constants.DEFAULT_DATE }
            );

            modelBuilder.Entity<TicketModel>().HasData(
                new TicketModel { Id = 1, CustomerId = "CUST001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = Constants.DEFAULT_DATE, UpdatedAt = Constants.DEFAULT_DATE, Description = "Issue with login", Resolution = null },
                new TicketModel { Id = 2, CustomerId = "CUST002", AgentId = 2, Status = TicketStatus.InProgress, Priority = TicketPriority.Medium, CreatedAt = Constants.DEFAULT_DATE, UpdatedAt = Constants.DEFAULT_DATE, Description = "Billing discrepancy", Resolution = null }
            );

            modelBuilder.Entity<CallModel>().HasData(
                new CallModel { Id = 1, CustomerId = "CUST001", AgentId = 1, StartTime = Constants.DEFAULT_DATE, EndTime = null, Status = CallStatus.InProgress, Notes = "Customer called regarding login issue" },
                new CallModel { Id = 2, CustomerId = "CUST002", AgentId = 2, StartTime = Constants.DEFAULT_DATE.AddMinutes(-30), EndTime = Constants.DEFAULT_DATE, Status = CallStatus.Completed, Notes = "Resolved billing discrepancy" }
            );
        }

    }
}
    