using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JamUtilities;
using Evolution;

namespace Evolution.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Evolution Testing Programm!");
            TempWorld world = new TempWorld();
            Population population = new Population(world, 1000);

            TimeObject time = new TimeObject(0,0,false);
            for (uint i = 0; i < 100; i++)
            {
                StreamWriter stream = new StreamWriter(String.Format("{0:000}.txt", i));
                foreach (Animal anim in population.Animals)
                {
                    anim.Save(stream);
                }
                stream.Close();
                int count = population.Count();
                population.Update(time);
                int deaths = count - population.Count();
                population.AddRandomAnimal((uint)deaths);
                Console.WriteLine("deaths: {0:0000} births: {1:0000} population {2:0000}", deaths, 0, population.Count());
            }
        }
    }
}
