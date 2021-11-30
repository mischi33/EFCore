﻿using System;
using System.Linq;
using App.Data;
using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Ui
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            // Checks if DB is there - if not it will be created
            _context.Database.EnsureCreated();

            // GetSamurais("Before Add");
            // AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
            // GetSamurais("After Add");

            // AddVariousTypes();

            // QueryFilters();
            // QueryAggregates();

            // RetrieveAndUpdateSamurai();
            // RetrieveAndUpdateMultipleSamurais();

            RetrieveAndDeleteSamurai();

            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            // _context.Samurais.AddRange(
            //     new Samurai { Name = "Shimada" },
            //     new Samurai { Name = "Okamoto" }
            // );
            // _context.Battles.AddRange(
            //     new Battle { Name = "Battle of Anegawa" },
            //     new Battle { Name = "Battle of Nagashino" }
            // );
            // The above code does the same as the following
            _context.AddRange(
                new Samurai { Name = "Shimada" },
                new Samurai { Name = "Okamoto" },
                new Battle { Name = "Battle of Anegawa" },
                new Battle { Name = "Battle of Nagashino" }
            );
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
            // This adds a comment before the executed sql statement in logs
            // .TagWith("ConsoleApp.Program.GetSamurais method")
            .TagWith("ConsoleApp.Program.GetSamurais method")
            .ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            var name = "Michelle";
            // creates "WHERE s.Name == name" 
            var samurais = _context.Samurais.Where(s => s.Name == name).ToList();

            // creates "WHERE s.Name LIKE 'M'"
            var samurais2 = _context.Samurais.Where(s => s.Name.Contains("M%")).ToList();

            // creates also a "WHERE s.Name LIKE 'M'"
            var samurais3 = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "M%")).ToList();

        }

        private static void QueryAggregates()
        {
            var name = "Michelle";
            // var samurai = _context.Samurais.Where(s => s.Name == name).FirstOrDefault();
            // The following is the same as above
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
        }

        private static void FindById()
        {
            var id = 2;
            // This is not a LINQ method - this will execute right away
            var samurai = _context.Samurais.Find(id);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            // This is performing a bulk operation with 4 update statements
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(samurai => samurai.Name += "San");
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteSamurai() {
            var samurai = _context.Samurais.Find(2);
            _context.Samurais.Remove(samurai);
            // Like Add/AddRange there's also a RemoveRange Method
            // And you can also call it on the context directly to delete multiple different types ob objects
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected() {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext()) {
                disconnectedBattles = _context.Battles.ToList();
            } // context1 is disposed

            disconnectedBattles.ForEach(b => {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using (var context2 = new SamuraiContext()) {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }
    }
}
