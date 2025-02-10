using Microsoft.AspNetCore.Mvc;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Controllers
{
    /// <summary>
    /// AgentController is responsible for handling agent-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
        private readonly IGenericService<AgentModel, int> _agentService;

        public AgentController(ILogger<AgentController> logger, IGenericService<AgentModel, int> agentService)
        {
            _logger = logger;
            _agentService = agentService;
        }

        /// <summary>
        /// Gets a list of all agents.
        /// </summary>
        /// <returns>A list of AgentModel objects.</returns>
        [HttpGet("GetAllAgents")]
        public async Task<ActionResult<IEnumerable<AgentModel>>> GetAllAgents()
        {
            try
            {
                return Ok(await _agentService.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting agents.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific agent by ID.
        /// </summary>
        /// <param name="id">The ID of the agent to retrieve.</param>
        /// <returns>An AgentModel object.</returns>
        [HttpGet("GetAgent/{id}", Name = "GetAgent")]
        public async Task<ActionResult<AgentModel>> GetAgent(int id)
        {
            try
            {
                var agent = await _agentService.GetByIdAsync(id);
                if (agent == null)
                {
                    return NotFound();
                }
                return Ok(agent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting agent with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new agent.
        /// </summary>
        /// <param name="agent">The AgentModel object to create.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("CreateAgent")]
        public async Task<ActionResult> CreateAgent(AgentModel agent)
        {
            try
            {
                await _agentService.AddAsync(agent);
                return CreatedAtAction("GetAgent", new { id = agent.Id }, agent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an agent.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing agent.
        /// </summary>
        /// <param name="id">The ID of the agent to update.</param>
        /// <param name="agent">The updated AgentModel object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("UpdateAgent/{id}", Name = "UpdateAgent")]
        public async Task<IActionResult> UpdateAgent(int id, AgentModel agent)
        {
            if (id != agent.Id)
            {
                return BadRequest();
            }

            try
            {
                await _agentService.UpdateAsync(agent);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating agent with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an existing agent.
        /// </summary>
        /// <param name="id">The ID of the agent to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("DeleteAgent/{id}", Name = "DeleteAgent")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            try
            {
                await _agentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting agent with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates the status of an agent.
        /// </summary>
        /// <param name="id">The ID of the agent whose status is to be updated.</param>
        /// <param name="status">The new status of the agent.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPatch("UpdateAgentStatus/{id}/status", Name = "UpdateAgentStatus")]
        public async Task<IActionResult> UpdateAgentStatus(int id, [FromBody] AgentStatus status)
        {
            try
            {
                var agent = await _agentService.GetByIdAsync(id);
                if (agent == null)
                {
                    return NotFound();
                }
                agent.Status = status;
                await _agentService.UpdateAsync(agent);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating status of agent with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
