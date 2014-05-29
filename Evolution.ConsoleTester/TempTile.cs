using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;

using WorldInterfaces;

namespace Evolution.ConsoleTester
{
    public class TempTile : ITile
    {
        public cTileProperties GetTileProperties()
        {
            cTileProperties prop = new cTileProperties();
            prop.TemperatureInKelvin = 300;
            return prop;
        }
        public Vector2i GetPositionInTiles()
        {
            return new Vector2i(0,0);
        }

    }
}
