using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;

namespace ArtificialIntelligence.Intelligence
{
    abstract public class AbstractIntelligencePattern
    {
        protected Animal _animal;
        public AbstractIntelligencePattern(Animal animal)
        {
            _animal = animal;
        }

        public abstract void DoIntelligenceUpdate(TimeObject timeObject);
    }
}
