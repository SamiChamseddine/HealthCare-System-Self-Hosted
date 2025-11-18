using System.ComponentModel.DataAnnotations;

namespace Florence.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [Required]
        public DateOnly DueDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [Range(0, 1)]
        public decimal VatRate { get; set; } = 0.11m;

        [Range(0, double.MaxValue)]
        public decimal VatAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
        public DateOnly? PaymentDate { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        public string PaymentMethod { get; set; } = "Cash";
        public string Notes { get; set; } = string.Empty;

        public List<InvoiceItem> Items { get; set; } = new();

        public enum InvoiceStatus
        {
            Pending,    
            Paid,       
            Late,       
            Cancelled,  
            Deleted     
        }

        public void AddPayment(decimal amount, string method)
        {
            Status = InvoiceStatus.Paid;
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            
        }

        public void CheckOverdue()
        {
            if (DueDate < DateOnly.FromDateTime(DateTime.UtcNow) &&
                Status == InvoiceStatus.Pending)
            {
                Status = InvoiceStatus.Late;
                LastModified = DateTime.UtcNow;
            }
        }

        public void Delete()
        {
            if (Status != InvoiceStatus.Paid)
            {
                Status = InvoiceStatus.Deleted;
                LastModified = DateTime.UtcNow;
            }
        }
    }
}
