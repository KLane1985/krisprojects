using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        IList<Campground> ListCampgroundsFromPark(int parkID);

        string GetDateAsString(int dateNum);
    }
}
