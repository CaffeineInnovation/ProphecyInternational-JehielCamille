using System;

namespace ProphecyInternational.Common.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastContactDate { get; set; }
    }
}
