using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Controllers
{
    [AllowAnonymous]
    public class ApplyController : Controller
    {
        // GET: Apply
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Message(ApplyForm form)
        {
            SendMail(form);
            return RedirectToAction("AfterMessage");
            //return Content(form.AdSoyad+"-"+form.Email + "-" +form.Mesaj + "-" +form.Nomre + "-" +form.Tehsil + "-" +form.Xidmetlerimiz);
        }
        public ActionResult AfterMessage()
        {
            return View();
        }
        private void SendMail(ApplyForm form)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("marmaraeduaz@gmail.com", "4sl4v4zg3cm3"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("marmaraeduaz@gmail.com");
            mailMessage.To.Add("marmaraeduaz@gmail.com");
            mailMessage.Subject = "Yeni müraciət formu";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Salam! Saytdan yeni müraciət formu göndərilib.  <br/><br/> <hr/> " +
                "Mesajı göndərən: " + form.AdSoyad + "  <br/><br/>" +
                "Təhsili: " + form.Tehsil + "  <br/><br/> " +
                "İstədiyi ixtisas: " + form.Xidmetlerimiz + "  <br/><br/>" +
                "Nömrəsi: " + form.Nomre + "  <br/><br/> " +
                "Emaili: " + form.Email + "  <br/><br/> " +
                "Mesajı: " + form.Mesaj + " <br/><br/> ";

            client.Send(mailMessage);

        }

    }
}