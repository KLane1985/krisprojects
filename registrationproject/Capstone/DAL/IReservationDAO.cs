using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        decimal GetTotalCost(decimal campgroundFee, IList<DateTime> departArriveDates);
        void MakeReservation(Site siteToReserve, IList<DateTime> departArriveDates);
    }
}
