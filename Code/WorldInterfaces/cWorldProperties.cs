using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace WorldInterfaces
{
    public class cWorldProperties
    {
        public Vector2i WorldSizeInTiles {get; set;}

        public float HeightMapNoiseFrequency { get; set; }

        public float MaxHeightInMeter { get; set; }

        public float WaterLevelInMeter { get; set; }
    }
}
