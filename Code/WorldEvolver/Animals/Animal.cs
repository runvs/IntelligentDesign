using System;
using JamUtilities;
using SFML.Graphics;

namespace WorldEvolver
{
    public class Animal : IGameObject
    {
        private cWorld _world;

        public float HealthMax { get; private set; }
        public float HealthCurrent { get; private set; }
        public float HealthRegeneration { get; private set; }
        public float MoveSpeed { get; private set; }
        public float Damage { get; private set; }
        public float Hunger { get; private set; }
        public float PreferredTemperature { get; private set; }
        public float PreferredAltitude { get; private set; }

        public Animal(AnimalProperties properties, cWorld world)
        {
            CalculateAnimalParameters(properties);
        }

        public bool IsDead()
        {
            throw new NotImplementedException();
        }

        public void GetInput()
        {
            throw new NotImplementedException();
        }

        public void Update(TimeObject timeObject)
        {
            throw new NotImplementedException();
        }

        public void Draw(RenderWindow rw)
        {
            throw new NotImplementedException();
        }

        private void CalculateAnimalParameters(AnimalProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}
