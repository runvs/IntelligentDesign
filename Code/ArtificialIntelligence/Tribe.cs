using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Window;
using WorldInterfaces;

namespace ArtificialIntelligence
{
    public class Tribe : IGameObject
    {

        private List<Animal> _animalList;
        private IWorld _world;
        private AnimalProperties _properties;

        private Vector2i PositionInTiles { get; set; }

        public Tribe(IWorld world, AnimalProperties properties)
        {
            if (world == null)
            {
                throw new ArgumentNullException("IWorld world", "could not resolve world in Tribe Constructor.");
            }
            _world = world;
            _animalList = new List<Animal>();
            _properties = properties;

            PositionInTiles = new Vector2i(10, 10);
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
            foreach (IGameObject a in _animalList)
            {
                a.Update(timeObject);
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            foreach (IGameObject a in _animalList)
            {
                a.Draw(rw);
            }
        }

        public void SpawnAninal()
        {
            Vector2i initialPosition = PositionInTiles + RandomGenerator.GetRandomVector2iInRect(new SFML.Graphics.IntRect(-5,-5, 10, 10));
            Animal animal = new Animal(_properties, _world, initialPosition);
            _animalList.Add(animal);
        }


    }
}
