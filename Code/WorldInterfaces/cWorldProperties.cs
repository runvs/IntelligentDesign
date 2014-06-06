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

        #region WorldGeneration
        
        public Vector2i WorldSizeInTiles {get; set;}

        public float HeightMapNoiseLength { get; set; }

        public float MaxHeightInMeter { get; set; }

        #endregion WorldGeneration

        #region TileCharacteristics

        //public float WaterHeightOffset{ get; set; } // At T = 0K
        //public float WaterSlope { get; set; }
        public float WaterGrassTransitionAtHeightZero { get; set;  }
        public float WaterGrassTransitionHeightAtWaterFreezingPoint { get; set; }
        public float MountainHeight { get; set; }
        public float DesertGrassTransitionAtHeightZero { get; set; }   // Start of desert at height 0 meter
        //public float DesertSlope { get; set; }   // Slope
        public float DesertGrassTransitionAtMountainHeight { get; set; }
        public float WaterFreezingTemperature { get; set; }

        #endregion TileCharacteristics

        #region AtmosphericStuff

        public float SunHeatInfluxPerSecond { get; set; }

        public float AtmosphericHeatOutFluxPerSecond { get; set; }

        public float DesiredTemperature { get { return SunHeatInfluxPerSecond / AtmosphericHeatOutFluxPerSecond; } }

        public float DayNightCycleFrequency { get; set; }

        public float SunLightIntensityFactor { get; set; }

        public float TileTemperatureExchangeAmplification { get; set; }
        public float TileTemperatureChangeMaximum { get; set; }

        /// <summary>
        /// 100 "last temperature" Values will be stored in each cTile. So this value times 100 is the time that will be integrated over
        /// </summary>
        public float TileTemperatureIntegrationTimer { get; set; }

        #endregion AtmosphericStuff
    }
}
