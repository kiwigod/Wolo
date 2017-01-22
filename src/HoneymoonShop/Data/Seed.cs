using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoneymoonShop.Models;
using HoneymoonShop.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace HoneymoonShop.Data
{
    public static class Seed
    {
        public static void Init(IHostingEnvironment env, ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            AddAppointments(context, env);
            AddReview(context);

            AddCategory(context);
            AddColor(context);
            AddFeature(context);
            AddManu(context);
            AddNeckline(context);
            AddSilhouette(context);
            AddStyle(context);

            context.SaveChanges();

            AddDress(context);
            AddDressColor(context);
            AddDressFeature(context);

            context.SaveChanges();
            context.Dispose();
        }

        private static void AddAppointments(ApplicationDbContext context, IHostingEnvironment env)
        {
            AppointmentsController appControl = new AppointmentsController(context, env);
            Random rand = new Random();
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
            int range = (DateTime.Today - start).Days;
            for (int i = 1; i <= 5; i++)
            {
                DateTime app = new DateTime(rand.Next(DateTime.Now.Year, DateTime.Now.Year + 1), rand.Next(DateTime.Now.Month, 13), rand.Next(1, 29));
                string time = appControl.TimeAvailableAtDate($"{app.Year}-{app.Month}-{app.Day}");
                while (time.Length < 1)
                {
                    app = new DateTime(rand.Next(DateTime.Now.Year, DateTime.Now.Year + 1), rand.Next(1, 13), rand.Next(1, 29));
                    time = appControl.TimeAvailableAtDate($"{app.Year}-{app.Month}-{app.Day}");
                }
                string[] times = time.Split(',');
                times = times[0].Split(':');
                context.Add(
                    new Appointment()
                    {
                        Mail = "Mail@Mail.com",
                        Name = "Name Name",
                        newsletter = Convert.ToBoolean(rand.Next(0, 1)),
                        PNumber = "06" + rand.Next(10000000, 99999999),
                        Date = new DateTime(rand.Next(DateTime.Now.Year, DateTime.Now.Year + 1), rand.Next(1, 13), rand.Next(1, 29), int.Parse(times[0]), int.Parse(times[1]), 0),
                        MDate = new DateTime(rand.Next(DateTime.Now.Year, DateTime.Now.Year + 1), rand.Next(1, 13), rand.Next(1, 29))
                    }
                    );
            }
        }

        private static void AddCategory(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Category()
                    {
                        Name = "Cat " + i
                    }
                    );
            }
        }

        private static void AddColor(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Color()
                    {
                        Name = "Color " + i,
                        ColorCode = "#FFF"
                    }
                    );
            }
        }

        private static void AddDress(ApplicationDbContext context)
        {
            Random rand = new Random();
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Dress()
                    {
                        CategoryID = rand.Next(1, 6),
                        Description = "Description " + i,
                        ManuID = rand.Next(1, 6),
                        NecklineID = rand.Next(1, 6),
                        Price = rand.Next(0, 20000),
                        SilhouetteID = rand.Next(1, 6),
                        StyleID = rand.Next(1, 6)
                    }
                    );
            }
        }

        private static void AddDressColor(ApplicationDbContext context)
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int rDressID = rand.Next(1, 6);
                int rColorID = rand.Next(1, 6);
                DressColor d = DressColorExists(rDressID, rColorID, context);

                while (d != null)
                {
                    rDressID = rand.Next(1, 6);
                    rColorID = rand.Next(1, 6);
                    d = DressColorExists(rDressID, rColorID, context);
                }

                context.Add(new DressColor() { ColorID = rColorID, DressID = rDressID });
                context.SaveChanges();
            }
        }

        private static DressColor DressColorExists(int dID, int cID, ApplicationDbContext context)
        {
            DressColor d;
            try
            {
                d = context.DressColor.AsNoTracking().Where(dc => dc.DressID == dID && dc.ColorID == cID).First();
            } catch
            {
                d = null;
            }
            return d;
        }

        private static void AddDressFeature(ApplicationDbContext context)
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int rDressID = rand.Next(1, 6);
                int rFeatureID = rand.Next(1, 6);
                DressFeature d = DressFeatureExists(rDressID, rFeatureID, context);

                while (d != null)
                {
                    rDressID = rand.Next(1, 6);
                    rFeatureID = rand.Next(1, 6);
                    d = DressFeatureExists(rDressID, rFeatureID, context);
                }

                context.Add(new DressFeature() { DressID = rDressID, FeatureID = rFeatureID });
                context.SaveChanges();
            }
        }

        private static DressFeature DressFeatureExists(int dID, int fID, ApplicationDbContext context)
        {
            DressFeature d;
            try
            {
                d = context.DressFeature.AsNoTracking().Where(df => df.DressID == dID && df.FeatureID == fID).First();
            } catch
            {
                d = null;
            }
            return d;
        }

        private static void AddFeature(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Feature()
                    {
                        Name = "Feature " + i
                    }
                    );
            }
        }

        private static void AddManu(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Manu()
                    {
                        Name = "Manu " + i
                    }
                    );
            }
        }

        private static void AddNeckline(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Neckline()
                    {
                        Name = "Neckline " + i
                    }
                    );
            }
        }

        private static void AddReview(ApplicationDbContext context)
        {
            Random rand = new Random();
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Review()
                    {
                        Description = "Review " + i,
                        Date = DateTime.Now,
                        Name = "Name " + i,
                        Rating = rand.Next(1, 6)
                    }
                    );
            }
        }

        private static void AddSilhouette(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Silhouette()
                    {
                        Name = "Silhouette " + i
                    }
                    );
            }
        }

        private static void AddStyle(ApplicationDbContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                context.Add(
                    new Style()
                    {
                        Name = "Style " + i
                    }
                    );
            }
        }
    }
}
