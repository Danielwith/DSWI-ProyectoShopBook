using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class PdfGenerator
    {
        private shopbookEntities db = new shopbookEntities();
        private decimal? total=0;
        public void generate(List<ShoppingCart> compras, string path, ClientData cliente)
        {
            // Must have write permissions to the path folder
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph header = new Paragraph("COMPROBANTE DE PAGO")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20)
               .SetBold();
            document.Add(header);

            Paragraph nombre = new Paragraph("Shopbook S.A")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(14)
               .SetBold();
            document.Add(nombre);

            Paragraph dir1 = new Paragraph("Parque Hernán Velarde, 27 Santa Beatriz")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(dir1);

            Paragraph dir2 = new Paragraph("15046 Lima, Lima")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(dir2);

            // Line separator
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            Paragraph nRecibo = new Paragraph("N° de Recibo: "+db.usp_maxBoleta().FirstOrDefault())
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(nRecibo);

            Paragraph fecha = new Paragraph("Fecha: "+DateTime.Now.ToString())
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(fecha);

            // Line separator
            document.Add(ls);
            Paragraph dataCli = new Paragraph("Info del Cliente:")
               .SetTextAlignment(TextAlignment.LEFT)
               .SetBold()
               .SetUnderline()
               .SetFontSize(14);
            document.Add(dataCli);

            Paragraph email = new Paragraph("Correo Electronico: " + cliente.email)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(email);

            Paragraph nomape = new Paragraph("Nombres y Apellidos: " + cliente.name +" "+cliente.apellido)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(nomape);

            Paragraph docNum = new Paragraph("DNI / RUC: "+cliente.docNum)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(docNum);

            Paragraph phone = new Paragraph("Telefono: " + cliente.phone)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(phone);

            Paragraph direccion = new Paragraph("Dirección: " + cliente.direccion)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(10);
            document.Add(direccion);

            document.Add(ls);

            // New line
            Paragraph newline = new Paragraph(new Text("\n"));

            Paragraph resumen = new Paragraph("Resumen de Compra:")
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetFontSize(14)
                       .SetBold()
                       .SetUnderline();
            document.Add(resumen);

            Table table = new Table(4, true);

            Cell cell11 = new Cell(1, 1)
               .SetBackgroundColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Cantidad"));

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
               .Add(new Paragraph("Importe"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);

            for (int i = 0; i < compras.Count(); i++)
            {
                Cell cell1 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(compras[i].Cantidad.ToString()));
                table.AddCell(cell1);
                Cell cell2 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(compras[i].Libros.tituLibro));
                table.AddCell(cell2);
                Cell cell3 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("S./ " + compras[i].Libros.precUni.ToString()));
                table.AddCell(cell3);
                Cell cell4 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(("S./ " + compras[i].Cantidad * compras[i].Libros.precUni).ToString()));
                table.AddCell(cell4);
                total += compras[i].Cantidad * compras[i].Libros.precUni;
            }

            document.Add(table);

            Paragraph totalCompra = new Paragraph("Total: " + total)
                       .SetTextAlignment(TextAlignment.RIGHT)
                       .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                       .SetFontSize(10);

            document.Add(newline);
            document.Add(totalCompra);
            document.Close();
        }
    }
}