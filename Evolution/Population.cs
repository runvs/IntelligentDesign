using System;
using System.Collections.Generic;

using SFML.Graphics;

using JamUtilities;

namespace Evolution
{
    class Population : IGameObject
    {
        private World _world;
        private List<Animal> _animals;

        public void Mate(uint numberOfChilds)
        {
            ///calculate mating probability
            double rank = 0;
            foreach (Animal anim in _animals)
            {
                rank = anim.SetRank(rank);
            }
            ///mating
            for (uint i = 0; i < numberOfChilds; i++)
            {
                /// first parent
                Animal A = _animals[0];
                /// second parent
                Animal B = _animals[0];
                /// find first parent
                double randomRank = RandomGenerator.GetRandomDouble(0, rank);
                foreach (Animal temp in _animals)
                {
                    if (temp.Rank >= randomRank)
                    {
                        A = temp;
                        break;
                    }
                }
                ///find second parent
                randomRank = RandomGenerator.GetRandomDouble(0, rank);
                foreach (Animal temp in _animals)
                {
                    if (temp.Rank >= randomRank)
                    {
                        B = temp;
                        break;
                    }
                }
                Console.WriteLine("mating: {0,5} and {1,5}", A.Id, B.Id);
                ///create new animal
                _animals.Add(new Animal(_world, A, B));
            }
        }

        public Population(World world, uint populationSize)
        {
            _world = world;
            _animals = new List<Animal>();
            for (uint i = 0; i < populationSize; i++)
                _animals.Add(new Animal(_world));
        }

        #region IGameObject
        public bool IsDead()
        {
            return _animals.Count == 0;
        }
        public void GetInput()
        {
            foreach (Animal anim in _animals)
            {
                anim.GetInput();
            }
        }
        public void Update(TimeObject timeObject)
        {
            foreach (Animal anim in _animals)
            {
                anim.Update(timeObject);
            }
        }
        public void Draw(RenderWindow rw)
        {
            foreach (Animal anim in _animals)
            {
                anim.Draw(rw);
            }
        }
        #endregion
    }
}
