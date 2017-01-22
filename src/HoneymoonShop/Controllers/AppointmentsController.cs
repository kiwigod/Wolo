using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HoneymoonShop.Data;
using HoneymoonShop.Models;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HoneymoonShop.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        public Dictionary<int, double[]> opening;

        public AppointmentsController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
            opening = new Dictionary<int, double[]>();
            opening.Add(1, new double[] { 12, 17.50 });     // monday
            opening.Add(2, new double[] { 9.50, 17.50 });   // tuesday
            opening.Add(3, new double[] { 9.50, 17.50 });   // wednesday
            opening.Add(4, new double[] { 9.50, 17.50 });   // thursday
            opening.Add(5, new double[] { 9.50, 17.50 });   // friday
            opening.Add(6, new double[] { 12, 18 });        // saturday
            opening.Add(0, new double[] { 11, 17 });        // sunday
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
        public async Task<IActionResult> Validate(string date, string time, string name, string mdate, string phone, string mail, bool newsletter)
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
                            int.Parse(aTime[0]), int.Parse(aTime[1]), 0),
                        Mail = mail,
                        MDate = new DateTime(int.Parse(maDate[0]), int.Parse(maDate[1]), int.Parse(maDate[2])),
                        Name = name,
                        newsletter = newsletter,
                        PNumber = phone
                    }
                );
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            string message = $"Uw afspraak is ingepland op {aDate[1]}-{aDate[2]}";
            await Message.Send("smtptestcore@gmail.com",
                "smtptestcore@gmail.com",
                "smtptestcore@gmail.com",
                "smtptestcore@gmail.com",
                message, "smtp.gmail.com",
                587,
                "smtptestcore@gmail.com",
                "smtptestcore@gmail.com",
                "testcore",
                name,
                mail,
                "Afspraak");

            if (newsletter == true)
            {
                message = "U bent aangemeld voor onze nieuwsbrief";
                await Message.Send("smtptestcore@gmail.com",
                    "smtptestcore@gmail.com",
                    "smtptestcore@gmail.com",
                    "smtptestcore@gmail.com",
                    message, 
                    "smtp.gmail.com",
                    587,
                    "smtptestcore@gmail.com",
                    "smtptestcore@gmail.com",
                    "testcore",
                    name,
                    mail,
                    "Nieuwsbrief");
            }
            //return RedirectToAction("Complete");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Complete()
        {
            return View();
        }

        [HttpGet]
        public string DatesUnavailableInMonth(int month, int year)
        {
            int days = DateTime.DaysInMonth(year, month);
            string s = string.Empty;
            for (int i = 1; i <= days; i++)
            {
                string today = $"{year}-{month}-{i}";
                if (TimeAvailableAtDate(today).Length == 0)
                {
                    s += $"{i},";
                }
            }
            return s;
        }

        [Authorize]
        public IActionResult Overview()
        {
            List<Appointment> appointments = _context.Appointment
                .Where(a => a.Date.Year >= DateTime.Now.Year)
                .Where(a => a.Date.Month >= DateTime.Now.Month)
                .ToList();
            return View(appointments);
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

            if (currentAppointments.Count == 0)
            {
                int index = (int)new DateTime(int.Parse(aDate[0]), int.Parse(aDate[1]), int.Parse(aDate[2])).Date.DayOfWeek;
                double[] time = opening[index];
                List<double> hoursOccupied = genTimetable(time);
                return checkTime(hoursOccupied);
            }
            else
            {
                double[] time = opening[((int)currentAppointments[0].Date.DayOfWeek)];
                List<double> hoursOccupied = genTimetable(time);
                foreach (Appointment a in currentAppointments)
                {
                    if (a.Date.Minute == 0)
                    {
                        hoursOccupied.Remove(a.Date.Hour);
                        hoursOccupied.Remove(a.Date.Hour + 0.5);
                        hoursOccupied.Remove(a.Date.Hour + 1);
                        hoursOccupied.Remove(a.Date.Hour + 1.5);
                        hoursOccupied.Remove(a.Date.Hour + 2);
                        hoursOccupied.Remove(a.Date.Hour + 2.5);
                    }
                    else
                    {
                        hoursOccupied.Remove(a.Date.Hour + 0.5);
                        hoursOccupied.Remove(a.Date.Hour + 1);
                        hoursOccupied.Remove(a.Date.Hour + 1.5);
                        hoursOccupied.Remove(a.Date.Hour + 2);
                        hoursOccupied.Remove(a.Date.Hour + 2.5);
                        hoursOccupied.Remove(a.Date.Hour + 3);
                    }
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
