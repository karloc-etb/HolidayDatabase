using System;

namespace HolidayDatabase
{
    class Holiday
    {
        public int HolidayNo { get; set; }
        public string Destination { get; set; }
        public decimal Cost { get; set; }
        public DateTime DepartureDate { get; set; }
        public int NoOfDays { get; set; }
        public bool Available { get; set; }

        public Holiday()
        {
            
        }
    }
}
