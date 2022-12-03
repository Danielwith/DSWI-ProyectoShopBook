using iText.Kernel.Colors;
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

        public void generate(string path, UsuarioBoletaDTO data, List<DtBoletaLibroDTO> detalle)
        {
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph header = new Paragraph("REPORTE DE PAGO - " + data.nroBoleta)
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

            Paragraph email = new Paragraph("Correo Electronico: " + data.email)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(email);

            Paragraph nomape = new Paragraph("Nombres y Apellidos: " + data.nombre.Trim() + " " + data.apellido.Trim())
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(nomape);

            Paragraph docNum = new Paragraph("DNI / RUC: " + data.dni)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(docNum);

            Paragraph phone = new Paragraph("Telefono: " + data.telefono)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(phone);

            Paragraph direccion = new Paragraph("Dirección: " + data.direccion)
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

            Table table = new Table(5, true);

            Cell cell11 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("ID Libro"));

            Cell cell12 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Titulo"));

            Cell cell13 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Precio Unitario"));

            Cell cell14 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Cantidad"));

            Cell cell15 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Importe"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);
            table.AddCell(cell15);

            for (int i = 0; i < detalle.Count(); i++)
            {
                Cell cell1 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(detalle[i].idLibro.ToString()));
                table.AddCell(cell1);
                Cell cell2 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(detalle[i].tituLibro));
                table.AddCell(cell2);
                Cell cell3 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("S./ " + detalle[i].precUni.ToString()));
                table.AddCell(cell3);
                Cell cell4 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("S./ " + detalle[i].cantidad.ToString()));
                table.AddCell(cell4);
                Cell cell5 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(("S./ " + detalle[i].cantidad * detalle[i].precUni).ToString()));
                table.AddCell(cell5);
            }
            document.Add(table);

            document.Close();
        }
    }
}