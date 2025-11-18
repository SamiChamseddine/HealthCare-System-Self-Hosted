using System.Text.Json.Serialization;

namespace Florence.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public List<Invoice> Invoices { get; set; } = new();
    }
}
