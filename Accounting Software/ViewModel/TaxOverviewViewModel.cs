namespace Accounting_Software.ViewModel
{
    public class TaxOverviewViewModel
    {
        public int UserId { get; set; }

        // Tax base: total income from sales
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }

        public List<TaxRuleViewModel> Rules { get; set; } = new();

        public decimal TotalRate => Rules.Sum(r => r.Rate);
        public decimal TotalTax => Rules.Sum(r => r.Amount);
        public decimal IncomeAfterTax => TotalIncome - TotalTax;
    }
}
