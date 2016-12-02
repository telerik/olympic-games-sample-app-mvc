using OlympicGames.Models;
using OlympicGames.WebApiControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlympicGames.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Olympic Games - Medals by Country";
            return RedirectToActionPermanent("Medals_by_Country");
        }

        public ActionResult Medals_by_Country()
        {

            ViewBag.Title = "Olympic Games - Medals by Country";
            return View();
        }

        public ActionResult Medals_by_Sport()
        {
            ViewBag.Title = "Olympic Games - Medals by Sport";
            return View();
        }

        public ActionResult Top_Results_by_Sport()
        {
            ViewBag.Title = "Olympic Games - Top Results by Sport";
            return View();
        }

        public ActionResult Athlete_Comparison()
        {
            ViewBag.Title = "Olympic Games - Athlete Comparison";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Olympic Games - About";
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
