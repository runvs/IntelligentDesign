using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Window;

namespace ArtificialIntelligence.Intelligence
{
    class AnimalAI : AbstractIntelligencePattern
    {
        public AnimalAI(Animal animal)
            : base(animal)
        {

        }


        private float _moveTimer;
        private int moveRecursionDepth;


        public override void DoIntelligenceUpdate(JamUtilities.TimeObject timeObject)
        {
            
            _moveTimer -= timeObject.ElapsedGameTime;

            if (_moveTimer <= 0)
            {
                _moveTimer = _animal.MoveTimerMax * ( 1.0f + ((float)(RandomGenerator.Random.NextDouble() - 0.5) * 0.5f ));
                moveRecursionDepth = 0;
                DoMove();
            }
            
        }

        private void DoMove()
        {
            moveRecursionDepth++;
            Vector2i tribeCenter = _animal.Tribe.PositionInTiles;
            Vector2i animalPosition = _animal.PositionInTiles;
            float difX = tribeCenter.X - animalPosition.X;
            float difY = tribeCenter.Y - animalPosition.Y;
            float distanceToCenterSquared = difX * difX + difY * difY;

            float temperaturAtPosition = _animal.World.GetTileOnPosition(animalPosition).GetTileProperties().TemperatureInKelvin;
            float diffrerenceToPreferredTemperature = Math.Abs(_animal.PreferredTemperature - temperaturAtPosition);

            float HeightAtPosition = _animal.World.GetTileOnPosition(animalPosition).GetTileProperties().HeightInMeters;
            float diffrerenceToPreferredHeight = Math.Abs(_animal.PreferredAltitude - HeightAtPosition);

            Direction moveDirection = DirectionExtensions.RandomDirection();

            Vector2i newAnimalPosition = _animal.PositionInTiles + moveDirection.DirectionToVector();
            float dif2X = tribeCenter.X - newAnimalPosition.X;
            float dif2Y = tribeCenter.Y - newAnimalPosition.Y;
            float newDistanceToCenterSquared = dif2X * dif2X + dif2Y * dif2Y;

            float temperaturAtNewPosition = _animal.World.GetTileOnPosition(newAnimalPosition).GetTileProperties().TemperatureInKelvin;
            float newDiffrerenceToPreferredTemperature = Math.Abs(_animal.PreferredTemperature - temperaturAtNewPosition);
            float temperatureGain = diffrerenceToPreferredTemperature - newDiffrerenceToPreferredTemperature;

            float HeightAtNewPosition = _animal.World.GetTileOnPosition(newAnimalPosition).GetTileProperties().HeightInMeters;
            float newDiffrerenceToPreferredHeight = Math.Abs(_animal.PreferredAltitude - HeightAtNewPosition);
            float HeightGain = diffrerenceToPreferredHeight - newDiffrerenceToPreferredHeight;

            float penality = 0.8f;


            if (newDistanceToCenterSquared > distanceToCenterSquared)
            {
                penality -= _animal.GroupBehaviour;
            }
            if (newDiffrerenceToPreferredTemperature < diffrerenceToPreferredTemperature)
            {
                penality += temperatureGain*2.0f ;
            }
            if (newDiffrerenceToPreferredHeight < diffrerenceToPreferredHeight)
            {
                penality += HeightGain/4.0f;
            }
            
            
            



            if (RandomGenerator.Random.NextDouble() <= penality)
            {
                _animal.Move(moveDirection);
            }
            else
            {
                if (moveRecursionDepth < 20)
                {
                    DoMove();
                }
                else
                {
                    return;
                }
            }

            
        }

        public static double WrongMoveAcceptanceProbability { get { return 0.75; } }
    }
}
