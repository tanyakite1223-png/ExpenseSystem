namespace ExpenseSystem.Models
{
    public class ExpenseCreateViewModel
    {
        public Expense? Expense { get; set; }
        public List<ExpenseDetail>? ExpenseDetails { get; set; }
    }
}