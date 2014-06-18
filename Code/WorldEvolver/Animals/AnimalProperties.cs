
namespace WorldEvolver
{
    public class AnimalProperties
    {
        /// <summary>
        /// The stamina of an animal. Determines how much damage an animal can endure before dying.
        /// </summary>
        public float Stamina { get; set; }

        /// <summary>
        /// The agility of an animal. Determines how fast an animal can walk.
        /// </summary>
        public float Agility { get; set; }

        /// <summary>
        /// The strength of an animal. Determines how much damage an animal may deal.
        /// </summary>
        public float Strength { get; set; }

        /// <summary>
        /// What kind of food the animal wants to eat.
        /// </summary>
        public DietType Diet { get; set; }

        /// <summary>
        /// The higher this value is the more the animals tend to stick together.
        /// </summary>
        public float GroupBehaviour { get; set; }

        /// <summary>
        /// Which type of terrain the animal prefers.
        /// </summary>
        public TerrainType PreferredTerrain { get; set; }

        /// <summary>
        /// Which altitude the animal prefers.
        /// </summary>
        public float PreferredAltitude { get; set; }

        /// <summary>
        /// Which temperature the animal prefers.
        /// </summary>
        public float PreferredTemperature { get; set; }

        public enum TerrainType
        {
            LAND,
            WATER
        }

        public enum DietType
        {
            HERBIVORE,
            CARNIVORE,
            OMNIVORE
        }
    }
}
