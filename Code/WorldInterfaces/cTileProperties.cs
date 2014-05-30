﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldInterfaces
{
    public class cTileProperties
    {
        public cTileProperties()
        {
            _foodTable = new Dictionary<eFoodType, float>();

            foreach (eFoodType e in Enum.GetValues(typeof(eFoodType)))
            {
                _foodTable.Add(e, 0.0f);
            }
            TemperatureInKelvin = 300;
        }

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

        System.Collections.Generic.Dictionary<eFoodType, float> _foodTable;
        public float GetFoodAmountOnTile (eFoodType foodType)
        {
            return _foodTable[foodType];
        }
        public void SetFoodAmountOnTile (eFoodType foodType, float newValue)
        {
            _foodTable[foodType] = newValue;
        }

        public void ChangeFoodAmountOnTile(eFoodType foodType, float delta)
        {
            _foodTable[foodType] += delta;
        }



    }
}

