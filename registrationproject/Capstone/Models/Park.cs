using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }

        public string Description { get; set; }

      /*  public override string ToString()
        {
            return ParkId.ToString().PadRight(3) + Name.PadRight(20) + Location.PadRight(8) + EstablishDate.Year.ToString().PadRight(8) + Area.ToString().PadRight(5) + Visitors.ToString().PadRight(10) + Description.PadRight(30);
        }*/
    }
}
