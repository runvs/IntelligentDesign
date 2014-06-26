using SFML.Window;

namespace JamTemplate
{
    public static class GameProperties
    {
        public static Vector2i WorldSizeInTiles { get { return new Vector2i(200, 200); } }

        public static int EvolutionPointsStart { get { return 100; } }

        public static int EvolutionPointsWorldMax { get { return 75; } }
    }
}
