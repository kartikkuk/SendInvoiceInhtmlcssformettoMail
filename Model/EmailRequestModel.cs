namespace InvoiceExport.Model
{
    public class EmailRequestModel
    {

        
            public string To { get; set; }
            public string Subject { get; set; }
            public string Name { get; set; } // To personalize the email
            public string Message { get; set; } // Additional message content
        
    }
}
