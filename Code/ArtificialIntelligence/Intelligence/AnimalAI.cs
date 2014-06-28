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
                _moveTimer = _animal.MoveTimerMax + ((float)(RandomGenerator.Random.NextDouble() - 0.5) * 0.5f * _animal.MoveTimerMax);
                moveRecursionDepth = 0;
                DoMove();
            }
            
        }

        private void DoMove()
        {
            moveRecursionDepth++;
            Vector2i tribeCenter = _animal.Tribe.PositionInTiles;
            Vector2i animapPosition = _animal.PositionInTiles;
            float difX = tribeCenter.X - animapPosition.X;
            float difY = tribeCenter.Y - animapPosition.Y;
            float distanceToCenterSquared = difX * difX + difY * difY;

            Direction moveDirection = DirectionExtensions.RandomDirection();

            Vector2i newAnimalPosition = _animal.PositionInTiles + moveDirection.DirectionToVector();
            float dif2X = tribeCenter.X - newAnimalPosition.X;
            float dif2Y = tribeCenter.Y - newAnimalPosition.Y;
            float newDistanceToCenterSquared = dif2X * dif2X + dif2Y * dif2Y;

            float penality = 1.0f;


            if (newDistanceToCenterSquared > distanceToCenterSquared)
            {
                penality -= _animal.GroupBehaviour;
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
