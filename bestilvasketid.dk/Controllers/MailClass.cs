using System;
using System.Net.Mail;

namespace bestilvasketid.dk.Controllers
{
    public class MailClass
    {
        public void SendEmail(string toMail, DateTime dateTime)
        {
            //SETTINGS FOR GMAIL SSL CONNECTION
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("bestilvasketid@gmail.com", "BestilNu!"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            //CREATES EMAIL (from, to, subject, content)
            MailMessage mail = new MailMessage("bestilvasketid@gmail.com", toMail,
                "BestilVasketid.dk " + dateTime.ToString("HH:mm - dd/MM-yyyy"),
                "Du har bestilt vasketid til kl. " + dateTime.ToString("HH:mm - dd/MM-yyyy"));

            smtpClient.Send(mail);
        }
    }
}