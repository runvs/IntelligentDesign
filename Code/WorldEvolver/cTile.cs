using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;
using SFML.Window;
using WorldInterfaces;

namespace WorldEvolver
{
    class cTile: ITile, IGameObject
    {
        private cTileProperties _properties;
        private Vector2i _positionInTiles;

        public cTileProperties GetTileProperties()
        {
            return _properties;
        }

        public SFML.Window.Vector2i GetPositionInTiles()
        {
            return _positionInTiles;
        }

        public bool IsDead()
        {
            // tiles cannot die
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
    }
}
