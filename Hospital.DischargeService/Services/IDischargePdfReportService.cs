using Hospital.DischargeService.Models;

namespace Hospital.DischargeService.Services
{
    public interface IQuestPdfReportService
    {
        byte[] GenerateDischargePdf(DischargeSummary summary);
    }

}
