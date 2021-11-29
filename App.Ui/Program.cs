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
            GetSamurais("Before Add");
            AddSamurais("Michelle", "Julie");
            GetSamurais("After Add");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamurais(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
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
    }
}