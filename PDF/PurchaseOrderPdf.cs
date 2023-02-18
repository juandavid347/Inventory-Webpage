using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using Inventory.Models;

namespace Inventory.PDF
{
    public class PurchaseOrderPdf
    {
        public static string CreatePurchaseOrderPdf(PurchaseOrder purchaseOrder, 
            IEnumerable<PurchaseItems> purchaseItems, CompanyInfo companyInfo)
        {
            var document = CreateDocument();
            DefineStyles(document);

            var section = document.AddSection();
            
            AddTitle(section);
            AddDateandSO(section, purchaseOrder);
            AddCustomerInfo(section, purchaseOrder);
            CreateTable(section, purchaseItems);
            AddVendorInfo(section, companyInfo);

            var filename = RenderToPdf(document);

            return filename;
        }

        public static Document CreateDocument()
        {
            var document = new Document();
            document.Info.Title = "Purchase Order";
            document.Info.Subject = "Purchase Order Template";
            document.Info.Author = "Juan Elizabeth";
            document.DefaultPageSetup.LeftMargin = "2 cm";
            document.DefaultPageSetup.RightMargin = "2 cm";
            return document;
        }

        public static void DefineStyles(Document document)
        {
            var style = document.Styles["Normal"];
            style = document.Styles.AddStyle("TitleStyle", "Normal");
            style.Font.Size = "28";
            style.Font.Bold = true;
            style.Font.Name = "Liberation Sans";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.LineSpacingRule = LineSpacingRule.OnePtFive;
            style = document.Styles.AddStyle("TextStyle", "Normal");
            style.Font.Size = "12";
            style.Font.Name = "Liberation Serif";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.LineSpacingRule = LineSpacingRule.OnePtFive;
            style = document.Styles.AddStyle("TableStyle", "Normal");
            style.Font.Size = "12";
            style.Font.Name = "Liberation Serif";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        }

        public static void AddTitle(Section section)
        {
            var paragraph = section.AddParagraph("Purchase Order");
            paragraph.Style = "TitleStyle";
        }

        public static void AddDateandSO(Section section, PurchaseOrder purchaseOrder)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Name = "Liberation Serif";
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.LineSpacingRule = LineSpacingRule.OnePtFive;
            paragraph.AddText("Date: " + purchaseOrder.Date.ToString("yyyy/MM/dd") + "\t\n");
            paragraph.AddText("PO No: " + purchaseOrder.PurchaseID.ToString() + "\t\t\n");
        }

        public static void AddCustomerInfo(Section section, PurchaseOrder purchaseOrder)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Name = "Liberation Serif";
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.LineSpacingRule = LineSpacingRule.OnePtFive;
            paragraph.AddText("Request to:\n");
            if (purchaseOrder.Vendor != null)
            {
                paragraph.AddText("Vendor: " + purchaseOrder.Vendor.Name + "\n");
                paragraph.AddText("Address: " + purchaseOrder.Vendor.Address + "\n");
                paragraph.AddText("Phone: " + purchaseOrder.Vendor.PhoneNumber + "\n");
                paragraph.AddText("Email: " + purchaseOrder.Vendor.Email + "");
            }
        }

        public static void CreateTable(Section section, IEnumerable<PurchaseItems> purchaseItems)
        {
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Rows.Alignment = RowAlignment.Center;
            table.AddColumn(Unit.FromInch(0.75));
            table.AddColumn(Unit.FromInch(2.75));
            table.AddColumn(Unit.FromInch(0.94));
            table.AddColumn(Unit.FromInch(1.06));
            table.AddColumn(Unit.FromInch(1.20));
            var tableHeading = table.AddRow();
            tableHeading.Height = "0.675 cm";
            tableHeading.VerticalAlignment = VerticalAlignment.Center;
            tableHeading.Cells[0].AddParagraph("No").Style = "TableStyle";
            tableHeading.Cells[1].AddParagraph("Product").Style = "TableStyle";
            tableHeading.Cells[2].AddParagraph("Quantity").Style = "TableStyle";
            tableHeading.Cells[3].AddParagraph("Price").Style = "TableStyle";
            tableHeading.Cells[4].AddParagraph("Amount").Style = "TableStyle";
            decimal totalAmount = 0;
            for (int i = 0; i < purchaseItems.Count(); i++)
            {
                var dataRow = table.AddRow();
                dataRow.Height = "0.675 cm";
                dataRow.VerticalAlignment = VerticalAlignment.Center;
                dataRow.Cells[0].AddParagraph((i + 1).ToString()).Style = "TableStyle";
                var item = purchaseItems.ElementAt(i);
                if (item.Item != null)
                {
                    dataRow.Cells[1].AddParagraph(item.Item.Name).Style = "TableStyle";
                    dataRow.Cells[2].AddParagraph(item.Quantity.ToString()).Style = "TableStyle";
                    dataRow.Cells[3].AddParagraph(item.Item.Price.ToString("C2")).Style = "TableStyle";
                    var amount = item.Quantity * item.Item.Price;
                    totalAmount += amount;
                    dataRow.Cells[4].AddParagraph(amount.ToString("C2")).Style = "TableStyle";
                }
            }
            var lastCell = table.AddRow();
            lastCell.Height = "0.675 cm";
            lastCell.VerticalAlignment = VerticalAlignment.Center;
            lastCell.Cells[0].MergeRight = 3;
            lastCell.Cells[0].AddParagraph("Total").Style = "TableStyle";
            lastCell.Cells[4].AddParagraph(totalAmount.ToString("C2")).Style = "TableStyle";
        }

        public static void AddVendorInfo(Section section, CompanyInfo companyInfo)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Name = "Liberation Serif";
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.LineSpacingRule = LineSpacingRule.OnePtFive;
            paragraph.AddText("\nIssued by:\n");
            paragraph.AddText("Customer: " + companyInfo.Name + "\n");
            paragraph.AddText("Address: " + companyInfo.Address + "\n");
            paragraph.AddText("Phone: " + companyInfo.PhoneNumber + "\n");
            paragraph.AddText("Email: " + companyInfo.Email + "");
        }

        public static string RenderToPdf(Document document)
        {
            var pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            DateTime now = DateTime.Now;
            var filename = "/PurchaseOrder" + now.ToString("yyyyMMddHHmmss") + ".pdf";
            pdfRenderer.PdfDocument.Save("./wwwroot" + filename);
            return filename;
        }
    }
}