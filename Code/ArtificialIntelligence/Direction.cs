using System;
using JamUtilities;
using SFML.Window;

namespace ArtificialIntelligence
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public static class DirectionExtensions
    {
        public static Vector2i DirectionToVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return new Vector2i(0, -1);

                case Direction.EAST:
                    return new Vector2i(1, 0);

                case Direction.SOUTH:
                    return new Vector2i(0, 1);

                case Direction.WEST:
                    return new Vector2i(-1, 0);

                default:
                    return new Vector2i(0, 0);
            }
        }

        public static Direction RandomDirection()
        {

            Array values = Enum.GetValues(typeof(Direction));
            Direction randomDir = (Direction)values.GetValue(RandomGenerator.Random.Next(values.Length));
            return randomDir;
        }

    }

}
