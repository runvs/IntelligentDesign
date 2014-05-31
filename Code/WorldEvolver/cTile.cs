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
        private cTileProperties _tileProperties;
        private cWorld _world;

        public List<ITile> NeighbourTiles {get; private set;}
        
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

        private List<TemperatureControlStrategies.cAbstractTemperatureControlStragegy> _temperatureControlList;




        public cTile (Vector2i position, cTileProperties tileProperties, cWorld world)
        {
            _positionInTiles = position;
            _tileProperties = tileProperties;
            _world = world;

            _localTime = 0.0f;

            _refreshTimerMax = (float)RandomGenerator.GetRandomDouble(0.0 ,0.01);
            _refreshTimer = _refreshTimerMax;
            _tileShape = new RectangleShape(new Vector2f(TileSizeInPixels, TileSizeInPixels));
            _tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_tileProperties);

            _tileProperties.DayNightCyclePhase = (float)(position.X)/(float)(_world.GetWorldProperties().WorldSizeInTiles.X)  * (float)(2.0 * Math.PI);

            _temperatureControlList = new List<TemperatureControlStrategies.cAbstractTemperatureControlStragegy>();
            _temperatureControlList.Add(new TemperatureControlStrategies.cBasicDayNightCycleStrategy(this));
            _temperatureControlList.Add(new TemperatureControlStrategies.cTemperatureExchangeStrategy(this));

            NeighbourTiles = new List<ITile>();
        }

        /// <summary>
        /// This Method fills the _neighbourTiles List with the respective Tiles
        /// It should be called when _world is completely created.
        /// </summary>
        public void BuildNeighbourTiles ()
        {
            Vector2i otherTilePos = _positionInTiles + new Vector2i(1,0);
            NeighbourTiles.Add(_world.GetTileOnPosition(otherTilePos));

            otherTilePos = _positionInTiles + new Vector2i(-1,0);
            NeighbourTiles.Add(_world.GetTileOnPosition(otherTilePos));

            otherTilePos = _positionInTiles + new Vector2i(0, 1);
            NeighbourTiles.Add(_world.GetTileOnPosition(otherTilePos));

            otherTilePos = _positionInTiles + new Vector2i(0, -1);
            NeighbourTiles.Add(_world.GetTileOnPosition(otherTilePos));
        }
        


        public cTileProperties GetTileProperties()
        {
            return _tileProperties;
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

            

            //System.Console.WriteLine(GetTileProperties().TemperatureInKelvin);
            DoTemperatureCalculations(timeObject);



        }


        public void Draw(SFML.Graphics.RenderWindow rw)
        {


            _tileShape.Position = TileSizeInPixels *  new Vector2f(_positionInTiles.X, _positionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            if (_tileShape.Position.X >= -10 && _tileShape.Position.X <= 810)
            {
                if (_tileShape.Position.Y >= -10 && _tileShape.Position.Y <= 610)
                {
                    rw.Draw(_tileShape);
                }
            }
            
        }

        public cWorldProperties GetWorldProperties() { return _world.GetWorldProperties(); }

        public float GetLocalTime() { return _localTime; }

        public float GetTileSizeInPixel()
        {
            return TileSizeInPixels;
        }

        public static float GetTileSizeInPixelStatic()
        {
            return TileSizeInPixels;
        }

        


        private void DoTemperatureCalculations(TimeObject timeObject)
        {
            foreach (var t in _temperatureControlList)
            {
                t.Update(timeObject);
            }
            foreach (var t in _temperatureControlList)
            {
                t.DoPostUpdate(timeObject);
            }
        }

        public void ResetTileAppearance()
        {
            if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_NORMAL)
            {
                _tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_tileProperties);
            }
            else if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_HEIGHT)
            {
                //_tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_tileProperties);
            }
            else if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE)
            {
                
                Color newCol;
                float temperature = GetTileProperties().TemperatureInKelvin;
                float desiredTemperature = _world.GetWorldProperties().DesiredTemperature;
                
                if (temperature > desiredTemperature)
                {

                    temperature = temperature - desiredTemperature;
                    temperature *= 5;
                    if (temperature >= 255)
                    {
                        temperature = 255;
                    }
                    byte b =(byte) ( 255 - temperature);
                     newCol = new Color(255, b, b);
                }
                else
                {
                    temperature = desiredTemperature - temperature;
                    temperature *= 5;
                    if (temperature >= 255)
                    {
                        temperature = 255;
                    }
                    byte b = (byte)(255 - temperature);
                    newCol = new Color(b , b,  255);
                }
                _tileShape.FillColor = newCol;
            }
        }
    }
}
