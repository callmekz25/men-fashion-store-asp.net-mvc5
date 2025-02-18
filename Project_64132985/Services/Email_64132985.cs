using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;



namespace Project_64132985.Services
{
    public class Email_64132985
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                var From = "callmevinh2511@gmail.com";
                var Password = "ijcfxnhisnxaewuo";
                mail.From = new System.Net.Mail.MailAddress(From);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(From, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
            }
        }
    }
}