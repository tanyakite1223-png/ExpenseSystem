using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseSystem.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Display(Name = "標題")]
        [Required(ErrorMessage = "Title必填")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "描述內容")]
        public string Description { get; set; }

        [Display(Name = "拒絕原因")]
        public string? RejectionReason { get; set; }
        public ExpenseStatus Status { get; set; }

        public string? ApplicantId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ExpenseDetail> ExpenseDetails { get; set; } = new();

        [NotMapped]
        public decimal TotalAmount => ExpenseDetails?.Sum(ed => ed.Amount) ?? 0;

    }

    public enum ExpenseStatus
    {
        [Display(Name = "草稿")]
        Draft,

        [Display(Name = "審核中")]
        Submitted,

        [Display(Name = "退／補件")]
        Returned,

        [Display(Name = "已核准")]
        Approved,

        [Display(Name = "已拒絕")]
        Rejected
    }
}

