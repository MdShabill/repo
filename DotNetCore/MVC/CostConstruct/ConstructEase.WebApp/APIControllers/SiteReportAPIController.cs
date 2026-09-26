using ConstructEase.WebApp.DTOs;
using ConstructEase.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;

namespace ConstructEase.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteReportAPIController : ControllerBase
    {
        private readonly SiteReportPdfService _pdfService;

        public SiteReportAPIController(
            SiteReportPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("generate-pdf")]
        public async Task<IActionResult> GeneratePdf(
            [FromBody] GeneratePdfRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Html))
            {
                return BadRequest(
                    "HTML content is required."
                );
            }

            try
            {
                var pdfBytes =
                    await _pdfService.GeneratePdfAsync(
                        request.Html
                    );

                var fileName =
                    string.IsNullOrWhiteSpace(
                        request.FileName)
                        ? "site-report.pdf"
                        : request.FileName;

                if (!fileName.EndsWith(
                        ".pdf",
                        StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".pdf";
                }

                return File(
                    pdfBytes,
                    "application/pdf",
                    fileName
                );
            }
            catch (PlaywrightException ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Playwright PDF generation failed.",
                        error = ex.Message
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "PDF generation failed.",
                        error = ex.Message
                    }
                );
            }
        }
    }
}