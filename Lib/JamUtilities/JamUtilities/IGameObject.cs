using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace JamUtilities
{
    public interface IGameObject
    {
        bool IsDead();
        void GetInput();
        void Update(TimeObject timeObject);
        void Draw(RenderWindow rw);
    }
}
