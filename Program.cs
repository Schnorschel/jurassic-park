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
      string name = ReadLine();

    }

    static void TransferDino()
    {

    }

    static void ViewBigDinos()
    {

    }

    static void ViewDiet()
    {

    }

    static void Quit()
    {

    }

    static void DisplayListOfDinos(IEnumerable<Dinosaur> dinoList)
    {
      Console.WriteLine("Jurassic Park currently has the following inhabitants:");
      Console.WriteLine("------------------------------------------------------");
      foreach (var dino in dinoList)
      {
        if (dinoList.First() != dino)
        {
          Console.WriteLine(new string('-', dino.Name.Length));
        }
        Console.Write($"{dino.Name}"); Console.WriteLine(", since " + dino.DateAcquired.ToString("M/dd/yyyy"));
        Console.WriteLine(new string('-', dino.Name.Length + dino.DateAcquired.ToString("M/dd/yyyy").Length + 8));
        Console.WriteLine($"  {dino.DietType}");
        Console.WriteLine("  " + dino.Weight.ToString() + " lbs");
        Console.Write("  lives in enclosure "); Console.WriteLine(dino.EnclosureNumber);
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
        Console.WriteLine("  (A)dd a dinosaur");
        Console.WriteLine("  (R)emove a dinosaur");
        Console.WriteLine("  (V)iew all dinosaurs");
        Console.WriteLine("  (Q)uit");

        Console.Write("What would you like to do? "); userResponse = Console.ReadKey();
        Console.WriteLine();
        while ("arvq".IndexOf(userResponse.Key.ToString().ToLower()) < 0)
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
          default: break;
        }

      } while (userResponse.Key.ToString().ToLower() != "q");
    }
  }
}
