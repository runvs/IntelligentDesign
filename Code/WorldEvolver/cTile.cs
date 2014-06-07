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



        eTileType _tileType;

        public eTileType GetTileType()
        {
            return _tileType;
        }

        // his is just the local time of this tile since the start of the game
        private float _localTime;

        /// <summary>
        /// This refresh Timer will be used to trigger a function to redo the tiles appearance
        /// </summary>
        private float _refreshTimer;
        private float _refreshTimerMax;



        private RectangleShape _tileShape;  // probably an image later?
        private static RectangleShape _dayNightShape;

        private List<TemperatureControlStrategies.cAbstractTemperatureControlStragegy> _temperatureControlList;

        private float _temperatureIntegrationTimer;
        private float _temperatureIntegrationTimerMax;
        // this queue will be used for the integrated temperature calculation
        private Queue<float> _lastTemperaturesList;


        public cTile (Vector2i position, cTileProperties tileProperties, cWorld world)
        {
            _positionInTiles = position;
            _tileProperties = tileProperties;
            _world = world;

            _localTime = 0.0f;

            _refreshTimerMax = (float)RandomGenerator.GetRandomDouble(0.5 ,1.5);
            _refreshTimer = _refreshTimerMax;
            _tileShape = new RectangleShape(new Vector2f(TileSizeInPixels, TileSizeInPixels));
            _tileShape.FillColor = cTileSetter.GetColorFromTileProperties(_tileProperties);

            _dayNightShape = new RectangleShape(new Vector2f(TileSizeInPixels, TileSizeInPixels));
            _dayNightShape.FillColor = new Color(0, 0, 0, 255);


            float centerY = (float)world.GetWorldProperties().WorldSizeInTiles.Y/2.0f;
            float yCoordinate = (float)position.Y;
            //float yFactor = (yCoordinate <= centerY) ? 0.9f + 0.2f/centerY * yCoordinate : 1.3f - 0.2f / centerY * yCoordinate;
            float yFactor = 0.85f +  (float)(0.3 * Math.Sin((float)position.Y/(float)world.GetWorldProperties().WorldSizeInTiles.Y *Math.PI));
            //float yFactor = 1.0f;


            _tileProperties.DayNightCyclePhase = (float)(position.X) / (float)(_world.GetWorldProperties().WorldSizeInTiles.X) * (float)(2.0 * Math.PI) * yFactor;

            _temperatureControlList = new List<TemperatureControlStrategies.cAbstractTemperatureControlStragegy>();
            _temperatureControlList.Add(new TemperatureControlStrategies.cBasicDayNightCycleStrategy(this));
            //_temperatureControlList.Add(new TemperatureControlStrategies.cTemperatureExchangeStrategy(this));

            NeighbourTiles = new List<ITile>();

            _lastTemperaturesList = new Queue<float>(100);
            for (int i = 0; i != 100; i++)
            {
                _lastTemperaturesList.Enqueue(_world.GetWorldProperties().DesiredTemperature);
            }
                _temperatureIntegrationTimerMax = _world.GetWorldProperties().TileTemperatureIntegrationTimer;
            _temperatureIntegrationTimer = _temperatureIntegrationTimerMax;

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

            _temperatureIntegrationTimer -= timeObject.ElapsedGameTime;
            if (_temperatureIntegrationTimer <= 0.0f)
            {
                _temperatureIntegrationTimer = _temperatureIntegrationTimerMax;
                AddTemperatureIntegrationPoint();
            }

            DoTemperatureCalculations(timeObject);


            DoWaterBudgeting(timeObject);



            DoPlantGrowth(timeObject);


        }

        private void DoWaterBudgeting(TimeObject timeObject)
        {
            float nativeFlux = 0.0f;
            if(_tileType == eTileType.TILETYPE_DESERT)
            {
                nativeFlux = -3;
            }
            else if(_tileType == eTileType.TILETYPE_GRASS)
            {
                nativeFlux = -1;
            }
            else if(_tileType == eTileType.TILETYPE_ICE)
            {
                nativeFlux = 0;
            }
            else if(_tileType == eTileType.TILETYPE_MOUNTAIN)
            {
                nativeFlux = -2;
            }
            else if(_tileType == eTileType.TILETYPE_SNOW)
            {
                nativeFlux = 0;
            }
            else if(_tileType == eTileType.TILETYPE_WATER)
            {
                nativeFlux = 10;
            }

            float inFlux = 0.0f;

            foreach (var t in NeighbourTiles)
            {
                if (t.GetTileType() == eTileType.TILETYPE_WATER)
                {
                    //inFlux += 2;
                }
            }

            //float totalWaterChange = inFlux + nativeFlux;

            //GetTileProperties().SummedUpWater += totalWaterChange * timeObject.ElapsedGameTime;

        }

        private void DoPlantGrowth(TimeObject timeObject)
        {
            if (GetTileType() == eTileType.TILETYPE_DESERT || GetTileType() == eTileType.TILETYPE_GRASS)
            {
                if (GetTileProperties().SummedUpWater >= 1)
                {
                    GetTileProperties().SummedUpWater -= 1;
                    GetTileProperties().ChangeFoodAmountOnTile(eFoodType.FOOD_TYPE_PLANT, _world.GetWorldProperties().PlantGrowthRate);
                }
            }
        }

        private void AddTemperatureIntegrationPoint()
        {
            _lastTemperaturesList.Dequeue();
            _lastTemperaturesList.Enqueue(GetTileProperties().TemperatureInKelvin);
            GetTileProperties().IntegratedTemperature = GetIntegratedTemperature();
        }

        public float GetIntegratedTemperature()
        {
            float integratedTemperature = 0.0f;
            foreach (var t in _lastTemperaturesList)
            {
                integratedTemperature += t;
            }
            integratedTemperature /= _lastTemperaturesList.Count;
            return integratedTemperature;
        }


        public void Draw(SFML.Graphics.RenderWindow rw)
        {


            _tileShape.Position = TileSizeInPixels *  new Vector2f(_positionInTiles.X, _positionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            if (_tileShape.Position.X >= -10 && _tileShape.Position.X <= 810)
            {
                if (_tileShape.Position.Y >= -10 && _tileShape.Position.Y <= 610)
                {
                    rw.Draw(_tileShape);

                    
                    if (_world.GetDrawOverlay(cWorld.eWorldDrawOverlay.WORLDDRAWOVERLAY_DAYNIGHT))
                    {
                        _dayNightShape.Position = TileSizeInPixels * new Vector2f(_positionInTiles.X, _positionInTiles.Y) - JamUtilities.Camera.CameraPosition;
                        byte a = (byte)(0.0f + 150.0f * _dayNightTime);

                        _dayNightShape.FillColor = new Color(0, 0, 0, a);

                        rw.Draw(_dayNightShape);
                    }

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
                _tileType = cTileSetter.GetTileTypeFromTileProperties(_tileProperties);
                _tileShape.FillColor = cTileSetter.GetColorFromTileType(_tileType);

                if (GetTileProperties().GetFoodAmountOnTile(eFoodType.FOOD_TYPE_PLANT) >= 1)
                {
                    _tileShape.FillColor = Color.Green;
                }

            }
            else if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_HEIGHT)
            {
                Color newCol;
                float height = GetTileProperties().HeightInMeters;
                float maxHeight = GetWorldProperties().MaxHeightInMeter;
                byte b = (byte)(255.0f * (0.05f + 0.9 * (height / maxHeight)));

                newCol = new Color(b, b, b);

                _tileShape.FillColor = newCol;

            }
            else if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE_CURRENT)
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
            else if (_world.WorldDrawType == cWorld.eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE_INTEGRATED)
            {
                Color newCol;
                float temperature = GetIntegratedTemperature();
                float desiredTemperature = _world.GetWorldProperties().DesiredTemperature;

                if (temperature > desiredTemperature)
                {

                    temperature = temperature - desiredTemperature;
                    temperature *= 5;
                    if (temperature >= 255)
                    {
                        temperature = 255;
                    }
                    byte b = (byte)(255 - temperature);
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
                    newCol = new Color(b, b, 255);
                }
                _tileShape.FillColor = newCol;
            }
        }

        // is between 0 and 1 with 0 being midday and 1 being full midnight
        public float _dayNightTime { get; set; }



    }
}
