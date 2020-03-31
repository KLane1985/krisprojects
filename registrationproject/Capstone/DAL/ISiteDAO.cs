using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{
    public interface ISiteDAO
    {

        IList<Site> ListSitesAtCampground(int campgroundID);
        IList<Site> ListSitesAtCampgroundWithinDate(int campgroundID, IList<DateTime> departArriveDates);
    }
}
