using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Services
{
    public class AgentService : IGenericService<AgentModel, int>
    {
        private readonly CallCenterManagementDbContext _dbContext;

        public AgentService(CallCenterManagementDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AgentModel>> GetAllAsync()
        {
            return await _dbContext.Agents
                .Select(agent => new AgentModel
                {
                    Id = agent.Id,
                    Name = agent.Name,
                    Email = agent.Email,
                    PhoneExtension = agent.PhoneExtension,
                    Status = agent.Status
                }).ToListAsync();
        }

        public async Task<AgentModel> GetByIdAsync(int agentId)
        {
            return await _dbContext.Agents
               .Where(a => a.Id == agentId)
               .Select(agent => new AgentModel
               {
                   Id = agent.Id,
                   Name = agent.Name,
                   Email = agent.Email,
                   PhoneExtension = agent.PhoneExtension,
                   Status = agent.Status
               }).FirstOrDefaultAsync();
        }

        public async Task AddAsync(AgentModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Agents.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(AgentModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingAgent = await _dbContext.Agents.FindAsync(entity.Id);
            if (existingAgent == null)
            {
                throw new KeyNotFoundException($"Agent with ID {entity.Id} not found.");
            }

            existingAgent.Name = entity.Name;
            existingAgent.Email = entity.Email;
            existingAgent.PhoneExtension = entity.PhoneExtension;
            existingAgent.Status = entity.Status;

            _dbContext.Agents.Update(existingAgent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var agent = await _dbContext.Agents.FindAsync(id);
            if (agent == null)
            {
                throw new KeyNotFoundException($"Agent with ID {id} not found.");
            }

            // Get related calls
            _ = _dbContext.Calls.ForEachAsync(c => {
                if(c.AgentId == id)
                {
                    // Set AgentId to NULL
                    c.AgentId = null;
                }});
            _dbContext.SaveChanges();

            // Get related tickets
            _ = _dbContext.Tickets.ForEachAsync(c => {
                if (c.AgentId == id)
                {
                    // Set AgentId to NULL
                    c.AgentId = null;
                }
            });
            _dbContext.SaveChanges();

            _dbContext.Agents.Remove(agent);
            await _dbContext.SaveChangesAsync();
        }
    }
}
