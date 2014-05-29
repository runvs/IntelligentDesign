using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace WorldInterfaces
{
    public interface ITile
    {
        cTileProperties GetTileProperties();
        Vector2i GetPositionInTiles(); 
    }
}
