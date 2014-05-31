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
            float yPos = _tile.GetPositionInTiles().Y;
            float ySize = _tile.GetWorldProperties().WorldSizeInTiles.Y;
            yFactorWarmRegions = 0.5f + (float)(0.5 * Math.Cos(yPos / ySize * Math.PI) * Math.Cos(yPos / ySize * Math.PI));
            yFactorColdRegions = 0.5f + (float)(0.5 * Math.Sin(yPos / ySize * Math.PI) * Math.Sin(yPos / ySize * Math.PI));
            
        }

        float yFactorWarmRegions;
        float yFactorColdRegions;

        protected override void DoUpdate(JamUtilities.TimeObject time)
        {
            float temperatureChange =
                (float)(Math.Sin(_tile.GetLocalTime() * _tile.GetWorldProperties().DayNightCycleFrequency + _tile.GetTileProperties().DayNightCyclePhase)) * _tile.GetWorldProperties().SunLightIntensityFactor;

            EnsureHeatChangeRanges(ref temperatureChange);

            _tile.GetTileProperties().TemperatureInKelvin += temperatureChange;
        }

        public override void DoPostUpdate(JamUtilities.TimeObject time)
        {
            float equatorialHeatFlow = 0.0f;
            if (_tile.GetTileProperties().TemperatureInKelvin >= _tile.GetWorldProperties().DesiredTemperature)
            {
                equatorialHeatFlow = yFactorWarmRegions * time.ElapsedGameTime;
            }
            else
            {
                equatorialHeatFlow = - yFactorColdRegions * time.ElapsedGameTime;
            }

            EnsureHeatChangeRanges(ref equatorialHeatFlow);


            float newTemperature = _tile.GetTileProperties().TemperatureInKelvin - equatorialHeatFlow;
            _tile.GetTileProperties().TemperatureInKelvin = newTemperature;
        }
    }
}
