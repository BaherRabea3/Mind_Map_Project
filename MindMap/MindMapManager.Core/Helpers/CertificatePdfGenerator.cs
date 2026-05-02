using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace MindMapManager.Core.Helpers
{
    public static class CertificatePdfGenerator
    {
        public static void Generate(
            string filePath,
            string userName,
            string roadmapName,
            DateTime issuedAt,
            string certificateCode)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(16));

                    page.Content().Column(col =>
                    {
                        col.Spacing(20);

                        col.Item().AlignCenter()
                            .Text("MindRoad")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        col.Item().AlignCenter()
                            .Text("Certificate of Completion")
                            .FontSize(32)
                            .Bold();

                        col.Item().AlignCenter().Text("This certifies that");

                        col.Item().AlignCenter()
                            .Text(userName)
                            .FontSize(26)
                            .Bold();

                        col.Item().AlignCenter().Text("has successfully completed");

                        col.Item().AlignCenter()
                            .Text(roadmapName)
                            .FontSize(22)
                            .Bold();

                        col.Item().AlignCenter()
                            .Text($"Issued on {issuedAt:dd MMM yyyy}");


                        col.Item().PaddingTop(20)
                            .AlignCenter()
                            .Text("Issued by MindRoad – Your Learning Journey")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(10)
                            .AlignCenter()
                            .Text($"Certificate Code: {certificateCode}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        col.Item().AlignCenter()
                            .Text("Verify this certificate at: https://mindroad.runasp.net//verify")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                    });
                });
            }).GeneratePdf(filePath);
        }
    }
}

