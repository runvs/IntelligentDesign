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

            _desertHeightSlope = (_worldProperties.MountainHeight - 0.0f) / (_worldProperties.DesertGrassTransitionAtMountainHeight - _worldProperties.DesertGrassTransitionAtHeightZero);
            _desertHeightOffset = -_desertHeightSlope * _worldProperties.DesertGrassTransitionAtHeightZero;

            _grassHeightSlope = (_worldProperties.WaterGrassTransitionHeightAtWaterFreezingPoint - 0.0f) / (_worldProperties.WaterFreezingTemperature - _worldProperties.WaterGrassTransitionAtHeightZero);
            _grassHeightOffset = -_grassHeightSlope * _worldProperties.WaterGrassTransitionAtHeightZero;
        }

        private static float _desertHeightOffset;
        private static float _desertHeightSlope;

        private static float _grassHeightOffset;
        private static float _grassHeightSlope;


        public static eTileType GetTileTypeFromTileProperties (cTileProperties properties)
        {
            float tempIntegrated = properties.IntegratedTemperature;
            float tempCurrent = properties.TemperatureInKelvin;
            float tempMean = (tempCurrent + 99.0f * tempIntegrated) / 100.0f;
            float height = properties.HeightInMeters;

            eTileType ret = eTileType.TILETYPE_GRASS;
            if (tempIntegrated <= _worldProperties.WaterFreezingTemperature)
            {

                if (height >= _grassHeightOffset + tempMean * _grassHeightSlope)
                {
                    ret = eTileType.TILETYPE_SNOW;
                }
                else
                {
                    ret = eTileType.TILETYPE_ICE;
                }
            }
            else
            {
                if (height >= _worldProperties.MountainHeight)
                {
                    ret = eTileType.TILETYPE_MOUNTAIN;
                }
                else
                {
                    if (height <= _grassHeightOffset + _grassHeightSlope * tempMean)
                    {
                        ret = eTileType.TILETYPE_WATER;
                    }
                    else if (height >= _desertHeightOffset + _desertHeightSlope * tempMean)
                    {
                        ret = eTileType.TILETYPE_DESERT;
                    }
                    else
                    {
                        ret = eTileType.TILETYPE_GRASS;
                    }

                }
            }

            return ret;
        }


        public static Color GetColorFromTileProperties(cTileProperties properties)
        {
            eTileType type = GetTileTypeFromTileProperties(properties);

            return GetColorFromTileType(type);

        }

        public static Color GetColorFromTileType(eTileType type)
        {
            Color col = new Color(0,0,0);
            if (type == eTileType.TILETYPE_DESERT)
            {
                col = new Color(250, 200, 100);
            }
            else if (type == eTileType.TILETYPE_GRASS)
            {
                col = new Color(100, 255, 120);
            }
            else if (type == eTileType.TILETYPE_ICE)
            {
                col = new Color(180,180,255);
            }
            else if (type == eTileType.TILETYPE_SNOW)
            {
                col = new Color(200,200,230);
            }
            else if (type == eTileType.TILETYPE_WATER)
            {
                col = new Color(50,50,200);
            }
            else if (type == eTileType.TILETYPE_MOUNTAIN)
            {
                col = new Color(100, 100, 100 );
            }

            return col;
        }
    }
}
