using System;
using System.Collections.Generic;

using SFML.Graphics;

using JamUtilities;
using WorldInterfaces;

namespace Evolution
{
    public class Population : IGameObject
    {
        private IWorld _world;
        public  List<Animal> Animals {get; private set;}

        public int Count()
        {
            return Animals.Count;
        }
        public void AddRandomAnimal(uint numberOfAnimals)
        {
            for (uint i = 0; i < numberOfAnimals; i++ )
                Animals.Add(new Animal(_world));
        }

        public void Mate(uint numberOfChilds)
        {
            ///calculate mating probability
            double rank = 0;
            foreach (Animal anim in Animals)
            {
                rank = anim.SetRank(rank);
            }
            ///mating
            for (uint i = 0; i < numberOfChilds; i++)
            {
                /// first parent
                Animal A = Animals[0];
                /// second parent
                Animal B = Animals[0];
                /// find first parent
                double randomRank = RandomGenerator.GetRandomDouble(0, rank);
                foreach (Animal temp in Animals)
                {
                    if (temp.Rank >= randomRank)
                    {
                        A = temp;
                        break;
                    }
                }
                ///find second parent
                randomRank = RandomGenerator.GetRandomDouble(0, rank);
                foreach (Animal temp in Animals)
                {
                    if (temp.Rank >= randomRank)
                    {
                        B = temp;
                        break;
                    }
                }
                Console.WriteLine("mating: {0,5} and {1,5}", A.Id, B.Id);
                ///create new animal
                Animals.Add(new Animal(_world, A, B));
            }
        }

        public Population(IWorld world, uint populationSize)
        {
            _world = world;
            Animals = new List<Animal>();
            for (uint i = 0; i < populationSize; i++)
                Animals.Add(new Animal(_world));
        }

        #region IGameObject
        public bool IsDead()
        {
            return Animals.Count == 0;
        }
        public void GetInput()
        {
            foreach (Animal anim in Animals)
            {
                anim.GetInput();
            }
        }
        public void Update(TimeObject timeObject)
        {
            foreach (Animal anim in Animals)
            {
                anim.Update(timeObject);
            }
            Animals.RemoveAll(x => x.IsDead());
        }
        public void Draw(RenderWindow rw)
        {
            foreach (Animal anim in Animals)
            {
                anim.Draw(rw);
            }
        }
        #endregion
    }
}
