using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public interface IParkDAL
    {

        IList<ParkModel> GetParks();
        ParkModel GetPark(string parkCode);
        Dictionary<string, string> GetParksDictionary();
        Task<IList<WeatherModel>> GinerateWeathers(string parkCode);
    }
}
