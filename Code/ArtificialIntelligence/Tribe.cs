using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace ArtificialIntelligence
{
    public class Tribe : IGameObject
    {

        private List<Animal> _animalList;
        private IWorld _world;
        public AnimalProperties Properties { get; private set; }

        public Vector2i PositionInTiles { get; set; }

        private Color _tribeColor;

        private int _numberOfAnimalsToSpawnThisRound;

        public Tribe(IWorld world, AnimalProperties properties)
        {
            if (world == null)
            {
                throw new ArgumentNullException("IWorld world", "could not resolve world in Tribe Constructor.");
            }
            _world = world;
            _animalList = new List<Animal>();
            Properties = properties;

            PositionInTiles = new Vector2i(_world.GetWorldProperties().WorldSizeInTiles.X / 2, _world.GetWorldProperties().WorldSizeInTiles.Y / 2);
            _tribeColor = RandomGenerator.GetRandomColor();
        }
    
        public bool IsDead()
        {
            return (_animalList.Count == 0);
        }

        public void GetInput()
        {

        }

        public void Update(TimeObject timeObject)
        {
            _numberOfAnimalsToSpawnThisRound = 0;
            Vector2f newCenterPosition = new Vector2f(0, 0);

            List<Animal>  newAnimalList = new List<Animal>();

            foreach (Animal a in _animalList)
            {
                a.Update(timeObject);
                newCenterPosition += new Vector2f(a.PositionInTiles.X, a.PositionInTiles.Y);

                if (!a.IsDead())
                {
                    newAnimalList.Add(a);
                }

            }
            newCenterPosition /= _animalList.Count;
            PositionInTiles = new Vector2i((int)newCenterPosition.X, (int)newCenterPosition.Y);

            _animalList = newAnimalList;

            //Console.WriteLine("TotalHealth : " + GetSummedCurrentHealth() + "\t/\t" + GetSummedMaxHealth() + "\t" + "\t" + GetSummedCurrentHealth() / GetSummedMaxHealth()* 100.0f  + "\t" + _animalList.Count);

            for (int i = 0; i != _numberOfAnimalsToSpawnThisRound; ++i)
            {
                SpawnAnimal();
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            foreach (IGameObject a in _animalList)
            {
                a.Draw(rw);
            }
        }

        public float GetSummedCurrentHealth()
        {
            float totalHealth = 0;
            foreach (Animal a in _animalList)
            {
                totalHealth += a.HealthCurrent;
            }

            return totalHealth;
        }



        public float GetSummedMaxHealth()
        {
            float totalHealth = 0;
            foreach (Animal a in _animalList)
            {
                totalHealth += a.HealthMax;
            }

            return totalHealth;
        }

        public void SpawnAnimal()
        {
            int count = _animalList.Count;
            int tribeSize = (int)(4.0f * (float)(Math.Sqrt(_animalList.Count)));
            int halfsize = -(tribeSize / 2) +1 ;

            Vector2i initialPosition = PositionInTiles + RandomGenerator.GetRandomVector2iInRect(new SFML.Graphics.IntRect(halfsize, halfsize, tribeSize, tribeSize));
            Animal animal = new Animal(Properties, _world, this, initialPosition);
            animal.AnimalColor = _tribeColor;
            _animalList.Add(animal);
        }

        public void LetThemHaveFun()
        {
            _numberOfAnimalsToSpawnThisRound++;
        }


        public bool TwoAnimalOnPosition(Vector2i position)
        {
            bool b1 = false;
            foreach (Animal a in _animalList)
            {
                if (b1 == false)
                {
                    if (a.PositionInTiles.Equals(position))
                    {
                        b1 = true;
                        continue;
                    }
                }
                else
                {
                    if(a.PositionInTiles.Equals(position))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }


    }
}
