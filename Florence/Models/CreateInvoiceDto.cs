namespace Florence.Models
{
    public class CreateInvoiceDto
    {
        public DateOnly DueDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatRate { get; set; } = 0.11m;
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; } = "USD";
        public string PaymentMethod { get; set; } = "Cash";
        public string Notes { get; set; } = string.Empty;
        public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }

    public class CreateInvoiceItemDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
    }
}
