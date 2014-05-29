using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldInterfaces
{
    public class cTileProperties
    {
        public float TemperatureInKelvin 
        { 
            get 
            {
                return _temperatureInKelvin;
            } 
            set
            {
                _temperatureInKelvin = value;
                if (value < 0)
                {
                    _temperatureInKelvin = 0;
                }
            } 
        }
        private float _temperatureInKelvin;

        public float HeightInMeters
        {
            get
            {
                return _heightInMeters;
            }
            set
            {
                _heightInMeters = value;
            }
        }
        private float _heightInMeters;
    }
}
