using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldEvolver.TemperatureControlStrategies
{
    class cTemperatureExchangeStrategy : cAbstractTemperatureControlStragegy
    {
        private float _summedUpTemperatureFlow;
        public cTemperatureExchangeStrategy (cTile tile) :base (tile)
        {
            
        }


        protected override void DoUpdate(JamUtilities.TimeObject time)
        {
            _summedUpTemperatureFlow = 0.0f;
            float thisTileTemperature = _tile.GetTileProperties().TemperatureInKelvin;
            float max = _tile.GetWorldProperties().TileTemperatureChangeMaximum;
            foreach (var t in _tile.NeighbourTiles)
            {
                float otherTileTemperature = t.GetTileProperties().TemperatureInKelvin;
                float temperatureDifference = otherTileTemperature - thisTileTemperature;
                float temperatureFlux = temperatureDifference * time.ElapsedGameTime;
                temperatureFlux *= _tile.GetWorldProperties().TileTemperatureExchangeAmplification;
                if (temperatureFlux >= max)
                {
                    temperatureFlux = max;
                }
                if (temperatureFlux <= -max)
                {
                    temperatureFlux = -max;
                }
                _summedUpTemperatureFlow += temperatureFlux;
            }
        }

        public override void DoPostUpdate(JamUtilities.TimeObject time)
        {
            _tile.GetTileProperties().TemperatureInKelvin += _summedUpTemperatureFlow;
        }
    }
}

