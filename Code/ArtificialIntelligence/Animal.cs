using System;
using ArtificialIntelligence.Intelligence;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace ArtificialIntelligence
{
    public class Animal : IGameObject
    {
        private IWorld _world;
        public Tribe Tribe { get; private set; }

        public float HealthMax { get; private set; }
        public float HealthCurrent { get; private set; }
        public float HealthRegeneration { get; private set; }

        public float MoveTimerMax { get; private set; }

        public float Damage { get; private set; }

        public AnimalProperties.DietType Diet { get; private set; }
        public float Hunger { get; private set; }

        public AnimalProperties.TerrainType PreferredTerrain { get; private set; }
        public float PreferredTemperature { get; private set; }
        public float PreferredAltitude { get; private set; }
        public float GroupBehaviour { get; private set; }





        public Vector2i PositionInTiles { get; set; }
        private CircleShape _shape;

        public static float TileSizeInPixels { get; set; }

        private AbstractIntelligencePattern _intelligence;

        public Color AnimalColor { set { _shape.FillColor = value; } }


        public Animal(AnimalProperties properties, IWorld world, Tribe tribe, Vector2i initialPosition)
        {
            _world = world;
            Tribe = tribe;
            _shape = new CircleShape(TileSizeInPixels / 2.0f - 1.0f);
            PositionInTiles = initialPosition;

            CalculateAnimalParameters(properties);

            _intelligence = new AnimalAI(this);
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
            _intelligence.DoIntelligenceUpdate(timeObject);
        }

        public void Draw(RenderWindow rw)
        {
            _shape.Position = TileSizeInPixels * new Vector2f(PositionInTiles.X, PositionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            rw.Draw(_shape);
        }

        public void Move(Direction direction)
        {
            PositionInTiles += direction.DirectionToVector();
        }

        private void CalculateAnimalParameters(AnimalProperties prop)
        {
            HealthMax = prop.Stamina * 10;
            HealthCurrent = HealthMax;
            HealthRegeneration = prop.Stamina * 0.5f;

            MoveTimerMax = 12.0f / (prop.Agility + 1.0f);
            Console.WriteLine("Move Timer is " + MoveTimerMax);
            Hunger = (prop.Strength + prop.Stamina) * 0.5f;

            PreferredAltitude = prop.PreferredAltitude;
            PreferredTerrain = prop.PreferredTerrain;
            PreferredTemperature = prop.PreferredTemperature;

            GroupBehaviour = prop.GroupBehaviour / 10.0f - 0.1f;
        }
    }
}
