namespace InvoiceExport.Model
{
    public class Invoice
    {
       
            public int InvoiceId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public DateTime InvoiceDate { get; set; }
            public List<InvoiceItem> Items { get; set; }
            public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
        }

        public class InvoiceItem
        {
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice => Quantity * UnitPrice;
        }
    }

