using Hospital.DischargeService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Hospital.DischargeService.Services
{
    public class QuestPdfReportService : IQuestPdfReportService
    {
        private readonly ILogger<QuestPdfReportService> _logger;

        static QuestPdfReportService() => QuestPDF.Settings.License = LicenseType.Community;

        public QuestPdfReportService(ILogger<QuestPdfReportService> logger) => _logger = logger;

        public byte[] GenerateDischargePdf(DischargeSummary s)
        {
            try
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        // ── Page setup ─────────────────────────────────────────
                        page.Size(PageSizes.A4);
                        page.Margin(40);
                        page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                        // ── Header ─────────────────────────────────────────────
                        page.Header().Element(ComposeHeader);

                        // ── Content ────────────────────────────────────────────
                        page.Content().Element(content => ComposeContent(content, s));

                        // ── Footer ─────────────────────────────────────────────
                        page.Footer().Element(ComposeFooter);
                    });
                });

                _logger.LogInformation("PDF generated for patient {PatientName} (discharge {Id})", s.PatientName, s.Id);
                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF generation failed for discharge {Id}", s.Id);
                throw;
            }
        }

        // ─── Header ─────────────────────────────────────────────────────────────
        private static void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item()
                             .Text("🏥 Hospital Management System")
                             .Bold().FontSize(18).FontColor("#1F4E79");

                        inner.Item()
                             .Text("Discharge Summary Report")
                             .FontSize(12).FontColor("#2E75B6");
                    });

                    row.ConstantItem(130).AlignRight().Column(inner =>
                    {
                        inner.Item().Text($"Date: {DateTime.Now:dd MMM yyyy}").FontSize(9).FontColor("#666666");
                        inner.Item().Text($"Ref: {DateTime.Now:yyyyMMddHHmm}").FontSize(9).FontColor("#666666");
                    });
                });

                col.Item().PaddingTop(4)
                   .BorderBottom(2).BorderColor("#2E75B6")
                   .Height(1);

                col.Item().Height(8);
            });
        }

        // ─── Content ────────────────────────────────────────────────────────────
        private static void ComposeContent(IContainer container, DischargeSummary s)
        {
            container.Column(col =>
            {
                col.Spacing(10);

                // ── Patient Information ─────────────────────────────────────────
                col.Item().Element(e => SectionTitle(e, "Patient Information"));
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(2);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(2);
                    });

                    InfoRow(table, "Patient Name", s.PatientName ?? "—",
                                   "Patient ID", s.PatientId.ToString()[..8] + "...");

                    InfoRow(table, "Age", $"{s.PatientAge} years",
                                   "Gender", s.PatientGender ?? "Not Specified");

                    InfoRow(table, "Admitted", s.AdmittedOn.HasValue
                                           ? s.AdmittedOn.Value.ToString("dd MMM yyyy HH:mm")
                                           : "Not recorded",
                                   "Discharged", s.DischargedOn.ToString("dd MMM yyyy HH:mm"));

                    InfoRow(table, "Discharging Dr", s.DischargingDoctorId.ToString()[..8] + "...",
                                   "Report ID", s.Id.ToString()[..8] + "...");
                });

                // ── Clinical Summary ────────────────────────────────────────────
                col.Item().Element(e => SectionTitle(e, "Clinical Summary"));
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(130);
                        cols.RelativeColumn();
                    });

                    LabelCell(table, "Diagnosis");
                    ValueCell(table, s.Diagnosis);

                    LabelCell(table, "Treatment");
                    ValueCell(table, s.Treatment);

                    LabelCell(table, "Medications");
                    ValueCell(table, string.IsNullOrWhiteSpace(s.Medications) ? "None prescribed" : s.Medications);

                    LabelCell(table, "Follow-Up");
                    ValueCell(table, string.IsNullOrWhiteSpace(s.FollowUpInstructions) ? "As needed" : s.FollowUpInstructions);
                });

                // ── AI Diet Recommendation ──────────────────────────────────────
                col.Item().Element(e => SectionTitle(e, "AI Diet Recommendation (Powered by Google Gemini)"));
                col.Item()
                   .Background("#F0F6FF")
                   .Border(0.5f).BorderColor("#BDD7EE")
                   .Padding(12)
                   .Column(inner =>
                   {
                       inner.Item()
                            .Text("✨ Personalised Dietary Guidance")
                            .Bold().FontSize(10).FontColor("#1F4E79");

                       inner.Item().Height(4);

                       inner.Item()
                            .Text(string.IsNullOrWhiteSpace(s.AIDietRecommendation)
                                ? "No diet recommendation generated."
                                : s.AIDietRecommendation)
                            .FontSize(10).FontColor("#333333");
                   });

                // ── Important Notes ─────────────────────────────────────────────
                col.Item().Element(e => SectionTitle(e, "Important Notes"));
                col.Item()
                   .Background("#FFF8E1")
                   .Border(0.5f).BorderColor("#FFD54F")
                   .Padding(10)
                   .Text(text =>
                   {
                       text.Span("⚠ ").Bold().FontColor("#E65100");
                       text.Span("This discharge summary is generated automatically by the Hospital Management System. " +
                                 "Please consult your treating physician for any medical queries. " +
                                 "In case of emergency, call your nearest hospital or emergency services immediately.")
                           .FontSize(9).FontColor("#555555");
                   });

                // ── Signature block ─────────────────────────────────────────────
                col.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item().BorderBottom(1).BorderColor("#CCCCCC").Width(160).Height(30);
                        inner.Item().Text("Patient / Guardian Signature").FontSize(8).FontColor("#777");
                    });

                    row.ConstantItem(60);

                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item().BorderBottom(1).BorderColor("#CCCCCC").Width(160).Height(30);
                        inner.Item().Text("Authorised By (Doctor)").FontSize(8).FontColor("#777");
                    });
                });
            });
        }

        // ─── Footer ─────────────────────────────────────────────────────────────
        private static void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().BorderTop(0.5f).BorderColor("#CCCCCC").Height(1);
                col.Item().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem()
                       .Text("CONFIDENTIAL — Hospital Management System")
                       .FontSize(8).FontColor("#999999");

                    row.ConstantItem(100).AlignRight()
                       .Text(x =>
                       {
                           x.Span("Page ").FontSize(8).FontColor("#999999");
                           x.CurrentPageNumber().FontSize(8).FontColor("#999999");
                           x.Span(" of ").FontSize(8).FontColor("#999999");
                           x.TotalPages().FontSize(8).FontColor("#999999");
                       });
                });
            });
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────
        private static void SectionTitle(IContainer c, string title) =>
            c.Background("#1F4E79")
             .Padding(6)
             .Text(title)
             .Bold().FontSize(10).FontColor("#FFFFFF");

        private static void LabelCell(TableDescriptor t, string label) =>
            t.Cell().Background("#DEEAF1").Padding(6)
             .Text(label).Bold().FontSize(9).FontColor("#1F4E79");

        private static void ValueCell(TableDescriptor t, string value) =>
            t.Cell().BorderBottom(0.5f).BorderColor("#E0E0E0").Padding(6)
             .Text(value).FontSize(9).FontColor("#333333");

        private static void InfoRow(TableDescriptor t,
            string lbl1, string val1, string lbl2, string val2)
        {
            t.Cell().Background("#DEEAF1").Padding(5)
             .Text(lbl1).Bold().FontSize(9).FontColor("#1F4E79");
            t.Cell().BorderBottom(0.5f).BorderColor("#E0E0E0").Padding(5)
             .Text(val1).FontSize(9);
            t.Cell().Background("#DEEAF1").Padding(5)
             .Text(lbl2).Bold().FontSize(9).FontColor("#1F4E79");
            t.Cell().BorderBottom(0.5f).BorderColor("#E0E0E0").Padding(5)
             .Text(val2).FontSize(9);
        }
    }

}