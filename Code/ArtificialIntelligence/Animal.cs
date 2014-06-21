using System;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace ArtificialIntelligence
{
    public class Animal : IGameObject
    {
        private IWorld _world;

        public float HealthMax { get; private set; }
        public float HealthCurrent { get; private set; }
        public float HealthRegeneration { get; private set; }

        public float MoveSpeed { get; private set; }

        public float Damage { get; private set; }

        public AnimalProperties.DietType Diet { get; private set; }
        public float Hunger { get; private set; }

        public AnimalProperties.TerrainType PreferredTerrain { get; private set; }
        public float PreferredTemperature { get; private set; }
        public float PreferredAltitude { get; private set; }

        public Vector2i PositionInTiles { get; set; }
        private CircleShape _shape;

        public static float TileSizeInPixels { get; set; }

        public Animal(AnimalProperties properties, IWorld world, Vector2i initialPosition)
        {
            _world = world;
            _shape = new CircleShape(TileSizeInPixels / 2.0f);
            PositionInTiles = initialPosition;

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

            MoveSpeed = 1 / (prop.Agility + 1.0f);
            Hunger = (prop.Strength + prop.Stamina) * 0.5f;

            PreferredAltitude = prop.PreferredAltitude;
            PreferredTerrain = prop.PreferredTerrain;
            PreferredTemperature = prop.PreferredTemperature;
        }
    }
}
