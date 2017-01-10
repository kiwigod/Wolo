using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneymoonShop.Data;
using Microsoft.AspNetCore.Hosting;
using HoneymoonShop.Models;

namespace HoneymoonShop.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        private Dictionary<int, double[]> opening;

        public AppointmentsController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
            opening = new Dictionary<int, double[]>();
            opening.Add(1, new double[] { 12, 17.50 });
            opening.Add(2, new double[] { 9.50, 17.50 });
            opening.Add(3, new double[] { 9.50, 17.50 });
            opening.Add(4, new double[] { 9.50, 17.50 });
            opening.Add(5, new double[] { 9.50, 17.50 });
            opening.Add(6, new double[] { 12, 18 });
            opening.Add(7, new double[] { 11, 17 });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Validate(string date, string time, string name, string mdate, string phone, string mail)
        {
            /***************
             * [0] - year
             * [1] - month
             * [2] - day
             ****************/
            string[] aDate = date.Split('-');
            string[] maDate = mdate.Split('-');

            /***************
             * [0] - Hour
             * [1] - Minute
             * [2] - Second
             ****************/
            string[] aTime = time.Split(':');

            try
            {
                _context.Add(
                    new Appointment()
                    {
                        Date = new DateTime(int.Parse(aDate[0]), int.Parse(aDate[1]), int.Parse(aDate[2]),
                        int.Parse(aTime[0]), int.Parse(aTime[1]), 0)
                    }
                );
                _context.SaveChanges();
                string state = "Uw afspraak is gemaakt";
                return View(state);
            } catch
            {
                string state = "Er is iets mis gegaan bij het inplannen van uw afspraak";
                return View(state);
            }
        }

        [HttpGet]
        public string TimeAvailableAtDate(string date)
        {
            /***************
            * [0] - year
            * [1] - month
            * [2] - day
            ****************/
            string[] aDate = date.Split('-');

            List<Appointment> currentAppointments = _context.Appointment.Where(a =>
                a.Date.Year == int.Parse(aDate[0]) &&
                a.Date.Month == int.Parse(aDate[1]) &&
                a.Date.Day == int.Parse(aDate[2])
            ).ToList();

            if (currentAppointments.Count == 0) {
                double[] time = opening[((int)new DateTime(int.Parse(aDate[0]), int.Parse(aDate[1]), int.Parse(aDate[2])).Date.DayOfWeek)];
                List<double> hoursOccupied = genTimetable(time);
                return checkTime(hoursOccupied);
            }
            else
            {
                double[] time = opening[((int)currentAppointments[0].Date.DayOfWeek)];
                List<double> hoursOccupied = genTimetable(time);
                foreach (Appointment a in currentAppointments)
                {
                    hoursOccupied.Remove(a.Date.Hour);
                    hoursOccupied.Remove(a.Date.Hour + 1);
                    hoursOccupied.Remove(a.Date.Hour + 2);
                }
                return checkTime(hoursOccupied);
            }
        }

        private string checkTime(List<double> hoursOccupied)
        {
            List<double> hoursAvailable = new List<double>();
            double temp = 0;
            foreach (double d in hoursOccupied)
            {
                if (hoursOccupied.Contains(d + 3))
                {
                    if (temp == 0)
                    {
                        temp = d + 3;
                        hoursAvailable.Add(d);
                    }
                    if (d >= temp)
                    {
                        temp = d + 3;
                        hoursAvailable.Add(d);
                    }
                }
            }
            string times = "";
            foreach (double d in hoursAvailable)
            {
                if (Math.Truncate(d) == d)
                {
                    times += d + ":00,";
                }
                else
                {
                    times += Math.Truncate(d) + ":30,";
                }
            }
            return times;
        }
        private List<double> genTimetable(double[] time)
        {
            List<double> hoursOccupied = new List<double>();
            for (double i = time[0]; i < time[1]; i += 0.5)
            {
                hoursOccupied.Add(i);
            }
            return hoursOccupied;
        }
    }
}