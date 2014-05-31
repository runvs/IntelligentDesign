using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldEvolver.TemperatureControlStrategies
{
    [Obsolete("Use cBasicDayNightCycleStrategy instead", true)]
    class cAdvandcedDayNightCylceStrategy : cAbstractTemperatureControlStragegy
    {
        public cAdvandcedDayNightCylceStrategy(cTile tile)
            : base(tile)
        {
            // Nothing to do here
        }

        protected override void DoUpdate(JamUtilities.TimeObject time)
        {
            _tile.GetTileProperties().SunLightIntensitiyFactor = 1.0f +
                _tile.GetWorldProperties().SunLightIntensityFactor *
                (float)(Math.Sin(-_tile.GetLocalTime() * _tile.GetWorldProperties().DayNightCycleFrequency + _tile.GetTileProperties().DayNightCyclePhase));

            float atmosphericHeatOutFlux = -_tile.GetWorldProperties().AtmosphericHeatOutFluxPerSecond * _tile.GetTileProperties().TemperatureInKelvin;
            float sunHeatInFlux = _tile.GetWorldProperties().SunHeatInfluxPerSecond * _tile.GetTileProperties().SunLightIntensitiyFactor;

            float totalHeatFlux = (atmosphericHeatOutFlux + sunHeatInFlux) * time.ElapsedGameTime;

            _tile.GetTileProperties().TemperatureInKelvin += totalHeatFlux;
        }

        public override void DoPostUpdate(JamUtilities.TimeObject time)
        {
            //
        }
    }
}

