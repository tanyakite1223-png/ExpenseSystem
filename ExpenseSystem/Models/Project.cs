using System.ComponentModel.DataAnnotations;

namespace ExpenseSystem.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Display(Name = "專案名稱")]
        public string ProjectName { get; set; }

        [Display(Name = "封存")]
        public bool IsActive { get; set; }

        public List<ExpenseDetail>? ExpenseDetails { get; set; }


    }
}