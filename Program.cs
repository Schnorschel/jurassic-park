using System;
using System.Collections.Generic;
using System.Linq;

namespace jurassic_park
{
  class Program
  {
    static List<Dinosaur> JurassicPark = new List<Dinosaur>();

    static void ViewAll()
    {
      Console.WriteLine("Jurassic Park currently has the following inhabitants:");
      Console.WriteLine("------------------------------------------------------");

      DisplayListOfDinos(JurassicPark.OrderBy(dino => dino.DateAcquired));
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

      JurassicPark.Add(dino);
    }

    static void RemoveDino()
    {
      Console.Write("What's the dinosaur's name you want to remove? ");
      string name = Console.ReadLine();
      var RemoveDino = JurassicPark.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      while (RemoveDino == null && name != "c")
      {
        Console.Write($"{name} was not found in Jurassic Park. Try again or (c)ancel: "); name = Console.ReadLine();
        RemoveDino = JurassicPark.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      }
      if (name == "c") { return; }
      JurassicPark.Remove(RemoveDino);
      Console.WriteLine($"{name} was successfully removed from Jurassic Park.");
    }

    static void TransferDino()
    {
      Console.Write("What's the dinosaur's name you want to transfer? "); string name = Console.ReadLine();
      var MoveDino = JurassicPark.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      while (MoveDino == null && name != "c")
      {
        Console.Write($"{name} was not found in Jurassic Park. Try again or (c)ancel: "); name = Console.ReadLine();
        MoveDino = JurassicPark.FirstOrDefault(dino => dino.Name.ToLower() == name.ToLower());
      }
      if (name == "c") { return; }
      Console.WriteLine($"{name} is currently in enclosure {MoveDino.EnclosureNumber}.");
      Console.Write($"Which enclosure do you want to move {name} to? "); var enclosure = Console.ReadLine();
      int i;
      while (!int.TryParse(enclosure, out i))
      {
        Console.Write($"{enclosure} is not a valid number. Try again: "); enclosure = Console.ReadLine();
      }
      MoveDino.EnclosureNumber = int.Parse(enclosure);
      Console.WriteLine($"{name} was successfully moved to enclosure {enclosure}.");
    }

    static void ViewBigDinos()
    {
      Console.WriteLine("Jurassic Park's top three dinosaurs by weight are:");
      Console.WriteLine("--------------------------------------------------");

      DisplayListOfDinos(JurassicPark.OrderByDescending(dino => dino.Weight).Take(3));
    }

    static void ViewDiet()
    {
      Console.WriteLine("Jurassic Park's dinosaurs' diet is as follows:");
      Console.WriteLine("----------------------------------------------");
      foreach (var dino in JurassicPark)
      {
        Console.WriteLine($"{dino.Name} is a {dino.DietType}.");
      }
    }

    static void Quit()
    {
      Console.WriteLine("Thank you for visiting Jurassic Park. Come back again soon.");
    }

    static void DisplayListOfDinos(IEnumerable<Dinosaur> dinoList)
    {
      foreach (var dino in dinoList)
      {
        if (dinoList.First() != dino)
        {
          Console.WriteLine(new string('-', dino.Name.Length));
        }
        Console.Write($"{dino.Name}"); Console.WriteLine(", since " + dino.DateAcquired.ToString("M/dd/yyyy"));
        Console.WriteLine(new string('-', dino.Name.Length + dino.DateAcquired.ToString("M/dd/yyyy").Length + 8));
        Console.WriteLine($"  is a {dino.DietType},");
        Console.WriteLine("  weighs" + dino.Weight.ToString() + " lbs,");
        Console.WriteLine("  lives in enclosure {dino.EnclosureNumber}.");
        if (dinoList.Last() == dino)
        {
          Console.WriteLine("------------------------------------------------------");
        }
      }
    }

    static void Main(string[] args)
    {
      ConsoleKeyInfo userResponse;
      Console.WriteLine("Welcome to Jurassic Park!");
      do
      {
        Console.WriteLine("The following actions are available:");
        Console.WriteLine("                      (A)dd a dinosaur");
        Console.WriteLine("                      (R)emove a dinosaur");
        Console.WriteLine("                      (T)ransfer a dinosaur");
        Console.WriteLine("                 View (b)iggest dinosaurs");
        Console.WriteLine("  View all dinosaurs' (d)iet");
        Console.WriteLine("                      (V)iew all dinosaurs");
        Console.WriteLine("                      (Q)uit");

        Console.Write("What would you like to do? "); userResponse = Console.ReadKey();
        Console.WriteLine();
        while ("artbdvq".IndexOf(userResponse.Key.ToString().ToLower()) < 0)
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
          case "r":
            RemoveDino();
            break;
          case "t":
            TransferDino();
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
