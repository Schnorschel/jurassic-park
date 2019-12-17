using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace jurassic_park
{
  class Program
  {
    // static List<Dinosaur> JurassicPark = new List<Dinosaur>();
    static JurassicParkContext Db = new JurassicParkContext();
    static string[] babyNames = new string[] { "Junior", "Bashful", "Happy" };
    static string[] diet = new string[] { "herbivore", "carnivore" };

    static void ViewAll()
    {
      Console.WriteLine("Jurassic Park currently has the following inhabitants:");
      Console.WriteLine("------------------------------------------------------");

      DisplayListOfDinos(Db.Dinosaurs.OrderBy(dino => dino.DateAcquired));
    }

    static void AddDino()
    {
      Console.WriteLine("Add a new Dinosaur to Jurassic Park");
      Console.WriteLine("-----------------------------------");
      Console.Write("                  What's the dinosaur's name? "); string name = Console.ReadLine();
      Console.Write("Is the dinosaur a (h)erbivore or (c)arnivore? "); string diet = Console.ReadLine();
      Console.Write("              When was the dinosaur acquired? "); string acquired = Console.ReadLine();
      Console.Write("       What's the dinosaur's weight (in lbs)? "); string weight = Console.ReadLine();
      Console.Write("   What enclosure is the dinosaur inhabiting? "); string enclosure = Console.ReadLine();

      var dino = new Dinosaur();
      dino.Name = name;
      dino.DietType = diet.Substring(0, 1).ToLower() == "h" ? "herbivore" : diet.Substring(0, 1).ToLower() == "c" ? "carnivore" : "";
      dino.DateAcquired = DateTime.Parse(acquired);
      dino.Weight = int.Parse(weight);
      dino.EnclosureNumber = int.Parse(enclosure);

      Db.Dinosaurs.Add(dino);
      Db.SaveChanges();
    }

    static void RemoveDino()
    {
      Console.Write("What's the dinosaur's name you want to remove? ");
      string name = Console.ReadLine();
      var RemoveDino = Db.Dinosaurs.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      while (RemoveDino == null && name != "c")
      {
        Console.Write($"{name} was not found in Jurassic Park. Try again or (c)ancel: "); name = Console.ReadLine();
        RemoveDino = Db.Dinosaurs.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      }
      if (name == "c") { return; }
      Db.Dinosaurs.Remove(RemoveDino);
      Db.SaveChanges();
      Console.WriteLine($"{name} was successfully removed from Jurassic Park.");
    }

    static void TransferDino()
    {
      Console.Write("What's the dinosaur's name you want to transfer? "); string name = Console.ReadLine();
      var MoveDino = Db.Dinosaurs.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      while (MoveDino == null && name != "c")
      {
        Console.Write($"{name} was not found in Jurassic Park. Try again or (c)ancel: "); name = Console.ReadLine();
        MoveDino = Db.Dinosaurs.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      }
      if (name == "c") { return; }
      Console.WriteLine($"{name} currently lives in " + (MoveDino.EnclosureNumber == -1 ? "the wild" : $"enclosure {MoveDino.EnclosureNumber}") + ".");
      Console.Write($"Which enclosure do you want to move {name} to? "); var enclosure = Console.ReadLine();
      int i;
      while (!int.TryParse(enclosure, out i))
      {
        Console.Write($"{enclosure} is not a valid number. Try again: "); enclosure = Console.ReadLine();
      }
      MoveDino.EnclosureNumber = int.Parse(enclosure);
      Db.SaveChanges();
      Console.WriteLine($"{name} was successfully moved to enclosure {enclosure}.");
    }

    static void ViewBigDinos()
    {
      Console.WriteLine("Jurassic Park's top three dinosaurs by weight are:");
      Console.WriteLine("--------------------------------------------------");

      DisplayListOfDinos(Db.Dinosaurs.OrderByDescending(dino => dino.Weight).Take(3));

      // Console.WriteLine("--------------------------------------------------");
    }

    static void ViewDiet()
    {
      Console.WriteLine("Jurassic Park's dinosaurs' diet is as follows:");
      Console.WriteLine("----------------------------------------------");
      foreach (var dino in Db.Dinosaurs)
      {
        Console.WriteLine($"{dino.Name} is a {dino.DietType}.");
      }
      Console.WriteLine("----------------------------------------------");
    }

    static void ReleaseFromEnclosure()
    {
      Console.Write("Which enclosure do you want to open for release? "); string enclosure = Console.ReadLine();
      int numReleased = 0;
      string releasedDinos = "";

      foreach (var dino in Db.Dinosaurs)
      {
        if (dino.EnclosureNumber == int.Parse(enclosure))
        {
          numReleased++;
          releasedDinos = releasedDinos + (releasedDinos.Length > 0 ? ", " : "") + dino.Name;
          dino.EnclosureNumber = -1;
        }
      }
      if (releasedDinos.Length > 0)
      {
        Db.SaveChanges();
        Console.WriteLine($"The following {numReleased} dinosaur" + plurify(numReleased) + " " + wasWere(numReleased) + $" released into the wild from closure {enclosure}:");
        Console.WriteLine(releasedDinos);
      }
      else
      {
        Console.WriteLine($"There are currently no dinosaurs in enclosure {enclosure}");
      }

    }

    static void HatchDino()
    {

      Random rnd = new Random();
      string randomName = babyNames[rnd.Next(0, 3)];
      int randomWeight = rnd.Next(200, 1501);
      string randomDiet = diet[rnd.Next(0, 2)];

      Dinosaur babyDino = new Dinosaur();

      babyDino.Name = randomName;
      babyDino.Weight = randomWeight;
      babyDino.DietType = randomDiet;
      babyDino.DateAcquired = DateTime.Today;
      babyDino.EnclosureNumber = rnd.Next(1, 11);

      Console.WriteLine($"{babyDino.Name}, a {babyDino.DietType}, weighing in at {babyDino.Weight} lbs, was born in enclosure {babyDino.EnclosureNumber}.");
      Console.WriteLine("Congratulate the proud parents!");
      Db.Add(babyDino);
      Db.SaveChanges();
    }

    static void NeedsASheep()
    {
      Console.WriteLine("Someone is a little lean today. The following fella could use a sheep:");
      DisplayListOfDinos(Db.Dinosaurs.Where(dino => dino.DietType == "carnivore").OrderBy(dino => dino.Weight).Take(1));

    }

    static void Quit()
    {
      Console.WriteLine("Thank you for visiting Jurassic Park. Come back again soon.");
    }

    static void DisplayListOfDinos(IEnumerable<Dinosaur> dinoList)
    {
      int i = 0;

      foreach (var dino in dinoList)
      {
        int dinoNameLength = dino.Name.Length;
        if (i++ > 0)
        {
          Console.WriteLine(new string('-', dinoNameLength));
        }
        Console.Write($"{dino.Name}"); Console.WriteLine(", since " + dino.DateAcquired.ToString("M/dd/yyyy"));
        Console.WriteLine(new string('-', dino.Name.Length + dino.DateAcquired.ToString("M/dd/yyyy").Length + 8));
        Console.WriteLine($"  ..is a {dino.DietType},");
        Console.WriteLine("  weighs " + dino.Weight.ToString() + " lbs,");
        Console.WriteLine("  lives in " + (dino.EnclosureNumber == -1 ? "the wild" : $"enclosure {dino.EnclosureNumber}") + ".");
      }
      Console.WriteLine("------------------------------------------------------");
    }

    static string plurify(int num)
    {
      return (num == 1) ? "" : "s";
    }

    static string wasWere(int num)
    {
      return (num == 1) ? "was" : "were";
    }

    static void Main(string[] args)
    {
      ConsoleKeyInfo userResponse;

      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

      IConfigurationRoot configuration = builder.Build();

      Console.WriteLine("Welcome to Jurassic Park!");
      do
      {
        Console.WriteLine("The following actions are available:");
        Console.WriteLine("                      (A)dd a dinosaur");
        Console.WriteLine("                      (R)emove a dinosaur");
        Console.WriteLine("                      (T)ransfer a dinosaur");
        Console.WriteLine("                 Relea(s)e dinosaurs from enclosure");
        Console.WriteLine("                      (H)atch a new dinosaur");
        Console.WriteLine("                 View (b)iggest dinosaurs");
        Console.WriteLine("              Someone (N)eeds a sheep");
        Console.WriteLine("  View all dinosaurs' (d)iet");
        Console.WriteLine("                      (V)iew all dinosaurs");
        Console.WriteLine("                      (Q)uit");

        Console.Write("What would you like to do? "); userResponse = Console.ReadKey();
        Console.WriteLine();
        while ("artshnbdvq".IndexOf(userResponse.Key.ToString().ToLower()) < 0)
        {
          Console.Write("That's not an accepted response. Try again: "); userResponse = Console.ReadKey();
          Console.WriteLine();
        }

        switch (userResponse.Key.ToString().ToLower())
        {
          case "a":
            AddDino();
            break;
          case "v":
            ViewAll();
            break;
          case "d":
            ViewDiet();
            break;
          case "b":
            ViewBigDinos();
            break;
          case "n":
            NeedsASheep();
            break;
          case "h":
            HatchDino();
            break;
          case "r":
            RemoveDino();
            break;
          case "t":
            TransferDino();
            break;
          case "s":
            ReleaseFromEnclosure();
            break;
          case "q":
            Quit();
            break;
          default: break;
        }

      } while (userResponse.Key.ToString().ToLower() != "q");
    }
  }
}
