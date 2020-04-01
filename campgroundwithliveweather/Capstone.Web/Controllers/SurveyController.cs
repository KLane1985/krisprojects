using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Web.DAL;
using Capstone.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capstone.Web.Controllers
{
    public class SurveyController : Controller
    {
        private IParkDAL parkDAL;
        private ISurveyDAL surveyDAL;
        public SurveyController(IParkDAL parkDAL, ISurveyDAL surveyDAL)
        {
            this.parkDAL = parkDAL;
            this.surveyDAL = surveyDAL;
        }
        [HttpGet]
        public IActionResult Survey()
        {
            Dictionary<string,string> parks = parkDAL.GetParksDictionary();
            ViewData["parks"] = parks;
            return View("Survey");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Survey(SurveyModel survey)
        {
            if (!ModelState.IsValid)
            {
                Dictionary<string, string> parks = parkDAL.GetParksDictionary();
                ViewData["parks"] = parks;
                return View(survey);
            }
            surveyDAL.SaveSurvey(survey);
            return RedirectToAction("SurveyResults","Survey");
        }

        public IActionResult SurveyResults()
        {
            IList<SurveyResultsModel> results = surveyDAL.GetResults();
            return View(results);
        }
    }
}