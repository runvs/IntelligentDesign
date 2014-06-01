using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;
using SFML.Window;
using WorldEvolver;
using WorldInterfaces;

namespace WorldEvolver
{
    public class cWorld : IWorld, IWorldInCreation, IGameObject
    {
        private System.Collections.Generic.List<ITile> _tileList;

        private cWorldProperties _worldProperties;

        public enum eWorldDrawType
        {
            WORLDDRAWTYPE_NORMAL,
            WORLDDRAWTYPE_TEMPERATURE,
            WORLDDRAWTYPE_HEIGHT
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

            int listID = pos.X + GetWorldProperties().WorldSizeInTiles.X * pos.Y;

            ret = _tileList[listID];

            //foreach (var t in _tileList)
            //{
            //    if (t.GetPositionInTiles().Equals(pos))
            //    {
            //        ret = t;
            //        break;
            //    }
            //}
            return ret;
        }

        public bool IsDead()
        {
            // worlds cannot die
            return false;
        }

        public void GetInput()
        {
            // do nothing now
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F1))
            {
                WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_NORMAL;
            }
            else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F2))
            {
                WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_HEIGHT;
            }
            else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.F3))
            {
                WorldDrawType = eWorldDrawType.WORLDDRAWTYPE_TEMPERATURE;
            }
        }

        public void Update(TimeObject timeObject)
        {
            foreach (var t in _tileList)
            {
                cTile tile = t as cTile;

                tile.Update(timeObject);

            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            foreach (var t in _tileList)
            {
                cTile tile = t as cTile;
                tile.Draw(rw);
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


        public List<ITile> GetTileList()
        {
            return _tileList;
        }
    }
}
