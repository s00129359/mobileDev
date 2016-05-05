using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00129359.Data
{
    public class Ticket
    {
        public string id { get; set; }
        public int custId { get; set; }
        public int ticketId { get; set; }
        public int RouteId { get; set; }
        public int JourneyId { get; set; }
        public string TicketType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime FirstUseDate { get; set; }
        public bool FirstJourneyUsed { get; set; }
        //only if return ticket this applies
        public bool SecondJourneyUsed { get; set; }
    }
}
