namespace ConstructEase.WebApp.DTOs
{
    public class GeneratePdfRequest
    {
        public string Html { get; set; } = string.Empty;

        public string FileName { get; set; } = "site-report.pdf";
    }
}