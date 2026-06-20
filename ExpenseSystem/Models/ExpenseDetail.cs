using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseSystem.Models
{
    public class ExpenseDetail
    {
        public int ExpenseDetailId { get; set; }

        [Display(Name = "單據類型")]
        public ExpenseReceipt ReceiptType { get; set; }

        [Display(Name = "費用類型")]
        public ExpenseCategory Category { get; set; }

        [Display(Name = "交易日期")]
        [Required(ErrorMessage = "交易日期必填")]
        public DateOnly? ExpenseDate { get; set; }

        [Display(Name = "商店名稱")]
        public string StoreName { get; set; }

        [Display(Name = "發票編號")]
        public string? InvoiceNumber { get; set; }

        [Display(Name = "金額")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "描述內容")]
        public string Description { get; set; }

        // FK — 存數字(指向哪張報銷單)
        public int ExpenseId { get; set; }

        [Display(Name = "專案名稱")]
        public int ProjectId { get; set; }

        // Navigation property — 存物件(EF Core 幫你把那張報銷單整個載回來)
        public Expense? Expense { get; set; }
        public Project? Project { get; set; }

    }


    public enum ExpenseCategory
    {
        [Display(Name = "交通費")]
        Transportation = 1,

        [Display(Name = "郵資")]
        Postage = 2,

        [Display(Name = "文具用品")]
        OfficeSupplies = 3,

        [Display(Name = "影印")]
        Copying = 4,

        [Display(Name = "餐飲費")]
        MealExpense = 5,

        [Display(Name = "雜項支出")]
        Miscellaneous = 6
    }

    public enum ExpenseReceipt
    {
        [Display(Name = "統一發票")]
        UniformInvoice = 0,

        [Display(Name = "收據")]
        Receipt = 1,

        [Display(Name = "購票證明")]
        TicketProof = 2,

        [Display(Name = "其他")]
        Other = 3
    }
}

