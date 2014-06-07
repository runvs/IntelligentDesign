using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;
using SFML.Window;
using WorldEvolver;
using WorldInterfaces;

namespace WorldEvolver
{
    public class cWorld : IWorld, IWorldInCreation, IGameObject
    {
        private System.Collections.Generic.List<ITile> _tileList;
        private System.Collections.Generic.List<cCloud> _cloudList;

        private cWorldProperties _worldProperties;

        public enum eWorldDrawType
        {
            WORLDDRAWTYPE_NORMAL,
            WORLDDRAWTYPE_TEMPERATURE_CURRENT,
            WORLDDRAWTYPE_TEMPERATURE_INTEGRATED,
            WORLDDRAWTYPE_HEIGHT
        }

        public enum eWorldDrawOverlay
        {
            WORLDDRAWOVERLAY_CLOUDS,
            WORLDDRAWOVERLAY_DAYNIGHT
        }

        Dictionary<eWorldDrawOverlay, bool> _overlayDictionary;
        private float _inputWallTime;

        public bool GetDrawOverlay(eWorldDrawOverlay overlay)
        {
            return _overlayDictionary[overlay];
        }

        public eWorldDrawType WorldDrawType { get; private set; }

        public ITile GetTileOnPosition(SFML.Window.Vector2i pos)
        {
            ITile ret = null;

            // wrap Position for periodic boundaries
            while (pos.X < 0)
            {
                pos.X += _worldProperties.WorldSizeInTiles.X;
            }
            while (pos.X >= _worldProperties.WorldSizeInTiles.X)
            {
                pos.X -= _worldProperties.WorldSizeInTiles.X;
            }

            while (pos.Y < 0)
            {
                pos.Y += _worldProperties.WorldSizeInTiles.Y;
            }
            while (pos.Y >= _worldProperties.WorldSizeInTiles.Y)
            {
                pos.Y -= _worldProperties.WorldSizeInTiles.Y;
            }

            int listID = pos.X + GetWorldProperties().WorldSizeInTiles.X * pos.Y;

            ret = _tileList[listID];
            return ret;
        }

        public bool IsDead()
        {
            // worlds cannot die
            return false;
        }

        public void GetInput()
        {
            if (_inputWallTime <= 0.0f)
            {
              
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F1))
                {
                    WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_NORMAL;
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F2))
                {
                    WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_HEIGHT;
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F3))
                {
                    WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE_CURRENT;
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F4))
                {
                    WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE_INTEGRATED;
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F5))
                {
                    _overlayDictionary[eWorldDrawOverlay.WORLDDRAWOVERLAY_CLOUDS] = !_overlayDictionary[eWorldDrawOverlay.WORLDDRAWOVERLAY_CLOUDS];
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F6))
                {
                    _overlayDictionary[eWorldDrawOverlay.WORLDDRAWOVERLAY_DAYNIGHT] = !_overlayDictionary[eWorldDrawOverlay.WORLDDRAWOVERLAY_DAYNIGHT];
                    _inputWallTime = 0.5f;
                }


                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.A))
                {

                    _worldProperties.SunLightIntensityFactor += 0.05f;
                    if (_worldProperties.SunLightIntensityFactor >= 1.0f)
                    {
                        _worldProperties.SunLightIntensityFactor = 1.0f;
                    }
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.D))
                {
                    _worldProperties.SunLightIntensityFactor -= 0.05f;
                    if (_worldProperties.SunLightIntensityFactor <= 0.0f)
                    {
                        _worldProperties.SunLightIntensityFactor = 0.0f;
                    }
                    _inputWallTime = 0.5f;
                }


                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))
                {
                    _worldProperties.DayNightCycleFrequency += 0.05f;
                    _inputWallTime = 0.5f;
                }
                else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))
                {
                    _worldProperties.DayNightCycleFrequency -= 0.05f;
                    if (_worldProperties.DayNightCycleFrequency <= 0.0f)
                    {
                        _worldProperties.DayNightCycleFrequency = 0.00001f;
                    }
                    _inputWallTime = 0.5f;
                }
            }

        }

        public void Update(TimeObject timeObject)
        {
            //Console.WriteLine(_inputWallTime);
            if (_inputWallTime > 0)
            {
                _inputWallTime -= timeObject.ElapsedRealTime;
            }

            foreach (var t in _tileList)
            {
                cTile tile = t as cTile;

                tile.Update(timeObject);

            }

            foreach (var c in _cloudList)
            {
                c.Update(timeObject);
                if (c.IsRaining)
                {
                    int range = (int)(c.GetCloudSize() / 2.0f);
                    for (int i = -range; i != range; ++i)
                    {
                        for (int j = -range; j != range; ++j)
                        {
                            GetTileOnPosition(c.PositionInTiles + new Vector2i(i, j)).GetTileProperties().ChangeFoodAmountOnTile(eFoodType.FOOD_TYPE_PLANT, _worldProperties.PlantGrowthRate * timeObject.ElapsedGameTime);
                        }
                    }
                }
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            foreach (var t in _tileList)
            {
                cTile tile = t as cTile;
                tile.Draw(rw);
            }

            if (GetDrawOverlay(eWorldDrawOverlay.WORLDDRAWOVERLAY_CLOUDS))
            {
                foreach (var c in _cloudList)
                {
                    c.Draw(rw);
                }
            }
        }

        public void AddTille(ITile tile)
        {
            if (tile != null)
            {
                _tileList.Add(tile);
            }
        }
        
        public cWorldProperties GetWorldProperties()
        {
            return _worldProperties;
        }


        public void SetWorldProperties(cWorldProperties properties)
        {
            _worldProperties = properties;
            _tileList = new List<ITile>(properties.WorldSizeInTiles.X * properties.WorldSizeInTiles.Y);

            _overlayDictionary = new Dictionary<eWorldDrawOverlay, bool>();
            foreach (eWorldDrawOverlay e in Enum.GetValues(typeof(eWorldDrawOverlay)))
            {
                _overlayDictionary.Add(e, false);
            }
        }


        public void BuildTileNeighbourLists()
        {
            //foreach (cTile t in _tileList)
            int runner = 0;
            int percentage = 0;
            int fifePercent = (int)((float)_tileList.Count * 0.05f);
            Console.Write("Finished percentage of all Tiles: ");
            for (int i = 0; i != _tileList.Count; i++)
            {
                runner++;
                if (runner >= fifePercent)
                {
                    percentage += 5;
                    runner = 0;
                    Console.Write("" + percentage + "%  ");
                }
                cTile t = _tileList[i] as cTile;
                t.BuildNeighbourTiles();
            }
        }


        //public List<ITile> GetTileList()
        //{
        //    return _tileList;
        //}


        public void CreateClouds()
        {
            _cloudList = new List<cCloud>();

            for (int i = 0; i != _worldProperties.CloudNumber; i++)
            {
                _cloudList.Add(
                    new cCloud(this, 
                        RandomGenerator.GetRandomVector2iInRect(new SFML.Graphics.IntRect(0, 0, 
                            _worldProperties.WorldSizeInTiles.X, _worldProperties.WorldSizeInTiles.Y))));
            }
        }
    }
}
