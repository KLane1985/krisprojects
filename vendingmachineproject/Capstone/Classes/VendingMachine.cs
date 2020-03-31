using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine   ///NO read or writes, only in CLI

    {
        public double currentBalance = 0;
        public double fedMoney = 0;
        public int newItemsRemaining;
        public double costOfItem;
        public double machineBalance;
        public int quartersLeft;
        public int dimesLeft;
        public int nickelsLeft;
        public int penniesLeft;
        public int quarter = 25;
        public int dime = 10;
        public int nickel = 5;
        public int penny = 1;

        public ICollection<VendingMachineItem> ListOfItems
        {
            get
            {
                return vendingDictionary.Values;
            }
        }
        public Dictionary<string, VendingMachineItem> vendingDictionary { get; private set; } = new Dictionary<string, VendingMachineItem>();
        public void ReadTheInventory()
        {
            string directory = Environment.CurrentDirectory;
            string filename = "VendingMachine.txt";

            string fullPath = Path.Combine(directory, filename);
            using (StreamReader sr = new StreamReader(fullPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    //store inventory in memory
                    string[] ar = line.Split("|");
                    //create a vending machine item for each line
                    VendingMachineItem item = new VendingMachineItem(ar[0], ar[1], double.Parse(ar[2]), ar[3]);
                    vendingDictionary.Add(ar[0], item);

                }
            }
        }

        public void FeedMoney(int fedMoney)
        {
            
            currentBalance += fedMoney;
        }

        public string SelectProduct(string itemSelected)
        //this is where we decrement the count
        {
            //currentBalance = currentBalance - costOfItem;
            //machineBalance = fedMoney - costOfItem;

            VendingMachineItem itemImBuying = vendingDictionary[itemSelected];



            if (itemImBuying.NumberLeft <= 0)
            {
                return "These are sold out";
            }
            if (currentBalance <= itemImBuying.Price)
            {
                return "Please deposit more money";
            }
            else
            {
                currentBalance -= itemImBuying.Price;
                machineBalance += itemImBuying.Price;
                itemImBuying.NumberLeft--;
                itemImBuying.NumberSold++;
            }

            //if amountLeft = 0, return error message and returned to purchase menu.
            VendingMachineCLI.PrintToAuditFile($"Date and Time: {DateTime.UtcNow}|Item Slot: {itemImBuying.Slot}|Item Name: {itemImBuying.Name}|Item Price: {itemImBuying.Price.ToString("$0.00")}|Money fed: {fedMoney.ToString("$0.00")}|Current Balance: {currentBalance.ToString("$0.00")}");

            if (itemImBuying.Type.Equals("Chip"))
            {
                return "Crunch Crunch Yum";
            }
            else if (itemImBuying.Type.Equals("Candy"))
            {
                return "Munch Munch Yum";
            }
            else if (itemImBuying.Type.Equals("Drink"))
            {
                return "Glug Glug Yum";
            }
            else if (itemImBuying.Type.Equals("Gum"))
            {
                return "Chew Chew Yum";
            }
            //VendingMachineCLI.PrintToAuditFile($"{DateTime.UtcNow} {itemImBuying.Type} {itemImBuying.Name} {itemImBuying.Price} {fedMoney} {currentBalance}");
            return "";


        }

        public void FinishTransaction()
        {
            currentBalance *= 100;

            quartersLeft = (int)currentBalance / quarter;
            currentBalance -= quartersLeft * 25;
            dimesLeft = (int)currentBalance / dime;
            currentBalance -= dimesLeft * 10;
            nickelsLeft = (int)currentBalance / nickel;
            currentBalance -= nickelsLeft * 5;

            double changeGiven = ((double)quartersLeft * 25 + (double)dimesLeft * 10 + (double)nickelsLeft * 5) / 100;
            VendingMachineCLI.PrintToAuditFile($"Date and Time: {DateTime.UtcNow} | Change Given:  {changeGiven.ToString("c")} | Current Balance: {currentBalance.ToString("c")}");



            //give them quarters until change is less than .25, give them dimes until change is less than .10, give them nickels until chane is less than .05, then give pennies at .01
        }
    }


}
