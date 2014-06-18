using System;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;

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

        public Vector2i PositionInTiles { get; set; }
        private CircleShape _shape;

        public Animal(AnimalProperties properties, cWorld world)
        {
            CalculateAnimalParameters(properties);
            _shape = new CircleShape(cTile.GetTileSizeInPixelStatic() / 2.0f);
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
            //throw new NotImplementedException();
        }

        public void Draw(RenderWindow rw)
        {
            _shape.Position = cTile.GetTileSizeInPixelStatic() * new Vector2f(PositionInTiles.X, PositionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            rw.Draw(_shape);
        }

        private void CalculateAnimalParameters(AnimalProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}
