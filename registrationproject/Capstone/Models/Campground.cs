using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundID { get; set; }
        public string Name { get; set; }
        public int ParkID { get; set; }
        public int OpenDate { get; set; }
        public int CloseDate { get; set; }
        public decimal DailyFee { get; set; }
        
        

        
    }   
}
