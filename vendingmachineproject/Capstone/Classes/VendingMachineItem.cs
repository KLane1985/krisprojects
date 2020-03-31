using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineItem
    {

        //List<string> ListOfItems = new List<string>();

        public double Price { get; }

        public string Slot { get;  }

        public string Type { get;  }
        
        public string Name { get;  }

        public int NumberLeft { get; set; } = 5;  //eventually we need to modify the number left when product is purchased

        public int NumberSold { get; set; } = 0;

        public VendingMachineItem(string slot, string name, double price, string type)
        {
            Slot = slot;
            Price = price;
            Type = type;
            Name = name;
        }







    }
}
