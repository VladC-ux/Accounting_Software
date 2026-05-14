using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting_Software.Service
{
    public class InvoicePdfService : IInvoicePdfService
    {
        private const string AccentColor = "#2563eb";
        private const string MutedColor = "#6b7280";
        private const string BorderColor = "#e5e7eb";
        private const string HeaderBg = "#f3f4f6";
        private const string TotalColor = "#16a34a";
        private const string TextColor = "#1c2434";

        public byte[] Generate(InvoiceViewModel invoice)
        {
            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(t => t.FontSize(10).FontColor(TextColor));

                    page.Header().Element(BuildHeader(invoice));
                    page.Content().Element(BuildContent(invoice));
                    page.Footer().Element(BuildFooter());
                });
            });

            return doc.GeneratePdf();
        }

        private static Action<IContainer> BuildHeader(InvoiceViewModel invoice) => container =>
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("INVOICE").FontSize(28).Bold().FontColor(AccentColor);
                        c.Item().PaddingTop(2).Text("Accounting Software").FontSize(10).FontColor(MutedColor);
                    });

                    row.RelativeItem().AlignRight().Column(c =>
                    {
                        c.Item().Text("Invoice #").FontSize(8).FontColor(MutedColor);
                        c.Item().Text(invoice.Number).FontSize(14).Bold();
                        c.Item().PaddingTop(4).Text("Issued").FontSize(8).FontColor(MutedColor);
                        c.Item().Text(invoice.IssuedAt.ToString("dd MMM yyyy HH:mm")).Bold();
                    });
                });

                col.Item().PaddingTop(10).LineHorizontal(1).LineColor(BorderColor);
            });
        };

        private static Action<IContainer> BuildContent(InvoiceViewModel invoice) => container =>
        {
            container.PaddingVertical(15).Column(col =>
            {
                col.Spacing(10);

                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("FROM").FontSize(8).Bold().FontColor(MutedColor).LetterSpacing(0.05f);
                        c.Item().PaddingTop(2).Text(invoice.StoreName ?? "—").Bold().FontSize(11);
                        if (!string.IsNullOrWhiteSpace(invoice.UserName))
                            c.Item().Text($"Issued by: {invoice.UserName}").FontSize(9).FontColor(MutedColor);
                    });

                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("BILL TO").FontSize(8).Bold().FontColor(MutedColor).LetterSpacing(0.05f);
                        c.Item().PaddingTop(2).Text(invoice.BuyerName ?? "Walk-in customer").Bold().FontSize(11);
                        if (!string.IsNullOrWhiteSpace(invoice.BuyerEmail))
                            c.Item().Text(invoice.BuyerEmail).FontSize(9).FontColor(MutedColor);
                        if (!string.IsNullOrWhiteSpace(invoice.BuyerPhone))
                            c.Item().Text(invoice.BuyerPhone).FontSize(9).FontColor(MutedColor);
                    });
                });

                col.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(25);
                        cols.RelativeColumn(5);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(2);
                        cols.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(HeaderBg).Padding(6).Text("#").Bold().FontSize(9);
                        header.Cell().Background(HeaderBg).Padding(6).Text("Description").Bold().FontSize(9);
                        header.Cell().Background(HeaderBg).Padding(6).AlignRight().Text("Qty").Bold().FontSize(9);
                        header.Cell().Background(HeaderBg).Padding(6).AlignRight().Text("Unit price").Bold().FontSize(9);
                        header.Cell().Background(HeaderBg).Padding(6).AlignRight().Text("Amount").Bold().FontSize(9);
                    });

                    int index = 1;
                    foreach (var item in invoice.Items)
                    {
                        table.Cell().BorderBottom(0.5f).BorderColor(BorderColor).Padding(6).Text(index.ToString()).FontSize(9);

                        table.Cell().BorderBottom(0.5f).BorderColor(BorderColor).Padding(6).Column(c =>
                        {
                            c.Item().Text(item.ProductName).Bold();
                            if (!string.IsNullOrWhiteSpace(item.Description))
                                c.Item().Text(item.Description!).FontSize(8).FontColor(MutedColor);
                        });

                        table.Cell().BorderBottom(0.5f).BorderColor(BorderColor).Padding(6).AlignRight().Text($"{item.Count} {item.UnitOfMass}");
                        table.Cell().BorderBottom(0.5f).BorderColor(BorderColor).Padding(6).AlignRight().Text(item.Price.ToString("N2"));
                        table.Cell().BorderBottom(0.5f).BorderColor(BorderColor).Padding(6).AlignRight().Text(item.Total.ToString("N2")).Bold();

                        index++;
                    }
                });

                col.Item().AlignRight().PaddingTop(8).Width(220).Column(c =>
                {
                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Subtotal").FontColor(MutedColor);
                        r.ConstantItem(100).AlignRight().Text(invoice.Subtotal.ToString("N2"));
                    });

                    if (invoice.TaxRate > 0)
                    {
                        c.Item().PaddingTop(2).Row(r =>
                        {
                            r.RelativeItem().Text($"VAT ({invoice.TaxRate:N2}%)").FontColor(MutedColor);
                            r.ConstantItem(100).AlignRight().Text(invoice.TaxAmount.ToString("N2"));
                        });
                    }

                    c.Item().PaddingTop(6).LineHorizontal(1).LineColor(BorderColor);

                    c.Item().PaddingTop(6).Row(r =>
                    {
                        r.RelativeItem().Text("TOTAL").Bold().FontSize(12);
                        r.ConstantItem(100).AlignRight().Text(invoice.Total.ToString("N2"))
                            .Bold().FontSize(14).FontColor(TotalColor);
                    });
                });
            });
        };

        private static Action<IContainer> BuildFooter() => container =>
        {
            container.Column(col =>
            {
                col.Item().LineHorizontal(1).LineColor(BorderColor);
                col.Item().PaddingTop(6).AlignCenter().Text("Thank you for your business!")
                    .FontSize(9).FontColor(MutedColor);
                col.Item().AlignCenter().Text($"Generated {DateTime.Now:dd MMM yyyy HH:mm}")
                    .FontSize(7).FontColor(Colors.Grey.Lighten1);
            });
        };
    }
}
