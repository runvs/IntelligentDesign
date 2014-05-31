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

        public float HeightMapNoiseLength { get; set; }

        public float MaxHeightInMeter { get; set; }

        public float WaterLevelInMeter { get; set; }

        public float SunHeatInfluxPerSecond { get; set; }

        public float AtmosphericHeatOutFluxPerSecond { get; set; }

        public float DesiredTemperature { get { return SunHeatInfluxPerSecond / AtmosphericHeatOutFluxPerSecond; } }

        public float DayNightCycleFrequency { get; set; }

        public float SunLightIntensityFactor { get; set; }

        public float TileTemperatureExchangeAmplification { get; set; }
        public float TileTemperatureChangeMaximum { get; set; }
    }
}
