using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseSystem.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Title必填")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }

        public string? RejectionReason { get; set; }
        public ExpenseStatus Status { get; set; }

        public string? ApplicantId { get; set; }

        public bool IsDeleted { get; set; }

    }

    public enum ExpenseStatus
    {
        [Display(Name = "草稿")]
        Draft,
        [Display(Name = "審核中")]
        Submitted,
        [Display(Name = "已核准")]
        Approved,
        [Display(Name = "已拒絕")]
        Rejected
    }
}

