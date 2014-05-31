using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldEvolver.TemperatureControlStrategies
{
    class cBasicDayNightCycleStrategy : cAbstractTemperatureControlStragegy
    {
        public cBasicDayNightCycleStrategy(cTile tile) : base(tile)
        {
            // nothing to do
        }
        protected override void DoUpdate(JamUtilities.TimeObject time)
        {
            float temperatureChange = 
                0.5f * (float)(Math.Sin(_tile.GetLocalTime() * _tile.GetWorldProperties().DayNightCycleFrequency + _tile.GetTileProperties().DayNightCyclePhase));

            if (temperatureChange >= _tile.GetWorldProperties().TileTemperatureChangeMaximum)
            {
                temperatureChange = _tile.GetWorldProperties().TileTemperatureChangeMaximum;
            }
            if (temperatureChange <= -_tile.GetWorldProperties().TileTemperatureChangeMaximum)
            {
                temperatureChange = -_tile.GetWorldProperties().TileTemperatureChangeMaximum;
            }
            _tile.GetTileProperties().TemperatureInKelvin += temperatureChange;
        }

        public override void DoPostUpdate(JamUtilities.TimeObject time)
        {
            
        }
    }
}
