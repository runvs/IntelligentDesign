using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;
using WorldInterfaces;

namespace Evolution.ConsoleTester
{
    public class TempWorld : IWorld
    {
        public ITile GetTileOnPosition(Vector2i pos)
        {
            return new TempTile();
        }

        public cWorldProperties GetWorldProperties()
        {
            return new cWorldProperties();
        }
    }
}
