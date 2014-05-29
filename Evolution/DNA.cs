using System;
using System.Collections.Generic;

using JamUtilities;

namespace Evolution
{
    class DNA
    {
        public enum GenType { TEMPERATURE }
        public Dictionary<GenType, double> Gen {get; private set;}
        public DNA()
        {
            foreach (GenType type in Enum.GetValues(typeof(GenType)))
            {
                Gen.Add(type, RandomGenerator.Random.NextDouble());
            }
        }
        public DNA(DNA A, DNA B)
        {
            foreach (GenType type in Enum.GetValues(typeof(GenType)))
            {
                Gen.Add(type, RandomGenerator.GetRandomDouble(A.Gen[type], B.Gen[type]));
            }
        }
    }
}
