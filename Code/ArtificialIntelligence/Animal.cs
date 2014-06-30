using System;
using ArtificialIntelligence.Intelligence;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;
using WorldInterfaces;

namespace ArtificialIntelligence
{
    public class Animal : IGameObject
    {
        public IWorld World {get; private set;}
        public Tribe Tribe { get; private set; }

        public float HealthMax { get; private set; }
        public float HealthCurrent { get; private set; }
        public float HealthRegeneration { get; private set; }

        public float MoveTimerMax { get; private set; }

        public float Damage { get; private set; }

        public AnimalProperties.DietType Diet { get; private set; }
        public float Hunger { get; private set; }

        public AnimalProperties.TerrainType PreferredTerrain { get; private set; }
        public float PreferredTemperature { get; private set; }
        public float PreferredAltitude { get; private set; }
        public float GroupBehaviour { get; private set; }





        public Vector2i PositionInTiles { get; set; }
        private CircleShape _shape;

        public static float TileSizeInPixels { get; set; }

        private AbstractIntelligencePattern _intelligence;

        public Color AnimalColor { set { _shape.FillColor = value; } }

        private float _temperatureCheckTimer;
        private float _temperatureCheckTimerMax = 1.0f;

        private float _foodTimer;   // will use MoveTimerMax

        private float _fuckDeadTime; // will also use MoveTimerMax
        


        public Animal(AnimalProperties properties, IWorld world, Tribe tribe, Vector2i initialPosition)
        {
            World = world;
            Tribe = tribe;
            _shape = new CircleShape(TileSizeInPixels / 2.0f - 1.0f);
            PositionInTiles = initialPosition;

            CalculateAnimalParameters(properties);

            _intelligence = new AnimalAI(this);
        }

        public bool IsDead()
        {
            if (HealthCurrent <= 0)
            {
                return true;
            }
            return false;
        }

        public void GetInput()
        {
            throw new NotImplementedException();
        }

        public void Update(TimeObject timeObject)
        {
            _intelligence.DoIntelligenceUpdate(timeObject);

            _temperatureCheckTimer -= timeObject.ElapsedGameTime;
            if (_temperatureCheckTimer <= 0)
            {
                _temperatureCheckTimer += _temperatureCheckTimerMax * ( 1.0f + ((float)(RandomGenerator.Random.NextDouble() - 0.5) * 0.5f ));
                DoCheckTemperature();
                DoCheckTerrain();
            }

            _foodTimer -= timeObject.ElapsedGameTime;
            if (_foodTimer <= 0)
            {
                _foodTimer += MoveTimerMax * (1.0f + ((float)(RandomGenerator.Random.NextDouble() - 0.5) * 0.5f));
                EatFood();
            }


            if (_fuckDeadTime >= 0)
            {
                _fuckDeadTime -= timeObject.ElapsedGameTime;
            }
            if (_fuckDeadTime < 0)
            {
                if (Tribe.TwoAnimalOnPosition(PositionInTiles))
                {
                    FuckIt();
                }
            }

        }

        private void FuckIt()
        {
            //Console.WriteLine("Fuck Yeah!");
            _fuckDeadTime += 175.0f * (float)Math.Sqrt(Math.Sqrt(MoveTimerMax));
            Tribe.LetThemHaveFun();
        }

        private void EatFood()
        {
            ITile currentTile = World.GetTileOnPosition(PositionInTiles);
            if(Diet == AnimalProperties.DietType.HERBIVORE || Diet == AnimalProperties.DietType.OMNIVORE)
            {

                if (currentTile.GetTileProperties().GetFoodAmountOnTile(eFoodType.FOOD_TYPE_PLANT) >= Hunger)
                {
                    currentTile.GetTileProperties().ChangeFoodAmountOnTile(eFoodType.FOOD_TYPE_PLANT, -Hunger);
                    if (HealthCurrent < HealthMax)
                    {
                        HealthCurrent += HealthRegeneration;
                    }
                }
            }
        }

        private void DoCheckTerrain()
        {
            if (PreferredTerrain == AnimalProperties.TerrainType.LAND)
            {
                if (World.GetTileOnPosition(PositionInTiles).GetTileType() == eTileType.TILETYPE_WATER)
                {
                    HealthCurrent -= 5;
                }
            }
            else
            {
                if (World.GetTileOnPosition(PositionInTiles).GetTileType() != eTileType.TILETYPE_WATER)
                {
                    HealthCurrent -= 4;
                }
            }
        }

        private void DoCheckTemperature()
        {
            float temperatureDifferenc = Math.Abs(World.GetTileOnPosition(PositionInTiles).GetTileProperties().TemperatureInKelvin - Tribe.Properties.PreferredTemperature);
            if (temperatureDifferenc >= 10)
            {
                HealthCurrent -= 1.75f;
            }
        }

        public void Draw(RenderWindow rw)
        {
            _shape.Position = TileSizeInPixels * new Vector2f(PositionInTiles.X, PositionInTiles.Y) - JamUtilities.Camera.CameraPosition;
            rw.Draw(_shape);
        }

        public void Move(Direction direction)
        {
            PositionInTiles += direction.DirectionToVector();

            if (PositionInTiles.X < 0)
            {
                PositionInTiles += new Vector2i(World.GetWorldProperties().WorldSizeInTiles.X, 0);
            }
            if (PositionInTiles.X >= World.GetWorldProperties().WorldSizeInTiles.X)
            {
                PositionInTiles -= new Vector2i(World.GetWorldProperties().WorldSizeInTiles.X, 0);
            }

            if (PositionInTiles.Y < 0)
            {
                PositionInTiles += new Vector2i( 0, World.GetWorldProperties().WorldSizeInTiles.Y);
            }
            if (PositionInTiles.Y >= World.GetWorldProperties().WorldSizeInTiles.Y)
            {
                PositionInTiles -= new Vector2i(0, World.GetWorldProperties().WorldSizeInTiles.Y);
            }

        }

        private void CalculateAnimalParameters(AnimalProperties prop)
        {
            HealthMax = 5 + prop.Stamina * 5 + prop.Strength * 3;
            HealthCurrent = HealthMax;
            HealthRegeneration = prop.Stamina * 0.5f;

            MoveTimerMax = 1.0f / (prop.Agility + 1.0f);
            //Console.WriteLine("Move Timer is " + MoveTimerMax);
            
            Hunger = (prop.Strength + prop.Stamina) * 0.5f;

            PreferredAltitude = prop.PreferredAltitude;
            PreferredTerrain = prop.PreferredTerrain;
            PreferredTemperature = prop.PreferredTemperature;

            GroupBehaviour = prop.GroupBehaviour / 15.0f - 0.1f;
        }
    }
}
