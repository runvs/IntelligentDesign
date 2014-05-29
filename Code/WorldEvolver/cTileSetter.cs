using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using WorldInterfaces;

namespace WorldEvolver
{
    public static class cTileSetter
    {
        private static cWorldProperties _worldProperties;
        public static void SetWorldProperties (cWorldProperties worldProperties)
        {
            _worldProperties = worldProperties;
        }


        public static Color GetColorFromTileProperties(cTileProperties properties)
        {
            Color col;

            if (properties.HeightInMeters >= _worldProperties.WaterLevelInMeter)
            {
                col = new Color(100, 255, 120);
            }
            else
            {
                col = new Color(50, 50, 200);
            }

            return col;
        }
    }
}
