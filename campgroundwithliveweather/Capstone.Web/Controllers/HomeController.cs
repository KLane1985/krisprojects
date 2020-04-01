using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Web.Models;
using Capstone.Web.DAL;
using Capstone.Web.Extensions;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private IParkDAL parkDAL;
        public HomeController(IParkDAL parkDAL)
        {
            this.parkDAL = parkDAL;
        }

        public IActionResult Index()
        {
            IList<ParkModel> parks = parkDAL.GetParks();
            return View(parks);
        }
        public async Task<IActionResult> ParkDetail(string code)
        {
            ParkModel park = parkDAL.GetPark(code);
            ViewData["weathers"] = await parkDAL.GinerateWeathers (code);
            ViewData["settings"] = GetUserSettings();
            return View(park);
        }
        public IActionResult ChangeTempType(string tempType, string code)
        {
            UserSettingsModel settings = GetUserSettings();
            settings.TempType = tempType;
            SaveUserSettings(settings);

            return RedirectToAction("ParkDetail", new { code });
        }
        [HttpPost]
        public IActionResult ShowAdvisory(string advisory, string code)
        {
            TempData["advisory"] = advisory;
            return RedirectToAction("ParkDetail", new { code });
        }


        private UserSettingsModel GetUserSettings()
        {
            //try and get the cart from the session
            UserSettingsModel settings = HttpContext.Session.Get<UserSettingsModel>("settings");

            if (settings == null) //the shopping cart does NOT exist
            {
                settings = new UserSettingsModel();
                SaveUserSettings(settings);
            }

            return settings;
        }

        private void SaveUserSettings(UserSettingsModel settings)
        {
            HttpContext.Session.Set("settings", settings);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
