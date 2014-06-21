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
        private AnimalProperties _properties;

        public Vector2i PositionInTiles { get; set; }

        private Color _tribeColor;

        public Tribe(IWorld world, AnimalProperties properties)
        {
            if (world == null)
            {
                throw new ArgumentNullException("IWorld world", "could not resolve world in Tribe Constructor.");
            }
            _world = world;
            _animalList = new List<Animal>();
            _properties = properties;

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

        public void SpawnAnimal()
        {
            int count = _animalList.Count;
            int tribeSize = (int)(4.0f * (float)(Math.Sqrt(_animalList.Count)));
            int halfsize = -(tribeSize / 2) +1 ;

            Vector2i initialPosition = PositionInTiles + RandomGenerator.GetRandomVector2iInRect(new SFML.Graphics.IntRect(halfsize, halfsize, tribeSize, tribeSize));
            Animal animal = new Animal(_properties, _world, initialPosition);
            animal.AnimalColor = _tribeColor;
            _animalList.Add(animal);
        }


    }
}
