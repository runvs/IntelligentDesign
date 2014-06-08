using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

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

        public float WaterAmount { get; private set; }
        public float WaterAmountMax { get; private set; }

        private float _cloudSize;

        private float _moveTimer;
        private float _moveTimerMax;
        private Vector2i _moveDirection;

        public cCloud (cWorld world, Vector2i position )
        {
            _world = world;
            PositionInTiles = position;

            _totalTime = 0.0f;

            _rainFrequency = (float)RandomGenerator.GetRandomDouble(0.5, 1.50);
            _rainOffset = (float)RandomGenerator.GetRandomDouble(0.0, 2.0*Math.PI); ;

            _cloudSize = 4;

            _shape = new CircleShape(cTile.TileSizeInPixels * _cloudSize);
            _shape.Origin = new Vector2f(cTile.TileSizeInPixels * _cloudSize, cTile.TileSizeInPixels * _cloudSize);

            WaterAmount = _world.GetWorldProperties().RainWaterAmount * 800;
            WaterAmountMax = WaterAmount;
            ReduceWaterAmount(0.0f);

            _moveTimerMax = 2.0f;
            _moveTimer = _moveTimerMax;
            _moveDirection = RandomGenerator.GetRandomVector2iInRect(new IntRect(0,0,1,1));
        }



        public bool IsDead()
        {
            if (WaterAmount <= 0)
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
            _moveTimer -= timeObject.ElapsedGameTime;
            if (_moveTimer <= 0)
            {
                _moveTimerMax = GetMoveTimerOnTileProperties(_world.GetTileOnPosition(PositionInTiles).GetTileProperties());
                _moveTimer = _moveTimerMax;
                PositionInTiles += _moveDirection;
            }

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
            WaterAmount -= p;
            Color col = _shape.FillColor;
            col.A = (byte)(255.0f * (WaterAmount / WaterAmountMax * 0.6f + 0.2f));
            _shape.FillColor = col;
        }

        public static float GetMoveTimerOnTileProperties (cTileProperties properties)
        {
            float tempCurrent = properties.TemperatureInKelvin;

            float ret = 2.25f;
            float slope = - 4.0f/60.0f;
            float xOffset = 2.25f - 300.0f* slope;

            if(tempCurrent <= 270)
            {
                ret = 3.0f;
            }
            else if (tempCurrent >= 330)
            {
                ret = 1.0f;
            }
            else
            {
                ret = xOffset  + slope * tempCurrent;
            }

            return ret;
        }
    }
}
