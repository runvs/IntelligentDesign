using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;

namespace WorldInterfaces
{
    public class cWorld : IWorld, IWorldInCreation, IGameObject
    {
        private System.Collections.Generic.List<ITile> _tileList;

        private cWorldProperties _worldProperties;

        public ITile GetTileOnPosition(SFML.Window.Vector2i pos)
        {
            throw new NotImplementedException();
        }

        public bool IsDead()
        {
            // worlds cannot die
            return false;
        }

        public void GetInput()
        {
            throw new NotImplementedException();
        }

        public void Update(TimeObject timeObject)
        {
            throw new NotImplementedException();
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            throw new NotImplementedException();
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
    }
}
