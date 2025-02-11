using Microsoft.AspNetCore.Mvc;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Controllers
{
    /// <summary>
    /// TicketController is responsible for handling ticket-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IGenericService<TicketModel, int> _ticketService;

        public TicketController(ILogger<TicketController> logger, IGenericService<TicketModel, int> ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Gets a list of all tickets.
        /// </summary>
        /// <returns>A list of TicketModel objects.</returns>
        [HttpGet("GetAllTickets")]
        public async Task<ActionResult<IEnumerable<TicketModel>>> GetAllTickets()
        {
            try
            {
                return Ok(await _ticketService.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tickets.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific ticket by ID.
        /// </summary>
        /// <param name="id">The ID of the ticket to retrieve.</param>
        /// <returns>An TicketModel object.</returns>
        [HttpGet("GetTicket/{id}", Name = "GetTicket")]
        public async Task<ActionResult<TicketModel>> GetTicket([FromQuery] int id)
        {
            try
            {
                var ticket = await _ticketService.GetByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting ticket with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="ticket">The TicketModel object to create.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("CreateTicket")]
        public async Task<ActionResult> CreateTicket([FromBody] TicketModel ticket)
        {
            try
            {
                await _ticketService.AddAsync(ticket);
                return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an ticket.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing ticket.
        /// </summary>
        /// <param name="id">The ID of the ticket to update.</param>
        /// <param name="ticket">The updated TicketModel object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("UpdateTicket/{id}", Name = "UpdateTicket")]
        public async Task<IActionResult> UpdateTicket([FromQuery] int id, [FromBody] TicketModel ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            try
            {
                await _ticketService.UpdateAsync(ticket);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating ticket with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an existing ticket.
        /// </summary>
        /// <param name="id">The ID of the ticket to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("DeleteTicket/{id}", Name = "DeleteTicket")]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            try
            {
                await _ticketService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting ticket with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Assign ticket to an agent.
        /// </summary>
        /// <param name="id">The ID of the ticket whose status is to be updated.</param>
        /// <param name="agentId">The new agent of the ticket.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPatch("AsssignTicketToAgent", Name = "UpdateTicketStatus")]
        public async Task<IActionResult> AsssignTicketToAgent([FromQuery] int id, [FromQuery] int agentId)
        {
            try
            {
                var ticket = await _ticketService.GetByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                ticket.AgentId = agentId;
                await _ticketService.UpdateAsync(ticket);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating status of ticket with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
