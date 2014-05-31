using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldInterfaces
{
    public interface IWorldInCreation
    {
        void AddTille(ITile tile);
        void SetWorldProperties(cWorldProperties properties);

        void BuildTileNeighbourLists();

        List<ITile> GetTileList();
    }
}
