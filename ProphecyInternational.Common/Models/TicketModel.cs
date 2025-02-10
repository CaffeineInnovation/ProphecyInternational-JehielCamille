using ProphecyInternational.Common.Enums;
using System;

namespace ProphecyInternational.Common.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int? AgentId { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Description { get; set; }
        public string Resolution { get; set; }
    }
}
