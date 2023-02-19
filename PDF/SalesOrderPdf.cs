// Create a Pdf Document given the specified sale order information

using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using Inventory.Models;

namespace Inventory.PDF
{
    public class SalesOrderPdf
    {
        // Create Pdf Document and return a unique filename
        public static string CreateSalesOrderPdf(SaleOrder saleOrder, 
            IEnumerable<SaleItems> saleItems, CompanyInfo companyInfo)
        {
            var document = CreateDocument();
            DefineStyles(document);

            var section = document.AddSection();
            
            AddTitle(section);
            AddDateandSO(section, saleOrder);
            AddCustomerInfo(section, saleOrder);
            CreateTable(section, saleItems);
            AddVendorInfo(section, companyInfo);

            var filename = RenderToPdf(document);

            return filename;
        }

        // Set the document information
        public static Document CreateDocument()
        {
            var document = new Document();
            document.Info.Title = "Sales Order";
            document.Info.Subject = "Sales Order Template";
            document.Info.Author = "Juan Elizabeth";
            document.DefaultPageSetup.LeftMargin = "2 cm";
            document.DefaultPageSetup.RightMargin = "2 cm";
            return document;
        }

        // Easy the document creation using customized styles
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
            var paragraph = section.AddParagraph("Sales Order");
            paragraph.Style = "TitleStyle";
        }

        public static void AddDateandSO(Section section, SaleOrder saleOrder)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Name = "Liberation Serif";
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.LineSpacingRule = LineSpacingRule.OnePtFive;
            paragraph.AddText("Date: " + saleOrder.Date.ToString("yyyy/MM/dd") + "\t\n");
            paragraph.AddText("SO No: " + saleOrder.SaleID.ToString() + "\t\t\n");
        }

        public static void AddCustomerInfo(Section section, SaleOrder saleOrder)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Name = "Liberation Serif";
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.LineSpacingRule = LineSpacingRule.OnePtFive;
            paragraph.AddText("Bill to:\n");
            if (saleOrder.Customer != null)
            {
                paragraph.AddText("Customer: " + saleOrder.Customer.Name + "\n");
                paragraph.AddText("Address: " + saleOrder.Customer.Address + "\n");
                paragraph.AddText("Phone: " + saleOrder.Customer.PhoneNumber + "\n");
                paragraph.AddText("Email: " + saleOrder.Customer.Email + "");
            }
        }

        public static void CreateTable(Section section, IEnumerable<SaleItems> saleItems)
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
            for (int i = 0; i < saleItems.Count(); i++)
            {
                var dataRow = table.AddRow();
                dataRow.Height = "0.675 cm";
                dataRow.VerticalAlignment = VerticalAlignment.Center;
                dataRow.Cells[0].AddParagraph((i + 1).ToString()).Style = "TableStyle";
                var item = saleItems.ElementAt(i);
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
            paragraph.AddText("Vendor: " + companyInfo.Name + "\n");
            paragraph.AddText("Address: " + companyInfo.Address + "\n");
            paragraph.AddText("Phone: " + companyInfo.PhoneNumber + "\n");
            paragraph.AddText("Email: " + companyInfo.Email + "");
        }

        // Render document and create unique filename based on current date and time
        public static string RenderToPdf(Document document)
        {
            var pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            DateTime now = DateTime.Now;
            var filename = "/SalesOrder" + now.ToString("yyyyMMddHHmmss") + ".pdf";
            pdfRenderer.PdfDocument.Save("./wwwroot" + filename);
            return filename;
        }
    }
}