using ProphecyInternational.Common.Enums;

namespace ProphecyInternational.Common.Models
{
    public class AgentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneExtension { get; set; }
        public AgentStatus Status { get; set; }
    }

}
