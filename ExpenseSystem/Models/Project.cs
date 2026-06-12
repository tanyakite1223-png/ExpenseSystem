namespace ExpenseSystem.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsActive { get; set; }

        public List<ExpenseDetail>? ExpenseDetails { get; set; }

        public static explicit operator int(Project? v)
        {
            throw new NotImplementedException();
        }
    }
}