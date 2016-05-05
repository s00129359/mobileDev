using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00129359.Data
{
    public class Journey
    {
        public string id { get; set; }
        public int Journey_id { get; set; }
        public int Route_id { get; set; }
        //didnt use date time
        // times will be displayed as
        // 15:00 or 16:30
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}
