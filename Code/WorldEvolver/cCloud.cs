using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;

namespace WorldEvolver
{
    public class cCloud : IGameObject
    {


        private CircleShape _shape;

        public Vector2i PositionInTiles { get; private set; }

        cWorld _world;

        float _rainFrequency;
        float _rainOffset;
        float _totalTime;
        public bool IsRaining { get; private set; }

        public float _waterAmount { get; private set; }

        private float _cloudSize;


        public cCloud (cWorld world, Vector2i position )
        {
            _world = world;
            PositionInTiles = position;

            _totalTime = 0.0f;

            _rainFrequency = (float)RandomGenerator.GetRandomDouble(5.0, 10.0);
            _rainOffset = (float)RandomGenerator.GetRandomDouble(0.0, 2.0*Math.PI); ;

            _cloudSize = 2;

            _shape = new CircleShape(cTile.TileSizeInPixels * _cloudSize);
            _shape.Origin = new Vector2f(cTile.TileSizeInPixels * _cloudSize, cTile.TileSizeInPixels * _cloudSize);

            _waterAmount = _world.GetWorldProperties().RainWaterAmount * 50;
        }



        public bool IsDead()
        {
            if (_waterAmount <= 0)
            {
                return true;
            }
            return false;   // at least now clouds cannot die 
        }

        public void GetInput()
        {
            // no input for clouds
        }

        public void Update(TimeObject timeObject)
        {
            _totalTime += timeObject.ElapsedGameTime;
            IsRaining = (Math.Sin(_rainOffset + _rainFrequency * _totalTime) > 0);
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            _shape.Position = new Vector2f(PositionInTiles.X * cTile.TileSizeInPixels, PositionInTiles.Y * cTile.TileSizeInPixels) - Camera.CameraPosition;
            rw.Draw(_shape);
        }


        internal float GetCloudSize()
        {
            return _cloudSize;
        }

        internal void ReduceWaterAmount(float p)
        {
            _waterAmount -= p;

        }
    }
}
