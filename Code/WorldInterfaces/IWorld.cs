﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace WorldInterfaces
{
    public interface IWorld
    {
        ITile GetTileOnPosition(Vector2i pos);

        cWorldProperties GetWorldProperties();
    }
}
