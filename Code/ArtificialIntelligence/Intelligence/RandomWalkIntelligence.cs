using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using WorldInterfaces;

namespace ArtificialIntelligence.Intelligence
{
    class RandomWalkIntelligence : AbstractIntelligencePattern
    {

        float _timer = 0.0f;
        float _timerMax = 2.0f + (float)( 2.0 * RandomGenerator.Random.NextDouble() - 1.0);

        public RandomWalkIntelligence(Animal animal) : base (animal)
        {

        }

        public override void DoIntelligenceUpdate(JamUtilities.TimeObject timeObject)
        {
            _timer -= timeObject.ElapsedGameTime;

            if (_timer <= 0.0f)
            {
                _timer = _timerMax;
                DoMove();
            }
        }

        private void DoMove()
        {
            _animal.Move(DirectionExtensions.RandomDirection());
        }

    }
}
