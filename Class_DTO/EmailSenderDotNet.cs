using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class EmailSenderDotNet
    {
        public bool sendEmail(ClientData user,string pdfPath)
        {
            MailAddress to = new MailAddress(user.email);
            MailAddress from = new MailAddress("shopbooknoreply@gmail.com");
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = "COMPROBANTE DE PAGO - ShopBook";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("shopbooknoreply@gmail.com", "obakshotouzhufkd"),
                EnableSsl = true
            };
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                $"<h2>Se a realizado con exito su compra.</h2>" +
                $"<hr/>" +
                $"<p>Tipo de documento: Boleta de Venta</p>" +
                $"<p>N° RUC / DNI del cliente: {user.docNum}</p>" +
                $"<p>Cliente: {user.name +" "+user.apellido}</p>" +
                $"<p>Fecha de emision: {DateTime.Now} </p>" +
                $"<hr/>" +
                $"<p>Se envía el comprobante electrónico en adjunto en formato PDF.</p>" +
                $"<strong>SHOPBOOK SA</strong><br/>"
                ,null,"text/html"
                );
            /*
            LinkedResource linkedImage = new LinkedResource(@imgPath);
            linkedImage.ContentId = "Logo"; 
            htmlView.LinkedResources.Add(linkedImage);*/
            msg.AlternateViews.Add(htmlView);

            Attachment attachment = new Attachment(pdfPath, MediaTypeNames.Application.Octet);
            msg.Attachments.Add(attachment);
            
            try
            {
                client.Send(msg);
                return true;
            }
            catch(SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
    }
}