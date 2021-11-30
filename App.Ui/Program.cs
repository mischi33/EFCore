﻿using App.Data;
using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Ui
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

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

        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(2);
            _context.Samurais.Remove(samurai);
            // Like Add/AddRange there's also a RemoveRange Method
            // And you can also call it on the context directly to delete multiple different types ob objects
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            } // context1 is disposed

            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        /************** Working with related data ***************/

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote> {
                    new Quote { Text = "I've come to save you" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote> {
            new Quote {Text = "Watch out for my sharp sword!"},
            new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
        }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            // EF Core knows that the quote is new because it has no ID yet
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                // Update can be used here but Attach is better for performance
                // newContext.Samurais.Update(samurai);
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            // Since you do not need to do anything on the samurai object you can just set the samuraiId directly
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            // Loads all samurais and all quotes and assembles them in memory (LEFT JOIN) -- one query
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();

            // Split query first selects all samurais and then does an INNER JOIN on quotes -- two queries
            var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();

            // Filter for a specific string inside a quote
            var filteredInclude = _context.Samurais.Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();

            // Filter on Samurais and include all Quotes from this Samurai
            var filterPrimaryEntityWithInclude = _context.Samurais.Where(s => s.Name.Contains("Sampson")).Include(s => s.Quotes).FirstOrDefault();
        }

        private static void ProjectSomeProperties()
        {
            // this returns a new object with the id and the name --> anonymous type
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();

            // If you need to use this object ouside of the query you need to create a new class or struct for it
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            // creates a new anonymouse type with the id, name and quotes count
            var somePropsWithQuotes = _context.Samurais
               .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count })
               .ToList();

            // Filter quotes
            var somePropsWithQuotesFiltered = _context.Samurais
                .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) }).ToList();

            // Filter for quotes and add whole samurai to anonymouse object
            var samuraisAndQuotes = _context.Samurais
                .Select(s => new { Samurai = s, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) }).ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = _context.Samurais.Find(1);
            // Load entry from DB for an object that is already in memory
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void FiteringWithRelatedData()
        {
            // when you do not need the quotes but want to filter for them
            var samurais = _context.Samurais.Where(s => s.Quotes.Any(q => q.Text.Contains("happy"))).ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 1);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 1);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext = new SamuraiContext();

            // All Quotes are updated
            // newContext.Quotes.Update(quote);

            // Only the specified entry is considered, anything else is ignored
            // You basically set the state to modified so that ef core knows that this entity has been modified
            // This happens automatically when the object is still tracked - here you do it manually
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }

        private static void AddingNewSamuraiToAnExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }

        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }

        private static void ReturnAllBattlesWithSamurais()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
        }

        private static void AddAllSamuraisToAllBattles()
        {
            // This could cause an exception for duplicate row if the connection of a samurai to a battle is already there
            // On an error the whole transaction will be roled back
            var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
            var allSamurais = _context.Samurais.ToList();
            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void WillNotRemoveSamuraiFromABattle()
        {
            var battle = _context.Battles.Find(1);
            var samurai = _context.Samurais.Find(12);
            battle.Samurais.Remove(samurai);
            _context.SaveChanges(); //the relationship is not being tracked
        }

        private static void RemoveSamuraiFromABattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
            if (b_s != null)
            {
                _context.Remove(b_s); //_context.Set<BattleSamurai>().Remove works, too
                _context.SaveChanges();
            }
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(12);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
            samurai.Horse = new Horse { Name = "Mr. Ed" };

            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }

        private static void ReplaceAHorse()
        {
            //var samurai = _context.Samurais.Include(s=>s.Horse)
            //                      .FirstOrDefault(s => s.Id == 5);
            //samurai.Horse = new Horse { Name = "Trigger" };
            var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Mr. Ed");
            horse.SamuraiId = 5; //owns Trigger! savechanges will fail
            _context.SaveChanges();

        }

        private static void GetSamuraisWithHorse()
        {
            var samurais = _context.Samurais.Include(s => s.Horse).ToList();
        }

        private static void GetHorsesWithSamurai()
        {
            var horseOnly = _context.Set<Horse>().Find(3);

            var horseWithSamurai = _context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Horse.Id == 3);

            var horseSamuraiPairs = _context.Samurais.Where(s => s.Horse != null).Select(s => new { Horse = s.Horse, Samurai = s }).ToList();
        }

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

            // RetrieveAndDeleteSamurai();

            // InsertNewSamuraiWithAQuote();
            // InsertNewSamuraiWithManyQuotes();
            // AddQuoteToExistingSamuraiWhileTracked();
            // AddQuoteToExistingSamuraiNotTracked(1);
            // Simpler_AddQuoteToExistingSamuraiNotTracked(18);

            // ExplicitLoadQuotes();

            // ModifyingRelatedDataWhenTracked();

            // ModifyingRelatedDataWhenNotTracked();

            // AddingNewSamuraiToAnExistingBattle();
            // ReturnBattleWithSamurais();
            // ReturnAllBattlesWithSamurais();

            AddAllSamuraisToAllBattles();
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;

        }
    }
}
