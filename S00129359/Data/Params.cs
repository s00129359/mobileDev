using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00129359.Data
{
    //sending params from mainpage to journeys
    public class Params
    {
        public int routeId { get; set; }
        public string date { get; set; }
        public string returnDate { get; set; }
        public string ticketType { get; set; }
        public int JourneyId { get; set; }

    }
}
