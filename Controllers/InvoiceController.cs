using DinkToPdf;
using DinkToPdf.Contracts;
using InvoiceExport.Model;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;



namespace InvoiceExport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IConverter _converter;

        public InvoiceController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpGet("{invoiceId}")]

        public async Task<IActionResult> GetInvoice(int invoiceId)
        {

            var invoice = new Invoice
            {
                InvoiceId = invoiceId,
                InvoiceDate = DateTime.Now,
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem { Description = "Item 1", Quantity = 2, UnitPrice = 10.00m },
                    new InvoiceItem { Description = "Item 2", Quantity = 1, UnitPrice = 15.00m }
                },

            };
            var html = await RenderInvoiceHtml(invoice);
            var requestbody = new EmailRequestModel { Name = "fgd" ,To="kartik@yopmail.com",Subject="fdg",Message=html};

            var sendmail = await SendEmail(html, requestbody);



            return Ok("");
        }

        private async Task<string> RenderInvoiceHtml(Invoice model)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "InvoiceTemplate.html");
            var template = await System.IO.File.ReadAllTextAsync(templatePath);

            // Replace placeholders with actual data
            template = template.Replace("{{InvoiceId}}", model.InvoiceId.ToString())
                               .Replace("{{InvoiceDate}}", model.InvoiceDate.ToShortDateString())
                               .Replace("{{CustomerName}}", model.CustomerName)
                               .Replace("{{CustomerEmail}}", model.CustomerEmail)
                               .Replace("{{TotalAmount}}", model.TotalAmount.ToString("C"));

            var itemsHtml = "";
            foreach (var item in model.Items)
            {
                itemsHtml += $"<tr><td>{item.Description}</td><td>{item.Quantity}</td><td>{item.UnitPrice:C}</td><td>{item.TotalPrice:C}</td></tr>";
            }
            template = template.Replace("{{Items}}", itemsHtml);

            return template;
        }



        private async Task<string> SendEmail(string html, EmailRequestModel emailRequest)
        {

            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ks93101@gmail.com", "credintials"), // Use a secure method for passwords
                    EnableSsl = true,
                })
                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress("ks93101@gmail.com"),
                    Subject = emailRequest.Subject,
                    Body = html,
                    IsBodyHtml = true,
                })
                {
                    mailMessage.To.Add(emailRequest.To);
                    await smtpClient.SendMailAsync(mailMessage);
                }

                return "Ok";
            }
            catch (Exception ex)
            {
           
                return $"Error: {ex.Message}";
            }

        }

        private string LoadTemplate(string templatePath, EmailRequestModel emailRequest)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var x = template;
            return template;
        }

    }
}




