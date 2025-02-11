using Microsoft.AspNetCore.Mvc;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Controllers
{
    /// <summary>
    /// CallController is responsible for handling call-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CallController : ControllerBase
    {
        private readonly ILogger<CallController> _logger;
        private readonly IGenericService<CallModel, int> _callService;

        public CallController(ILogger<CallController> logger, IGenericService<CallModel, int> callService)
        {
            _logger = logger;
            _callService = callService;
        }

        /// <summary>
        /// Gets a list of all calls.
        /// </summary>
        /// <returns>A list of CallModel objects.</returns>
        [HttpGet("GetAllCalls")]
        public async Task<ActionResult<IEnumerable<CallModel>>> GetAllCalls()
        {
            try
            {
                return Ok(await _callService.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting calls.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific call by ID.
        /// </summary>
        /// <param name="id">The ID of the call to retrieve.</param>
        /// <returns>An CallModel object.</returns>
        [HttpGet("GetCall/{id}", Name = "GetCall")]
        public async Task<ActionResult<CallModel>> GetCall(int id)
        {
            try
            {
                var call = await _callService.GetByIdAsync(id);
                if (call == null)
                {
                    return NotFound();
                }
                return Ok(call);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting call with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new call.
        /// </summary>
        /// <param name="call">The CallModel object to create.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("CreateCall")]
        public async Task<ActionResult> CreateCall(CallModel call)
        {
            try
            {
                await _callService.AddAsync(call);
                return CreatedAtAction("GetCall", new { id = call.Id }, call);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an call.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing call.
        /// </summary>
        /// <param name="id">The ID of the call to update.</param>
        /// <param name="call">The updated CallModel object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("UpdateCall/{id}", Name = "UpdateCall")]
        public async Task<IActionResult> UpdateCall(int id, CallModel call)
        {
            if (id != call.Id)
            {
                return BadRequest();
            }

            try
            {
                await _callService.UpdateAsync(call);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating call with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an existing call.
        /// </summary>
        /// <param name="id">The ID of the call to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("DeleteCall/{id}", Name = "DeleteCall")]
        public async Task<IActionResult> DeleteCall(int id)
        {
            try
            {
                await _callService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting call with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates the agent assigned to a call.
        /// </summary>
        /// <param name="id">The ID of the call whose agent is to be updated.</param>
        /// <param name="agentId">The new agent of the call.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPatch("AssignCallToAgent")]
        public async Task<IActionResult> AssignCallToAgent(int id, int agentId)
        {
            try
            {
                var call = await _callService.GetByIdAsync(id);
                if (call == null)
                {
                    return NotFound();
                }
                call.AgentId = agentId;
                await _callService.UpdateAsync(call);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating status of call with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
