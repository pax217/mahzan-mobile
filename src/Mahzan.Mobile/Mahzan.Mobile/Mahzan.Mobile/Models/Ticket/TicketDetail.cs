using System;

namespace Mahzan.Mobile.Models.Ticket
{
    public class TicketDetail
    {
        public Guid TicketDetailId { get; set; }
        
        public Guid ProductId { get; set; }
        
        public int Quantity { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}