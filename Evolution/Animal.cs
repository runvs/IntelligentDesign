using System;
using System.IO;
using SFML.Graphics;
using JamUtilities;

using SFML.Window;

using WorldInterfaces;

namespace Evolution
{
    public class Animal : IGameObject
    {
        public static uint MaxId = 0;
        public uint Id;

        private IWorld _world;
        public DNA DNA;
        public double Rank { get; private set; }

        #region Phenotype
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public double PreferredTemperature { get; private set; } 
        #endregion

        public void UpdatePhenotype()
        {
            MaxHealth = 100;
            PreferredTemperature = 273 + DNA.Gen[DNA.GenType.TEMPERATURE] * 100;
        }

        public double GetFitness(){
            return CurrentHealth;
        }

        public double SetRank(double lastRank)
        {
            Rank = lastRank + GetFitness();
            return Rank;
        }

        public void Save(StreamWriter stream)
        {
            stream.WriteLine("{0:000} {1:000}", Id, PreferredTemperature);
        }

        public Animal(IWorld world)
        {
            Id = MaxId;
            MaxId++;
            _world = world;
            DNA = new DNA();
            UpdatePhenotype();
            CurrentHealth = MaxHealth;
        }
        public Animal(IWorld world, Animal A, Animal B)
        {
            Id = MaxId;
            MaxId++;
            _world = world;
            DNA = new DNA(A.DNA, B.DNA);
            UpdatePhenotype();
            CurrentHealth = MaxHealth;
        }
        #region IGameObject
        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }
        public void GetInput()
        {

        }
        public void Update(TimeObject timeObject)
        {
            CurrentHealth -= (int) (10 + 0.5*Math.Abs(_world.GetTileOnPosition(new Vector2i(0,0)).GetTileProperties().TemperatureInKelvin-PreferredTemperature));
        }
        public void Draw(RenderWindow rw)
        {

        }
        #endregion
    }
}
