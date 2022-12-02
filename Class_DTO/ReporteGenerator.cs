using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class ReporteGenerator
    {
        private shopbookEntities db = new shopbookEntities();

        public void generate(string path)
        {
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph header = new Paragraph("REPORTE DE PAGO - data.nroBoleta")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20)
               .SetBold();
            document.Add(header);

            Paragraph fecha = new Paragraph("Reporte generado a las " + DateTime.Now.ToString())
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(fecha);

            // Line separator
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            Paragraph dataCli = new Paragraph("Info del Cliente:")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetBold()
               .SetUnderline()
               .SetFontSize(14);
            document.Add(dataCli);

            Paragraph email = new Paragraph("Correo Electronico: " + "cliente.email")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(email);

            Paragraph nomape = new Paragraph("Nombres y Apellidos: " + "cliente.name" + " " + "cliente.apellido")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(nomape);

            Paragraph docNum = new Paragraph("DNI / RUC: " + "cliente.docNum")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(docNum);

            Paragraph phone = new Paragraph("Telefono: " + "cliente.phone")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(phone);

            Paragraph direccion = new Paragraph("Dirección: " + "cliente.direccion")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(direccion);

            // New line
            Paragraph newline = new Paragraph(new Text("\n"));
            document.Add(newline);

            Paragraph resumen = new Paragraph("Compra realizada:")
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetFontSize(14)
                       .SetBold()
                       .SetUnderline();
            document.Add(resumen);

            document.Close();
        }
    }
}