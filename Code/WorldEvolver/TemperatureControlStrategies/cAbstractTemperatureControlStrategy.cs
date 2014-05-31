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
        }

        protected cTile _tile;

        public void Update(TimeObject time )
        {
            // TODO some Pre and Post Checks

            DoUpdate(time);
        }

        protected abstract void DoUpdate(TimeObject time);

        public abstract void DoPostUpdate(TimeObject time);
    }
}
