using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;
using ProphecyInternational.Server.Models;

namespace ProphecyInternational.Server.Services
{
    public class TicketService : IGenericService<TicketModel, int>, IPagedGenericService<TicketModel>
    {
        private readonly CallCenterManagementDbContext _dbContext;

        public TicketService(CallCenterManagementDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<TicketModel>> GetAllAsync()
        {
            return await _dbContext.Tickets
                .Select(ticket => new TicketModel 
                { 
                    Id = ticket.Id, 
                    CustomerId = ticket.CustomerId, 
                    AgentId = ticket.AgentId, 
                    Status = ticket.Status, 
                    Priority = ticket.Priority, 
                    CreatedAt = ticket.CreatedAt, 
                    UpdatedAt = ticket.UpdatedAt, 
                    Description = ticket.Description, 
                    Resolution = ticket.Resolution
                }).ToListAsync();
        }

        public async Task<TicketModel> GetByIdAsync(int ticketId)
        {
            return await _dbContext.Tickets
               .Where(a => a.Id == ticketId)
               .Select(ticket => new TicketModel
               {
                   Id = ticket.Id,
                   CustomerId = ticket.CustomerId,
                   AgentId = ticket.AgentId,
                   Status = ticket.Status,
                   Priority = ticket.Priority,
                   CreatedAt = ticket.CreatedAt,
                   UpdatedAt = ticket.UpdatedAt,
                   Description = ticket.Description,
                   Resolution = ticket.Resolution
               }).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TicketModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Tickets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TicketModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingTicket = await _dbContext.Tickets.FindAsync(entity.Id);
            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {entity.Id} not found.");
            }

            existingTicket.CustomerId = entity.CustomerId;
            existingTicket.AgentId = entity.AgentId;
            existingTicket.Status = entity.Status;
            existingTicket.Priority = entity.Priority;
            existingTicket.CreatedAt = entity.CreatedAt;
            existingTicket.UpdatedAt = entity.UpdatedAt;
            existingTicket.Description = entity.Description;
            existingTicket.Resolution = entity.Resolution;

            _dbContext.Tickets.Update(existingTicket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            _dbContext.Tickets.Remove(ticket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<TicketModel>> GetPaginatedItemsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var totalCount = await _dbContext.Tickets.CountAsync();

            var tickets = await _dbContext.Tickets
                .OrderBy(t => t.Id) // Ensure ordering for consistent pagination
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ticket => new TicketModel
                {
                    Id = ticket.Id,
                    CustomerId = ticket.CustomerId,
                    AgentId = ticket.AgentId,
                    Status = ticket.Status,
                    Priority = ticket.Priority,
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    Description = ticket.Description,
                    Resolution = ticket.Resolution
                })
                .ToListAsync();

            return new PagedResult<TicketModel>
            {
                Items = tickets,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
        