using SFML.Window;

namespace WorldEvolver.Animals
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
    }
}
