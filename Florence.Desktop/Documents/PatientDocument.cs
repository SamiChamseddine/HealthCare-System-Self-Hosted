using System;
using System.IO;
using Florence.Desktop.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Florence.Desktop.Documents
{
    public class PatientDocument : IDocument
    {
        private readonly PatientDto _patient;
        private readonly string _logoPath;
        private static readonly string FlorenceBlue = "#1a3e8c";

        public PatientDocument(PatientDto patient)
        {
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
                            if (File.Exists(_logoPath))
                            {
                                byte[] logoData = File.ReadAllBytes(_logoPath);

                                left.Item()
                                    .Height(100)
                                    .Image(logoData)
                                    .FitHeight();
                            }

                        }

                        left.Item().Text("FLORENCE HEALTHCARE AT HOME")
                            .Bold().FontColor(FlorenceBlue).FontSize(14);
                        left.Item().Text("R.C.: 2049785 / Beirut - MOF n°: 1622790");
                        left.Item().Text("VAT N°: 601-1622790");
                        left.Item().Text("Beirut, Lebanon");
                    });

                    
                    row.RelativeItem(1).AlignRight().Column(right =>
                    {
                        right.Item().Text("PATIENT REPORT")
                            .Bold().FontColor(FlorenceBlue).FontSize(20);

                        right.Item().PaddingVertical(3)
                            .Border(1)
                            .BorderColor(FlorenceBlue)
                            .Padding(8)
                            .Column(box =>
                            {
                                box.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Patient ID:").SemiBold();
                                    r.RelativeItem().AlignRight().Text(_patient.Id.ToString());
                                });

                                box.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Generated:").SemiBold();
                                    r.RelativeItem().AlignRight().Text(DateTime.Now.ToString("yyyy-MM-dd"));
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

                
                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Basic Information").Bold().FontColor(FlorenceBlue);

                        AddRow(section, "Full Name", _patient.FullName);
                        AddRow(section, "Date of Birth", _patient.DateOfBirth.ToString("yyyy-MM-dd"));
                        AddRow(section, "Gender", _patient.Gender.ToString());
                        AddRow(section, "Address", _patient.Address);
                        AddRow(section, "Phone", _patient.PhoneNumber);
                    });

                
                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Emergency Contact").Bold().FontColor(FlorenceBlue);
                        AddRow(section, "Name", _patient.EmergencyContact);
                        AddRow(section, "Relationship", _patient.EmergencyRelationship);
                        AddRow(section, "Phone", _patient.EmergencyPhone);
                    });

                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Insurance").Bold().FontColor(FlorenceBlue);
                        AddRow(section, "Provider", _patient.InsuranceProvider);
                        AddRow(section, "Policy Number", _patient.PolicyNumber);
                    });

                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Medical History").Bold().FontColor(FlorenceBlue);
                        AddRow(section, "Conditions", _patient.MedicalConditionsJson);
                        AddRow(section, "Medications", _patient.CurrentMedications);
                        AddRow(section, "Surgeries", _patient.Surgeries);
                        AddRow(section, "Allergies", _patient.Allergies);
                        AddRow(section, "Family History", _patient.FamilyHistoryJson);
                    });

                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Additional Information").Bold().FontColor(FlorenceBlue);
                        AddRow(section, "Preferred Language", _patient.PreferredLanguage);
                        AddRow(section, "Physician", _patient.PrimaryCarePhysician);
                        AddRow(section, "Vaccinations", _patient.Vaccinations);
                        AddRow(section, "Pain Level", _patient.CurrentPain);
                    });
                col.Item().Border(1).BorderColor(Colors.Grey.Medium)
                    .Padding(10)
                    .Column(section =>
                    {
                        section.Item().Text("Record Metadata").Bold().FontColor(FlorenceBlue);
                        AddRow(section, "Created By", _patient.CreatedBy);
                        AddRow(section, "Created At", _patient.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
                        AddRow(section, "Updated At", _patient.UpdatedAt.ToString("yyyy-MM-dd HH:mm"));
                    });

                col.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("Issued By: ____________________");
                    r.RelativeItem().Text("Signature: ____________________");
                    r.RelativeItem().AlignRight().Text("Date: ____________________");
                });
            });
        }

        private void AddRow(ColumnDescriptor col, string label, string value)
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text(label + ":").SemiBold();
                row.RelativeItem(3).AlignRight().Text(value ?? "");
            });
        }
        private void FooterSection(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(8);

                col.Item().Text("Terms & Conditions").Bold();
                col.Item().Text(text =>
                {
                    text.Span("This report is confidential and intended for medical purposes only. ");
                    text.Span("Distribution without authorization is prohibited.");
                });

                col.Item().PaddingTop(10)
                    .AlignCenter()
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
