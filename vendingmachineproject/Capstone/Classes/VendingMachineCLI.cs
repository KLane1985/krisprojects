using System;
using System.IO;

namespace Capstone.Classes
{

    class VendingMachineCLI
    {

        private VendingMachine vendMach = new VendingMachine();
        
        public void StartTheMachine()
        {
            vendMach.ReadTheInventory();

            string userChoice = "";
          
                Console.WriteLine("Welcome to the Vend-o-Matic 5000!");
            while (userChoice != "3")
            {
               
                Console.WriteLine("Here are your menu options:");
                Console.WriteLine("1. Display Items");
                Console.WriteLine("2. Purchase Items");
                Console.WriteLine("3. Exit");



                userChoice = Console.ReadLine();
                Console.Clear();

                if (userChoice == "1")
                {
                    DisplayItemList();
                }
                else if (userChoice == "2")
                {
                    DisplayPurchaseMenu();
                }
                else if (userChoice == "4")
                {
                    GenerateSalesReport();
                }
            }
        }

        public void DisplayItemList()
        {
            Console.WriteLine($"{"Slot",-5} {"Item", -20} {"Price", -10}{"Number Left", -10}");
            Console.WriteLine("--------------------------------------------------");
            foreach (VendingMachineItem item in vendMach.ListOfItems)
            {
                if (item.NumberLeft <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("SOLD OUT");
                    Console.WriteLine();

                }
                else
                {
                    
                    Console.WriteLine($"{item.Slot,-5}{item.Name,-20}  {item.Price.ToString("$0.00"), -10}{item.NumberLeft, 5} ");
                    Console.WriteLine();
                }

            }
        }

        public void DisplayPurchaseMenu()
        {
            string itemSelected = "";
            string userChoice = "";
            while (userChoice != "3") //Finish gets you out of this menu
            {
                Console.WriteLine("1) Feed Money");
                Console.WriteLine("2) Select product");
                Console.WriteLine("3) Finish Transaction");
                Console.WriteLine();



                userChoice = Console.ReadLine();
                Console.Clear();
                if (userChoice == "1")
                {
                    int fedMoney = 0;
                    bool success = false;
                    while (!success || fedMoney<0)
                    { 
                        Console.WriteLine("How much money would you like to deposit? (Whole numbers only):");

                        string strfedMoney = Console.ReadLine();
                    
                        success = int.TryParse(strfedMoney, out fedMoney);
                        
                    }

                    while(!success || fedMoney>50)
                    {
                        Console.WriteLine("You may only add a maximum of $50 at a time");

                        string strfedMoney = Console.ReadLine();

                        success = int.TryParse(strfedMoney, out fedMoney);
                    }
                    

                    vendMach.FeedMoney(fedMoney);
                    Console.Clear();

                    Console.WriteLine($"This is your current balance: {vendMach.currentBalance.ToString("c")} ");
                }
                else if (userChoice == "2")
                {

                    if (vendMach.currentBalance <= 0)
                    {
                        Console.WriteLine("Error: You must deposit money before making a selection");

                    }

                    else
                    {
                        DisplayItemList();
                        Console.WriteLine("Please choose an item: ");
                        itemSelected = Console.ReadLine();
                        Console.Clear();
                        //vendMach.SelectProduct(itemSelected);

                        try
                        {
                            Console.WriteLine(vendMach.SelectProduct(itemSelected.ToUpper()));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Not a valid item, please choose again:");
                            Console.WriteLine();
                            Console.WriteLine(e.Message);
                            DisplayItemList();
                        }
                        Console.WriteLine($"Your current balance is: {vendMach.currentBalance.ToString("c")}");

                    }

                }
            }


            vendMach.FinishTransaction();
            Console.WriteLine($"Your current balance is: {vendMach.currentBalance.ToString("c")}");
            Console.WriteLine($"You receive {vendMach.quartersLeft} quarters");
            Console.WriteLine($"You receive {vendMach.dimesLeft} dimes");
            Console.WriteLine($"You receive {vendMach.nickelsLeft} nickels");
            Console.WriteLine($"You receive {vendMach.penniesLeft} pennies");
            Console.WriteLine();

        }

        public void GenerateSalesReport()
        {
            string directory2 = Environment.CurrentDirectory;
            string fileName2 = "SalesReport.txt";
            string fullPath2 = Path.Combine(directory2, fileName2);

            using (StreamWriter sw = new StreamWriter(fullPath2, false))
            {
                // sw.WriteLine($"{vendMach.currentBalance.ToString("c")} | {DateTime.UtcNow}| {vendMach.fedMoney} | {vendMach.machineBalance}");

                double totalsale = 0;

                foreach (VendingMachineItem item in vendMach.ListOfItems)
                {
                    sw.WriteLine($"{ item.Name} | {item.NumberSold}");
                    totalsale += item.Price * item.NumberSold;
                }
                sw.WriteLine("*****Total Sales*****");
                sw.WriteLine(totalsale.ToString("c"));
            }


                //Console.WriteLine("display all the items from the object vendMach that's a class variable");
        }

        public static void PrintToAuditFile(string lineToPrint)
        {
            string directory2 = Environment.CurrentDirectory;
            string fileName2 = "audit.txt";
            string fullPath2 = Path.Combine(directory2, fileName2);

            using (StreamWriter sw = new StreamWriter(fullPath2, true))
            {
                sw.WriteLine(lineToPrint);
            }
        }



    }



}
