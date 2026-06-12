using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseSystem.Models
{
    public class ExpenseDetail
    {
        public int ExpenseDetailId { get; set; }
        public DateTime ExpenseDate { get; set; }      // 交易日期

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }           // 金額

        public string Description { get; set; }       // 描述內容

        // FK — 存數字(指向哪張報銷單)
        public int ExpenseId { get; set; }
        public int ProjectId { get; set; }

        // Navigation property — 存物件(EF Core 幫你把那張報銷單整個載回來)
        public Expense? Expense { get; set; }
        public Project? Project { get; set; }

    }
}

