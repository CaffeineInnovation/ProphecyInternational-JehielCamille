using ProphecyInternational.Common.Enums;
using System;

namespace ProphecyInternational.Common.Models
{
    public class CallModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int? AgentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public CallStatus Status { get; set; }
        public string Notes { get; set; }
    }
}
