using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldInterfaces;
using WorldEvolver;
using JamUtilities;

namespace WorldEvolver.TemperatureControlStrategies
{
    abstract class cAbstractTemperatureControlStragegy
    {
        public cAbstractTemperatureControlStragegy(cTile tile)
        {
            _tile = tile;
            maximumHeatTransferPerFrame = _tile.GetWorldProperties().TileTemperatureChangeMaximum;
        }

        protected cTile _tile;

        public void Update(TimeObject time )
        {
            // TODO some Pre and Post Checks?
            DoUpdate(time);
        }

        /// <summary>
        ///  This will be called every frame for every tile
        /// </summary>
        /// <param name="time"></param>
        protected abstract void DoUpdate(TimeObject time);

        /// <summary>
        ///  This will be called after the update of all Tiles
        /// </summary>
        /// <param name="time"></param>
        public abstract void DoPostUpdate(TimeObject time);

        protected float maximumHeatTransferPerFrame;
        // This Method ensures that the temperature change is not larger than the maximum allowed HeatTransfer (for numeric issues this can get large when a frame takes a long time to calculate)
        protected void EnsureHeatChangeRanges(ref float temperatureChange)
        {
            if (temperatureChange >= maximumHeatTransferPerFrame)
            {
                temperatureChange = maximumHeatTransferPerFrame;
            }
            if (temperatureChange <= -maximumHeatTransferPerFrame)
            {
                temperatureChange = -maximumHeatTransferPerFrame;
            }
        }
    }
}
