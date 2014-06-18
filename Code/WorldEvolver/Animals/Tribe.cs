using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using WorldInterfaces;

namespace WorldEvolver
{
    public class Tribe : IGameObject
    {

        private List<Animal> _animalList;
        private cWorld _world;

        public Tribe (cWorld world)
        {
            _world = world;
            _animalList = new List<Animal>();
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


    }
}
