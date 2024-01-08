using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoS_Inventory
{
    public class PdfHeaderFooter : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable headerTbl = new PdfPTable(1);
            headerTbl.TotalWidth = 300;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(new Phrase("Receipt Mo!"));
            cell.Border = 0;
            headerTbl.AddCell(cell);
            headerTbl.WriteSelectedRows(0, -1, document.Left, document.PageSize.Height - 10, writer.DirectContent);

            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = 300;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell = new PdfPCell(new Phrase("Thank You. THIS SERVES AS YOUR OFFICIAL RECEIPT"));
            cell.Border = 0;
            footerTbl.AddCell(cell);
            footerTbl.WriteSelectedRows(0, -1, document.Left, document.Bottom, writer.DirectContent);
        }
    }
}