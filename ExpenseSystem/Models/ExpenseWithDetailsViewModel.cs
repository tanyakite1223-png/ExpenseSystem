namespace ExpenseSystem.Models
{
    public class ExpenseWithDetailsViewModel
    {
        public Expense? Expense { get; set; }
        public List<ExpenseDetail>? ExpenseDetails { get; set; }
    }
}