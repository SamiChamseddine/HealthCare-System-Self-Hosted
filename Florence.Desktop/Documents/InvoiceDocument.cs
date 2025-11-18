using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Florence.Desktop.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Florence.Desktop.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly InvoiceDto _invoice;
        private readonly CustomerDto _customer;
        private readonly string _logoPath;
        private static readonly string FlorenceBlue = "#1a3e8c";

        public InvoiceDocument(InvoiceDto invoice, CustomerDto customer)
        {
            _invoice = invoice;
            _customer = customer;
            _logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.png");
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(25);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(HeaderSection);
                page.Content().Element(ContentSection);
                page.Footer().Element(FooterSection);
            });
        }

        private void HeaderSection(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem(2).Column(leftCol =>
                    {
                        leftCol.Spacing(4);

                        if (File.Exists(_logoPath))
                        {
                            var logo = Image.FromFile(_logoPath);
                            leftCol.Item()
                                .Height(100) 
                                .Image(logo)
                                .FitHeight();

                        }

                        leftCol.Item().Text("FLORENCE HEALTHCARE AT HOME")
                            .Bold().FontColor(FlorenceBlue).FontSize(14);
                        leftCol.Item().Text("R.C.: 2049785 / Beirut - MOF n°: 1622790");
                        leftCol.Item().Text("VAT N°: 601-1622790");
                        leftCol.Item().Text("Beirut, Lebanon");
                    });

                    row.RelativeItem(1).AlignRight().Column(rightCol =>
                    {
                        rightCol.Item().Text("INVOICE")
                            .Bold().FontColor(FlorenceBlue).FontSize(20);
                        rightCol.Item().PaddingVertical(3).Border(1).BorderColor(FlorenceBlue).Padding(8).Column(box =>
                        {
                            box.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Invoice #:").SemiBold();
                                r.RelativeItem().AlignRight().Text($"{_invoice.InvoiceNumber:D4}");
                            });
                            box.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Date Created:").SemiBold();
                                r.RelativeItem().AlignRight().Text($"{_invoice.DateCreated:yyyy-MM-dd}");
                            });
                            box.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Due Date:").SemiBold();
                                r.RelativeItem().AlignRight().Text($"{_invoice.DueDate:yyyy-MM-dd}");
                            });
                        });
                    });
                });

                col.Item().PaddingTop(10);
                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });
        }

        private void ContentSection(IContainer container)
        {
            container.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                col.Item().Row(row =>
                {
                    row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(8).Column(left =>
                    {
                        left.Spacing(4);
                        left.Item().Text("Bill To:").Bold().FontColor(FlorenceBlue);

                        left.Item().Text(_customer.Name).FontSize(12);
                        if (!string.IsNullOrWhiteSpace(_customer.Address))
                            left.Item().Text($"Address: {_customer.Address}");
                        if (!string.IsNullOrWhiteSpace(_customer.Phone))
                            left.Item().Text($"Phone: {_customer.Phone}");
                        if (!string.IsNullOrWhiteSpace(_customer.Email))
                            left.Item().Text($"Email: {_customer.Email}");
                    });

                    row.ConstantItem(10);

                    row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(8).Column(right =>
                    {
                        right.Spacing(4);
                        right.Item().Text("Payment Details").Bold();
                        right.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Currency:");
                            r.RelativeItem().AlignRight().Text(_invoice.Currency);
                        });
                        right.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Payment Method:");
                            r.RelativeItem().AlignRight().Text(_invoice.PaymentMethod);
                        });
                    });
                });

                // Items table
                col.Item().Element(ItemsTableSection);

                // Signature / Issued By
                col.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("Issued By: ____________________");
                    r.RelativeItem().Text("Signature: ____________________");
                    r.RelativeItem().AlignRight().Text("Date: ____________________");
                });
            });
        }

        private void ItemsTableSection(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                });

                table.Header(h =>
                {
                    h.Cell().Background(FlorenceBlue).Padding(6).Text("Description").FontColor(Colors.White).SemiBold();
                    h.Cell().Background(FlorenceBlue).Padding(6).Text("Qty").AlignRight().FontColor(Colors.White).SemiBold();
                    h.Cell().Background(FlorenceBlue).Padding(6).Text("Unit Price").AlignRight().FontColor(Colors.White).SemiBold();
                    h.Cell().Background(FlorenceBlue).Padding(6).Text("Total").AlignRight().FontColor(Colors.White).SemiBold();
                });

                foreach (var (item, idx) in _invoice.Items.Select((x, i) => (x, i)))
                {
                    var bg = idx % 2 == 0 ? Colors.White : Colors.Grey.Lighten3; 

                    void AddCell(string text, bool alignRight = false)
                    {
                        table.Cell()
                            .Border(0.5f)
                            .BorderColor(Colors.Grey.Medium)
                            .Background(bg)
                            .Padding(6)
                            .AlignMiddle()
                            .Element(c =>
                            {
                                if (alignRight)
                                    c.AlignRight().Text(text);
                                else
                                    c.Text(text);
                            });
                    }

                    AddCell(item.Description ?? "-");
                    AddCell(item.Quantity.ToString("0.##"), true);
                    AddCell(item.UnitPrice.ToString("C", CurrencyFormat()), true);
                    AddCell(item.Total.ToString("C", CurrencyFormat()), true);
                }

            });
        }

        private void FooterSection(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(8);

                col.Item().Row(row =>
                {
                    row.RelativeItem(2).Column(left =>
                    {
                        left.Item().Text("Amount in words:").Italic().FontColor(Colors.Grey.Darken2);
                        left.Item().Text(ToAmountInWords(_invoice.Total, _invoice.Currency))
                                 .Italic().FontColor(Colors.Grey.Darken1)
                                 .WrapAnywhere();
                    });

                    row.RelativeItem(1).Column(right =>
                    {
                        right.Item().Border(1).BorderColor(Colors.Grey.Medium).Padding(8).Column(box =>
                        {
                            box.Item().Row(r => {
                                r.RelativeItem().Text("Subtotal:");
                                r.RelativeItem().AlignRight().Text(_invoice.Subtotal.ToString("C", CurrencyFormat()));
                            });
                            if (_invoice.DiscountAmount > 0)
                                box.Item().Row(r => {
                                    r.RelativeItem().Text($"Discount ({_invoice.DiscountPercent:F2}%):");
                                    r.RelativeItem().AlignRight().Text("-" + _invoice.DiscountAmount.ToString("C", CurrencyFormat()));
                                });
                            if (_invoice.VatAmount > 0)
                                box.Item().Row(r => {
                                    r.RelativeItem().Text("VAT:");
                                    r.RelativeItem().AlignRight().Text(_invoice.VatAmount.ToString("C", CurrencyFormat()));
                                });

                            box.Item().PaddingTop(4).Row(r => {
                                r.RelativeItem().Text("Total Due:").Bold();
                                r.RelativeItem().AlignRight().Text(_invoice.Total.ToString("C", CurrencyFormat())).Bold().FontColor(FlorenceBlue);
                            });
                        });
                    });
                });

                col.Item().Text("Terms & Conditions").Bold();
                col.Item().Text(text => {
                    text.Span("If services are discontinued for any reason, suspended days will be refunded. ");
                    text.Span("This invoice requires an official receipt as proof of payment. ");
                    text.Span("All sales are final - no refunds or exchanges.");
                });

                col.Item().PaddingTop(10).AlignCenter().Text(txt =>
                {
                    txt.Span("FlorenceHealthCareAtHome@outlook.com | +961 71 385 125").FontColor(Colors.Grey.Medium);
                });

                col.Item().AlignCenter().Text(txt =>
                {
                    txt.Span("Page ").FontColor(Colors.Grey.Medium);
                    txt.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                    txt.Span(" of ").FontColor(Colors.Grey.Medium);
                    txt.TotalPages().FontColor(Colors.Grey.Medium);
                });
            });
        }

        private CultureInfo CurrencyFormat()
        {
            if (string.Equals(_invoice.Currency, "LBP", StringComparison.OrdinalIgnoreCase))
            {
                var ci = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
                ci.NumberFormat.CurrencySymbol = "LBP";
                ci.NumberFormat.CurrencyDecimalDigits = 0;
                return ci;
            }
            return CultureInfo.GetCultureInfo("en-US");
        }

        private static string ToAmountInWords(decimal amount, string currency)
        {
            long whole = (long)Math.Floor(amount);
            int cents = (int)Math.Round((amount - whole) * 100m);
            string currencyWord = currency?.ToUpperInvariant() == "LBP" ? "Lebanese Pounds" : "US Dollars";
            return $"{NumberToWords(whole)} and {cents:00}/100 {currencyWord}";
        }

        private static string NumberToWords(long number)
        {
            if (number == 0) return "Zero";
            if (number < 0) return "Minus " + NumberToWords(Math.Abs(number));

            string[] units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
                               "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen",
                               "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy",
                              "Eighty", "Ninety" };

            string ToWords(long n)
            {
                if (n < 20) return units[n];
                if (n < 100) return tens[n / 10] + (n % 10 > 0 ? " " + units[n % 10] : "");
                if (n < 1000) return units[n / 100] + " Hundred" + (n % 100 > 0 ? " " + ToWords(n % 100) : "");
                if (n < 1_000_000) return ToWords(n / 1000) + " Thousand" + (n % 1000 > 0 ? " " + ToWords(n % 1000) : "");
                if (n < 1_000_000_000) return ToWords(n / 1_000_000) + " Million" + (n % 1_000_000 > 0 ? " " + ToWords(n % 1_000_000) : "");
                return ToWords(n / 1_000_000_000) + " Billion" + (n % 1_000_000_000 > 0 ? " " + ToWords(n % 1_000_000_000) : "");
            }

            return ToWords(number);
        }
    }
}