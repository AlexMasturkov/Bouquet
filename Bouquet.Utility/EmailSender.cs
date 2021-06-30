using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Bouquet.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions emailOptions;
        public EmailSender(IOptions<EmailOptions> options)
        {
            emailOptions = options.Value;
        }

      
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(emailOptions.MailJetKey, emailOptions.MailJetAuth)
            {
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
           .Property(Send.FromEmail, "amastryukov@myseneca.ca")
           .Property(Send.FromName, "Alexey")
           .Property(Send.Subject, subject)          
           .Property(Send.HtmlPart, htmlMessage)
           .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
               });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
     /*   private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("admin@bouquet.ca", "Bouquet");
            var to = new EmailAddress(email, "End User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            return client.SendEmailAsync(msg);
        }*/

      
    

    /*  public Task SendEmailAsync(string email, string subject, string htmlMessage)
      {
          return Execute(emailOptions.SendGridKey, subject, htmlMessage, email);
      }
      private  Task Execute(string sendGridKey,string subject,string message,string email)
      {
          var client = new SendGridClient(sendGridKey);
          var from = new EmailAddress("admin@bouquet.ca", "Bouquet");           
          var to = new EmailAddress(email, "End User");    
          var msg = MailHelper.CreateSingleEmail(from, to, subject, "",message);
          return client.SendEmailAsync(msg);
      }*/
}

