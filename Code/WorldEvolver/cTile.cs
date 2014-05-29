using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace WorldEvolver
{
    public class cTile: ITile, IGameObject
    {
        private cTileProperties _properties;
        private Vector2i _positionInTiles;

        public static float TileSizeInPixels { get; set; }

        // his is just the local time of this tile since the start of the game
        private float _localTime;

        /// <summary>
        /// This refresh Timer will be used to trigger a function to redo the tiles appearance
        /// </summary>
        private float _refreshTimer;
        private float _refreshTimerMax;

        private RectangleShape _tileShape;  // probably an image later?


        public cTile (Vector2i position, cTileProperties properties)
        {
            _positionInTiles = position;
            _properties = properties;

            _localTime = 0.0f;

            _refreshTimerMax = (float)RandomGenerator.GetRandomDouble(4.0 ,6.0);
            _refreshTimer = _refreshTimerMax;
            _tileShape = new RectangleShape(new Vector2f(TileSizeInPixels, TileSizeInPixels));
            _tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_properties);
        }

        public void ResetTileAppearance ()
        {
            _tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_properties);
        }


        public cTileProperties GetTileProperties()
        {
            return _properties;
        }

        public SFML.Window.Vector2i GetPositionInTiles()
        {
            return _positionInTiles;
        }

        public bool IsDead()
        {
            // tiles cannot die
            return false;
        }

        public void GetInput()
        {
            throw new NotImplementedException();
        }

        public void Update(TimeObject timeObject)
        {
            _localTime += timeObject.ElapsedGameTime;
            _refreshTimer -= timeObject.ElapsedGameTime;
            if (_refreshTimer <= 0.0f)
            {
                _refreshTimer = _refreshTimerMax;
                ResetTileAppearance();
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            _tileShape.Position = TileSizeInPixels *  new Vector2f(_positionInTiles.X, _positionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            rw.Draw(_tileShape);
        }



        public float GetTileSizeInPixel()
        {
            return TileSizeInPixels;
        }

        public static float GetTileSizeInPixelStatic()
        {
            return TileSizeInPixels;
        }


    }
}
