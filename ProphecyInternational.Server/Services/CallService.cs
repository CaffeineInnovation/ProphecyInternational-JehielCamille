using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;
using ProphecyInternational.Server.Models;

namespace ProphecyInternational.Server.Services
{
    public class CallService : IGenericService<CallModel, int>, IPagedGenericService<CallModel>
    {
        private readonly CallCenterManagementDbContext _dbContext;

        public CallService(CallCenterManagementDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CallModel>> GetAllAsync()
        {
            return await _dbContext.Calls
                .Select(call => new CallModel 
                { 
                    Id = call.Id, 
                    CustomerId = call.CustomerId, 
                    AgentId = call.AgentId,
                    StartTime = call.StartTime,
                    EndTime = call.EndTime,
                    Status = call.Status,
                    Notes = call.Notes
                }).ToListAsync();
        }

        public async Task<CallModel> GetByIdAsync(int callId)
        {
            return await _dbContext.Calls
               .Where(a => a.Id == callId)
               .Select(call => new CallModel
               {
                   Id = call.Id,
                   CustomerId = call.CustomerId,
                   AgentId = call.AgentId,
                   StartTime = call.StartTime,
                   EndTime = call.EndTime,
                   Status = call.Status,
                   Notes = call.Notes
               }).FirstOrDefaultAsync();
        }

        public async Task AddAsync(CallModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Calls.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CallModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingCall = await _dbContext.Calls.FindAsync(entity.Id);
            if (existingCall == null)
            {
                throw new KeyNotFoundException($"Call with ID {entity.Id} not found.");
            }

            existingCall.CustomerId = entity.CustomerId;
            existingCall.AgentId = entity.AgentId;
            existingCall.StartTime = entity.StartTime;
            existingCall.EndTime = entity.EndTime;
            existingCall.Status = entity.Status;
            existingCall.Notes = entity.Notes;

            _dbContext.Calls.Update(existingCall);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var call = await _dbContext.Calls.FindAsync(id);
            if (call == null)
            {
                throw new KeyNotFoundException($"Call with ID {id} not found.");
            }

            _dbContext.Calls.Remove(call);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<CallModel>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _dbContext.Calls
                .Select(call => new CallModel
                {
                    Id = call.Id,
                    CustomerId = call.CustomerId,
                    AgentId = call.AgentId,
                    StartTime = call.StartTime,
                    EndTime = call.EndTime,
                    Status = call.Status,
                    Notes = call.Notes
                });

            var totalCount = await query.CountAsync(); // Total records count
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CallModel>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
