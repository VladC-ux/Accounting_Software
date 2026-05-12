using Accounting_Software.Data.Entities;
using Accounting_Software.Service_Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting_Software.Service
{
    public class ReceiptPdfService : IReceiptPdfService
    {
        public byte[] GenerateSaleReceipt(TransactionHistory tx, string? buyerName)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(t => t.FontSize(10).FontColor("#1c2434"));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("SALES RECEIPT")
                            .FontSize(20).Bold().FontColor("#2563eb");
                        col.Item().PaddingTop(2).Text("Accounting Software")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e5e7eb");
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(6);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Receipt #").FontColor(Colors.Grey.Medium).FontSize(8);
                                c.Item().Text($"{tx.Id:D6}").Bold();
                            });
                            row.RelativeItem().AlignRight().Column(c =>
                            {
                                c.Item().Text("Date").FontColor(Colors.Grey.Medium).FontSize(8);
                                c.Item().Text(tx.SoldDate.ToString("dd MMM yyyy HH:mm")).Bold();
                            });
                        });

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Store").FontColor(Colors.Grey.Medium).FontSize(8);
                                c.Item().Text(tx.StoreName ?? "—").Bold();
                            });
                            row.RelativeItem().AlignRight().Column(c =>
                            {
                                c.Item().Text("Buyer").FontColor(Colors.Grey.Medium).FontSize(8);
                                c.Item().Text(buyerName ?? $"User #{tx.UserId}").Bold();
                            });
                        });

                        col.Item().PaddingVertical(6).LineHorizontal(1).LineColor("#e5e7eb");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(4);
                                cols.RelativeColumn(1);
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#f9fafb").Padding(6).Text("Product").Bold().FontSize(9);
                                header.Cell().Background("#f9fafb").Padding(6).AlignRight().Text("Qty").Bold().FontSize(9);
                                header.Cell().Background("#f9fafb").Padding(6).AlignRight().Text("Price").Bold().FontSize(9);
                                header.Cell().Background("#f9fafb").Padding(6).AlignRight().Text("Total").Bold().FontSize(9);
                            });

                            table.Cell().Padding(6).Column(c =>
                            {
                                c.Item().Text(tx.ProductName).Bold();
                                if (!string.IsNullOrWhiteSpace(tx.Description))
                                    c.Item().Text(tx.Description!).FontSize(8).FontColor(Colors.Grey.Medium);
                                if (tx.Mass > 0)
                                    c.Item().Text($"{tx.Mass} {tx.unitOfmass}").FontSize(8).FontColor(Colors.Grey.Medium);
                            });
                            table.Cell().Padding(6).AlignRight().Text(tx.Count.ToString());
                            table.Cell().Padding(6).AlignRight().Text(tx.Price.ToString("N2"));
                            table.Cell().Padding(6).AlignRight().Text(tx.Total.ToString("N2")).Bold();
                        });

                        col.Item().PaddingTop(6).LineHorizontal(1).LineColor("#e5e7eb");

                        col.Item().AlignRight().Column(c =>
                        {
                            c.Item().PaddingTop(4).Text(txt =>
                            {
                                txt.Span("TOTAL: ").FontSize(12).Bold();
                                txt.Span(tx.Total.ToString("N2")).FontSize(14).Bold().FontColor("#16a34a");
                            });
                        });
                    });

                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor("#e5e7eb");
                        col.Item().PaddingTop(6).Text("Thank you for your purchase!")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"Generated {DateTime.Now:dd MMM yyyy HH:mm}")
                            .FontSize(7).FontColor(Colors.Grey.Lighten1);
                    });
                });
            });

            return doc.GeneratePdf();
        }
    }
}
