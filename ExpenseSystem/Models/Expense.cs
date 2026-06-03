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

        [Display(Name = "金額")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "消費日期")]
        public DateTime ExpenseDate { get; set; }

        [Display(Name = "描述內容")]
        public string Description { get; set; }

        [Display(Name = "拒絕原因")]
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

