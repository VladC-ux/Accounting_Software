using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class ExportController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IStoreService _storeService;

        public ExportController(
            IUserRepository userRepository,
            ITransactionHistoryService transactionHistoryService,
            IProductService productService,
            ISellerService sellerService,
            IStoreService storeService)
        {
            _userRepository = userRepository;
            _transactionHistoryService = transactionHistoryService;
            _productService = productService;
            _sellerService = sellerService;
            _storeService = storeService;
        }

        // Export transaction history (with optional filters)
        public IActionResult Transactions(int userId, DateTime? dateFrom, DateTime? dateTo, string? actionType)
        {
            var transactions = _transactionHistoryService.GetHistoryByUserId(userId);

            if (dateFrom.HasValue)
                transactions = transactions.Where(t => t.SoldDate >= dateFrom.Value).ToList();

            if (dateTo.HasValue)
                transactions = transactions.Where(t => t.SoldDate <= dateTo.Value.AddDays(1).AddSeconds(-1)).ToList();

            if (!string.IsNullOrEmpty(actionType) && actionType != "All")
                transactions = transactions.Where(t => t.typeofAction == actionType).ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Transactions");

            // Title
            ws.Cell(1, 1).Value = "Transaction History";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;
            ws.Range(1, 1, 1, 9).Merge();

            // Filter info
            ws.Cell(2, 1).Value = $"Generated: {DateTime.Now:dd MMM yyyy HH:mm}";
            ws.Cell(2, 1).Style.Font.Italic = true;
            ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Range(2, 1, 2, 9).Merge();

            // Headers
            int headerRow = 4;
            var headers = new[] { "Date", "Type", "Product", "Store", "Count", "Mass", "Unit", "Price", "Total" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(headerRow, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a2e");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            // Data
            int row = headerRow + 1;
            foreach (var t in transactions)
            {
                bool isSale = t.typeofAction == "Sale";

                ws.Cell(row, 1).Value = t.SoldDate.ToString("dd MMM yyyy HH:mm");
                ws.Cell(row, 2).Value = t.typeofAction;
                ws.Cell(row, 3).Value = t.ProductName;
                ws.Cell(row, 4).Value = t.StoreName ?? "—";
                ws.Cell(row, 5).Value = t.Count;
                ws.Cell(row, 6).Value = t.Mass;
                ws.Cell(row, 7).Value = t.unitOfmass.ToString();
                ws.Cell(row, 8).Value = t.Price;
                ws.Cell(row, 9).Value = t.Total;

                ws.Cell(row, 8).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(row, 9).Style.NumberFormat.Format = "#,##0.00";

                // Color rows: Sale = light green, Add = light red
                var rowColor = isSale ? XLColor.FromHtml("#e8f5e9") : XLColor.FromHtml("#fce4ec");
                ws.Range(row, 1, row, 9).Style.Fill.BackgroundColor = rowColor;

                // Type cell color
                ws.Cell(row, 2).Style.Font.FontColor = isSale
                    ? XLColor.FromHtml("#2e7d32")
                    : XLColor.FromHtml("#c62828");
                ws.Cell(row, 2).Style.Font.Bold = true;

                // Amount color
                ws.Cell(row, 9).Style.Font.FontColor = isSale
                    ? XLColor.FromHtml("#2e7d32")
                    : XLColor.FromHtml("#c62828");

                for (int col = 1; col <= 9; col++)
                    ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                row++;
            }

            // Summary section
            row++;
            ws.Cell(row, 1).Value = "Summary";
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 1).Style.Font.FontSize = 11;
            row++;

            var spent  = transactions.Where(t => t.typeofAction == "Add").Sum(t => t.Total);
            var earned = transactions.Where(t => t.typeofAction == "Sale").Sum(t => t.Total);

            WritesSummaryRow(ws, row++, "Total Records", transactions.Count.ToString());
            WritesSummaryRow(ws, row++, "Total Spent",   spent.ToString("C"));
            WritesSummaryRow(ws, row++, "Total Earned",  earned.ToString("C"));
            WritesSummaryRow(ws, row++, "Net Profit",    (earned - spent).ToString("C"));

            ws.Columns().AdjustToContents();
            ws.Column(1).Width = Math.Max(ws.Column(1).Width, 18);

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // Export all products
        public IActionResult Products()
        {
            var products = _productService.GetAll();
            var sellers  = _sellerService.GetAll();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Products");

            ws.Cell(1, 1).Value = "Products";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;
            ws.Range(1, 1, 1, 7).Merge();

            ws.Cell(2, 1).Value = $"Generated: {DateTime.Now:dd MMM yyyy HH:mm}";
            ws.Cell(2, 1).Style.Font.Italic = true;
            ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
            ws.Range(2, 1, 2, 7).Merge();

            int headerRow = 4;
            var headers = new[] { "Name", "Seller", "Price", "Count", "Total", "Mass", "Unit" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(headerRow, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a2e");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            int row = headerRow + 1;
            bool alternate = false;
            foreach (var p in products)
            {
                var sellerName = sellers.FirstOrDefault(s => s.Id == p.SellerId)?.Name ?? "—";
                var rowColor = alternate ? XLColor.FromHtml("#f5f5f5") : XLColor.White;

                ws.Cell(row, 1).Value = p.Name;
                ws.Cell(row, 2).Value = sellerName;
                ws.Cell(row, 3).Value = p.Price;
                ws.Cell(row, 4).Value = p.Count;
                ws.Cell(row, 5).Value = p.Total;
                ws.Cell(row, 6).Value = p.Mass;
                ws.Cell(row, 7).Value = p.unitOfmass.ToString();

                ws.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(row, 5).Style.NumberFormat.Format = "#,##0.00";

                ws.Range(row, 1, row, 7).Style.Fill.BackgroundColor = rowColor;
                for (int col = 1; col <= 7; col++)
                    ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                alternate = !alternate;
                row++;
            }

            ws.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"products_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private void WritesSummaryRow(IXLWorksheet ws, int row, string label, string value)
        {
            ws.Cell(row, 1).Value = label;
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 2).Value = value;
            ws.Range(row, 1, row, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#f8f9fa");
        }
    }
}
