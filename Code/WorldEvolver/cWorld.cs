﻿using System;
using System.Collections.Generic;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace WorldEvolver
{
    public class cWorld : IWorld, IWorldInCreation, IGameObject
    {
        private List<ITile> _tileList;
        private List<cCloud> _cloudList;

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

            int listID = pos.Y + GetWorldProperties().WorldSizeInTiles.Y * pos.X;

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

            foreach (cTile t in _tileList)
            {
                t.Update(timeObject);
            }

            CloudUpdate(timeObject);
        }

        private void CloudUpdate(TimeObject timeObject)
        {
            List<cCloud> newList = new List<cCloud>(_cloudList.Count);
            foreach (var c in _cloudList)
            {
                c.Update(timeObject);
                if (c.IsRaining)
                {
                    int range = (int)(c.GetCloudSize());
                    for (int i = -range; i != range; ++i)
                    {
                        for (int j = -range; j != range; ++j)
                        {
                            if (i * i + j * j <= range * range)
                            {
                                if (RandomGenerator.Random.NextDouble() <= 0.85)
                                {
                                    Vector2i newPos = c.PositionInTiles + new Vector2i(i, j);

                                    GetTileOnPosition(newPos).GetTileProperties().SummedUpWater += _worldProperties.RainWaterAmount * timeObject.ElapsedGameTime;
                                    c.ReduceWaterAmount(_worldProperties.RainWaterAmount * timeObject.ElapsedGameTime);
                                }
                            }

                        }
                    }
                }

                if (!c.IsDead())
                {
                    newList.Add(c);
                }

            }
            _cloudList = newList;

            float numberOfTilesToCheck = GetWorldProperties().WorldSizeInTiles.X * GetWorldProperties().WorldSizeInTiles.Y * 0.005f;
            for (int i = 0; i <= numberOfTilesToCheck; i++)
            {
                int id = RandomGenerator.Random.Next(_tileList.Count);
                if (_tileList[id].GetTileType() == eTileType.TILETYPE_WATER)
                {
                    if (_tileList[id].GetTileProperties().TemperatureInKelvin > GetWorldProperties().DesertGrassTransitionAtHeightZero)
                    {
                        if (RandomGenerator.Random.NextDouble() <= 0.01)
                        {
                            cCloud newCloud = new cCloud(this, _tileList[id].GetPositionInTiles());
                            _cloudList.Add(newCloud);
                        }
                    }
                }
            }
        }

        public void Draw(RenderWindow rw)
        {
            foreach (cTile t in _tileList)
            {
                t.Draw(rw);
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


        public void CreateClouds()
        {
            _cloudList = new List<cCloud>();

            for (int i = 0; i != _worldProperties.CloudNumber; i++)
            {
                _cloudList.Add(
                    new cCloud(this,
                        new Vector2i(16, 8)));
            }
        }

  
    }
}
