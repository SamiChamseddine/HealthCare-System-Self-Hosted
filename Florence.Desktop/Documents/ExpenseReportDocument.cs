using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Florence.Desktop.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace Florence.Desktop.Documents
{
    public class ExpenseReportDocument : IDocument
    {
        private readonly ExpenseReportDto _report;
        private readonly PatientDtoFull _patient;
        private readonly string _logoPath;

        private static readonly string FlorenceBlue = "#1a3e8c";

        public ExpenseReportDocument(ExpenseReportDto report, PatientDtoFull patient)
        {
            _report = report;
            _patient = patient;

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
                    row.RelativeItem(2).Column(left =>
                    {
                        left.Spacing(4);

                        if (File.Exists(_logoPath))
                        {
                            var logo = Image.FromFile(_logoPath);

                            left.Item()
                                .Height(100)
                                .Image(logo)
                                .FitHeight();
                        }


                        left.Item().Text("FLORENCE HEALTHCARE AT HOME")
                            .Bold().FontSize(14).FontColor(FlorenceBlue);

                        left.Item().Text("R.C.: 2049785 / Beirut - MOF n°: 1622790");
                        left.Item().Text("VAT N°: 601-1622790");
                        left.Item().Text("Beirut, Lebanon");
                    });


                    row.RelativeItem(1).AlignRight().Column(right =>
                    {
                        right.Item().Text("EXPENSE REPORT")
                            .Bold().FontColor(FlorenceBlue).FontSize(20);

                        right.Item().PaddingVertical(3)
                            .Border(1).BorderColor(FlorenceBlue)
                            .Padding(8)
                            .Column(box =>
                            {
                                box.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Report #:").SemiBold();
                                    r.RelativeItem().AlignRight().Text(_report.Id.ToString());
                                });

                                box.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Created:").SemiBold();
                                    r.RelativeItem().AlignRight().Text(_report.CreatedAt.ToString("yyyy-MM-dd"));
                                });

                                box.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Period:").SemiBold();
                                    r.RelativeItem().AlignRight()
                                        .Text($"{_report.StartDate:yyyy-MM-dd} → {_report.EndDate:yyyy-MM-dd}");
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

                // Patient info
                col.Item().Border(1).BorderColor(Colors.Grey.Medium).Padding(8).Column(bill =>
                {
                    bill.Spacing(4);
                    bill.Item().Text("Patient Information").Bold().FontColor(FlorenceBlue);
                    bill.Item().Text(_patient.FullName).FontSize(12);

                    bill.Item().Text($"Phone: {_patient.PhoneNumber}");
                    bill.Item().Text($"DOB: {_patient.DateOfBirth:d}");
                    bill.Item().Text($"Gender: {_patient.Gender}");
                });

                col.Item().Element(ItemsTableSection);

                col.Item().AlignRight().Element(TotalsSection);
            });
        }


        private void ItemsTableSection(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(4);
                    cols.RelativeColumn(1);
                    cols.RelativeColumn(2);
                });

                table.Header(h =>
                {
                    void Head(string t) =>
                        h.Cell()
                         .Background(FlorenceBlue)
                         .Padding(6)
                         .Text(t).FontColor(Colors.White).SemiBold();

                    Head("Nurse");
                    Head("Date");
                    Head("Description");
                    Head("Hours");
                    Head("Amount");
                });

                foreach (var (item, index) in _report.Items.Select((x, i) => (x, i)))
                {
                    var bg = index % 2 == 0 ? Colors.White : Colors.Grey.Lighten3;

                    void Cell(string v, bool alignRight = false)
                    {
                        table.Cell()
                            .Background(bg)
                            .Border(0.5f)
                            .BorderColor(Colors.Grey.Medium)
                            .Padding(6)
                            .AlignMiddle()
                            .Element(c =>
                            {
                                if (alignRight)
                                    c.AlignRight().Text(v);
                                else
                                    c.Text(v);
                            });
                    }

                    Cell(item.NurseName ?? "N/A");
                    Cell(item.Date.ToShortDateString());
                    Cell(item.Description ?? "");
                    Cell(item.Hours.ToString("0.##"), true);
                    Cell(item.Amount.ToString("C2", CultureInfo.CurrentCulture), true);
                }
            });
        }


        private void TotalsSection(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(8).Column(col =>
            {
                col.Spacing(4);

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Total Hours:");
                    r.RelativeItem().AlignRight().Text(_report.TotalHours.ToString("0.##"));
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Total Amount:").Bold();
                    r.RelativeItem().AlignRight()
                        .Text(_report.TotalAmount.ToString("C2", CultureInfo.CurrentCulture))
                        .Bold().FontColor(FlorenceBlue);
                });
            });
        }

        private void FooterSection(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(8);

                col.Item().AlignCenter()
                    .Text("email | phone number")
                    .FontColor(Colors.Grey.Medium);

                col.Item().AlignCenter().Text(txt =>
                {
                    txt.Span("Page ").FontColor(Colors.Grey.Medium);
                    txt.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                    txt.Span(" of ").FontColor(Colors.Grey.Medium);
                    txt.TotalPages().FontColor(Colors.Grey.Medium);
                });
            });
        }
    }
}
