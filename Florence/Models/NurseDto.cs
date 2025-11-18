namespace Florence.Models
{
    public class NurseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public NursePosition Position { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int ExpenseItemCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
