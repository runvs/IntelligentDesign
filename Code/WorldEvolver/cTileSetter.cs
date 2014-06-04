using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using WorldInterfaces;

namespace WorldEvolver
{
    /// <summary>
    ///  This Class serves as a Phase Diagramm Coordinator for all the tiles.
    ///  It will check which TileType a Tile has when it is in a certain state
    ///  All derived properties (how will a tile look like and so on) will be availible from methods in this class
    /// </summary>
    public static class cTileSetter
    {
        private static cWorldProperties _worldProperties;
        public static void SetWorldProperties (cWorldProperties worldProperties)
        {
            _worldProperties = worldProperties;

            //Console.WriteLine(_worldProperties.DesiredTemperature);

            _desertHeightOffset = -_worldProperties.DesertTemperatureStart * _worldProperties.DesertSlope;
        }

        private static float _desertHeightOffset;

        public static cTile.eTileType GetTileTypeFromTileProperties (cTileProperties properties)
        {
            float tempIntegrated = properties.IntegratedTemperature;
            float tempCurrent = properties.TemperatureInKelvin;
            float tempMean = (tempCurrent + 19.0f * tempIntegrated) / 20.0f;
            float height = properties.HeightInMeters;

            cTile.eTileType ret = cTile.eTileType.TILETYPE_GRASS;
            if (tempIntegrated >= _worldProperties.DesertTemperatureStart)
            {
                
                if (height >= _worldProperties.MountainHeight)
                {
                    ret = cTile.eTileType.TILETYPE_MOUNTAIN;
                }
                else
                {
                    if (height >= (_desertHeightOffset + tempMean * _worldProperties.DesertSlope))
                    {
                        ret = cTile.eTileType.TILETYPE_GRASS;
                    }
                    else 
                    {
                        ret = cTile.eTileType.TILETYPE_DESERT;
                    }
                }
            }
            else if (properties.IntegratedTemperature >= _worldProperties.WaterFreezingTemperature)
            {
                if (properties.HeightInMeters >= _worldProperties.MountainHeight)
                {
                    ret = cTile.eTileType.TILETYPE_MOUNTAIN;
                }
                else
                {
                    if (properties.HeightInMeters > (_worldProperties.WaterHeightOffset + _worldProperties.WaterSlope * tempMean))
                    {
                        ret = cTile.eTileType.TILETYPE_GRASS;
                    }
                    else
                    {
                        ret = cTile.eTileType.TILETYPE_WATER;
                    }
                }
            }
            else
            {
                if (properties.HeightInMeters > (_worldProperties.WaterHeightOffset + _worldProperties.WaterSlope * tempMean))
                {
                    ret = cTile.eTileType.TILETYPE_SNOW;
                }
                else
                {
                    ret = cTile.eTileType.TILETYPE_ICE;
                }
            }

            return ret;
        }


        public static Color GetColorFromTileProperties(cTileProperties properties)
        {
            cTile.eTileType type = GetTileTypeFromTileProperties(properties);

            return GetColorFromTileType(type);

        }

        private static Color GetColorFromTileType(cTile.eTileType type)
        {
            Color col = new Color(0,0,0);
            if (type == cTile.eTileType.TILETYPE_DESERT)
            {
                col = new Color(250, 200, 100);
            }
            else if (type == cTile.eTileType.TILETYPE_GRASS)
            {
                col = new Color(100, 255, 120);
            }
            else if (type == cTile.eTileType.TILETYPE_ICE)
            {
                col = new Color(180,180,255);
            }
            else if (type == cTile.eTileType.TILETYPE_SNOW)
            {
                col = new Color(200,200,230);
            }
            else if (type == cTile.eTileType.TILETYPE_WATER)
            {
                col = new Color(50,50,200);
            }
            else if (type == cTile.eTileType.TILETYPE_MOUNTAIN)
            {
                col = new Color(100, 100, 100 );
            }

            return col;
        }
    }
}
