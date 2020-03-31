using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone
{
    public class CapstoneCLI
    {


        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        
        public CapstoneCLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        public void RunCLI()
        {
            GetAllParks();

            while(true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case "1":
                        DisplayParkInfo(1);
                        break;
                    case "2":
                        DisplayParkInfo(2);
                        break;
                    case "3":
                        DisplayParkInfo(3);
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
        public void GetAllParks()
        {
            Console.Clear();
            IList<Park> parks = parkDAO.ListAllAvailableParks();

            int i = 1;
            Console.WriteLine("Select a Park for Further Details");
            foreach (var park in parks)
            {
                Console.Write($"   {i}) ");
                Console.WriteLine($"{park.Name}");
                i++;
            }
            Console.WriteLine("   Q) quit");
        }

        public void DisplayParkInfo(int parkId)
        {
            Console.Clear();
            IList<Park> parks = parkDAO.ListAllAvailableParks();
            string parkName = "";

            foreach (var park in parks)
            {
                if (parkId == park.ParkId)
                {
                    parkName = park.Name;
                    Console.WriteLine($"{park.Name}");
                    Console.WriteLine($"Location:          {park.Location}");
                    Console.WriteLine($"Established:       {park.EstablishDate.ToString("MM/dd/yyyy")}");
                    Console.WriteLine($"Area:              {park.Area.ToString("0,000")} sq km");
                    Console.WriteLine($"Annual Visitors:   {park.Visitors.ToString("0,000,000")}");
                    Console.WriteLine();
                    Console.WriteLine($"{park.Description}");
                }
            }

            Console.WriteLine("Select a Command");
            Console.WriteLine("    1) View Campgrounds");
          //  Console.WriteLine("    2) Search For Reservations");
            Console.WriteLine("    2) Return to Previous Screen");

            while (true)
            {
                string command = Console.ReadLine();

                

                switch (command.ToLower())
                {
                    case "1":
                        DisplayCampgrounds(parkName, parkId);
                        break;
                   /* case "2":
                        //SearchForReservation(parkId);
                        break;*/
                    case "2":
                        RunCLI();
                        break;
                }
            }



        }

        public void DisplayCampgrounds(string parkName, int parkID)
        {
            int i = 0;
            
            Console.Clear();
            Console.WriteLine($"{parkName} National Park Campgrounds");
            Console.WriteLine();
            IList<Campground> campgrounds = campgroundDAO.ListCampgroundsFromPark(parkID);
            Console.WriteLine(String.Format("{0,3} {1,-33} {2,-10}{3,-10}{4,7}\n", "", "Name", "Open", "Close", "Daily Fee"));
            foreach (var campground in campgrounds)
            {
                i++;
                if (parkID == campground.ParkID)
                {
                    Console.WriteLine(string.Format("{0,3} {1,-33} {2,-10}{3,-10}{4,-7}", "#" + i, campground.Name, campgroundDAO.GetDateAsString(campground.OpenDate), campgroundDAO.GetDateAsString(campground.CloseDate), campground.DailyFee.ToString("C")));
                    
                }

                
            }
            
            Console.WriteLine("Select a Command");
            Console.WriteLine("    1) Search for Available Reservation");
            Console.WriteLine("    2) Return to Previous Screen");

            while (true)
            {
                string command = Console.ReadLine();

                int.TryParse(command, out int commandInt);
                if (commandInt ==1 )
                {
                    SearchForReservation(parkID);
                    //SearchForReservation(campgrounds[i - 1].ParkID);
                }
                else if (commandInt == 2)
                {
                    DisplayParkInfo(parkID);
                }
                else
                {
                    Console.WriteLine("try again");
                    continue;
                }



                /*
                switch (command.ToLower())
                {
                    case "1":
                        SearchForReservation(parkID);
                        break;
                    case "2":
                        DisplayParkInfo(parkID);
                        break;
                    
                }
                */
            }



        }

        public void SearchForReservation(int parkID)
        {
            Console.Clear();
            int i = 0;
            
            //print out list of parks, get a counter for each of them, and use the counter to decide index of each park.
            IList<Campground> campgrounds = campgroundDAO.ListCampgroundsFromPark(parkID);
            Console.WriteLine(String.Format("{0,3} {1,-33} {2,-10}{3,-10}{4,7}\n", "", "Name", "Open", "Close", "Daily Fee"));
            foreach (var campground in campgrounds)
            {
                i++;
                if (parkID == campground.ParkID)
                {
                    Console.WriteLine(string.Format("{0,3} {1,-33} {2,-10}{3,-10}{4,-7}", "#" + i, campground.Name, campgroundDAO.GetDateAsString(campground.OpenDate), campgroundDAO.GetDateAsString(campground.CloseDate), campground.DailyFee.ToString("C")));

                }

                
            }

            Console.WriteLine("Which campground (enter 0 to cancel)?");
            while (true)
            {
                string command = Console.ReadLine();
                int.TryParse(command, out int commandInt);
                if(commandInt <= i && commandInt != 0)
                {
                    
                    
                    GetDates(campgrounds[commandInt-1].CampgroundID, campgrounds[commandInt-1].ParkID);
                }
                else if(commandInt == 0)
                {
                    DisplayCampgrounds(campgrounds[i-1].Name, parkID);
                }
                else
                {
                    Console.WriteLine("try again");
                    continue;
                }

                //switch (command)
                //{
                //    case "1":
                //        GetDates(campgrounds[int.Parse(command)-1].CampgroundID);
                //        break;
                //    case "2":
                //        break;
                //    case "3":
                //        break;
                //    case "0":
                //        DisplayParkInfo(parkID);
                //        break;
                   

                //}
            }       
            
        }


        public void GetDates(int campgroundID, int parkID)
        {
            IList<DateTime> departArriveDates = new List<DateTime>();
            IList<Site> sitesToReserve = new List<Site>();

            while (true)
            {
                //DateTime fromDate;
                //DateTime toDate;
                try
                {
                    
                    Console.WriteLine("Please enter the arrival date: MM / DD / YYYY or press \"Q\" to quit");
                    string input = Console.ReadLine();
                    if(input.ToLower() == "q")
                    {
                        SearchForReservation(parkID);
                    } 
                    //string fromDateString = Console.ReadLine();
                    DateTime fromDate = Convert.ToDateTime(input);
                    Console.WriteLine("Please enter the departure date: MM / DD / YYYY or press \"Q\" to quit");
                    input = Console.ReadLine();
                    if (input.ToLower() == "q")
                    {
                        SearchForReservation(parkID);
                    }
                    //fromDate = Convert.ToDateTime(fromDateString);
                    DateTime toDate = Convert.ToDateTime(input);

                    
                    departArriveDates.Add(fromDate);
                    departArriveDates.Add(toDate);

                    IList<Site> sites = siteDAO.ListSitesAtCampgroundWithinDate(campgroundID, departArriveDates);
                    IList<Campground> campgrounds = campgroundDAO.ListCampgroundsFromPark(parkID);

                    decimal campgroundFee = 0;
                    
                    if (sites.Count == 0)
                    {
                        Console.WriteLine("No Results. Press any key to enter an alternate date range.");
                        Console.ReadKey();
                        GetDates(campgroundID, parkID);
                    }

                    Console.WriteLine("Results Matching Your Criteria: ");
                    
                    Console.WriteLine(string.Format("{0,-10}{1,-12}{2,-16}{3,-16}{4,-12}{5,-14}", "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost"));
                    foreach (Site site in sites)
                    {
                        foreach (Campground campground in campgrounds)
                        {
                            if (site.CampgroundID == campground.CampgroundID)
                            {
                               campgroundFee = reservationDAO.GetTotalCost(campground.DailyFee, departArriveDates);
                            }
                        }

                        sitesToReserve.Add(site);
                        Console.WriteLine(string.Format("{0,-10}{1,-12}{2,-16}{3,-16}{4,-12}{5,-14}", site.SiteNumber, site.MaxOccupancy, site.Accessible, site.MaxRvLength, site.Utilities, campgroundFee.ToString("C")));
                        
                    }
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    
                }

                

            }

            GetReservation(departArriveDates, sitesToReserve);
        }

        public void GetReservation(IList<DateTime> departArriveDates, IList<Site> sitesToReserve)
        {
            Console.WriteLine("Which campground would you like to reserve? Press 0 to cancel");

            string input = Console.ReadLine();
            try
            {
                foreach (Site site in sitesToReserve)
                {
                    if (site.SiteNumber == int.Parse(input))
                    {
                        Site reserveSite = site;
                        reservationDAO.MakeReservation(reserveSite, departArriveDates);
                        RunCLI();
                        break;
                    }
                    else if (int.Parse(input) == 0)
                    {
                        RunCLI();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
           

        }
    }
}
