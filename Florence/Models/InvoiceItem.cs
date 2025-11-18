using System.Text.Json.Serialization;

namespace Florence.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;

        public int InvoiceId { get; set; }
        [JsonIgnore]
        public Invoice Invoice { get; set; } = null!;
    }
}
