using SFML.Graphics;
using System;
using JamUtilities;
using JamUtilities.Particles;
using JamUtilities.ScreenEffects;
using WorldInterfaces;
using WorldEvolver;
using SFML.Window;
using ArtificialIntelligence;
using System.Collections.Generic;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        cWorld _world;
        public cWorldProperties GameWorldCreationProperties { get; set; }

        private List<Tribe> _tribeList;

        #endregion Fields

        #region Methods

        public World()
        {
            _tribeList = new List<Tribe>();
        }

        public void GetInput()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Left))
            {
                Camera.ShouldBePosition = Camera.ShouldBePosition + new Vector2f(-10,0);
            }
            else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Right))
            {
                Camera.ShouldBePosition = Camera.ShouldBePosition + new Vector2f(10, 0);
            }
            else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Up))
            {
                Camera.ShouldBePosition = Camera.ShouldBePosition + new Vector2f(0, -10);
            }
            else if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Down))
            {
                Camera.ShouldBePosition = Camera.ShouldBePosition + new Vector2f(0, 10);
            }

            _world.GetInput();

        }

        public void Update(TimeObject timeObject)
        {
            ScreenEffects.Update(timeObject);
            SpriteTrail.Update(timeObject);
            ParticleManager.Update(timeObject);
            _world.Update(timeObject);
            JamUtilities.Camera.DoCameraMovement(timeObject);

            List<Tribe> templist = new List<Tribe>();
            foreach (var t in _tribeList)
            {
                t.Update(timeObject);
                if (!t.IsDead())
                {
                    templist.Add(t);
                }
            }
            _tribeList = templist;

        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(SFML.Graphics.Color.Cyan);
            _world.Draw(rw);
            ParticleManager.Draw(rw);

            foreach (var t in _tribeList)
            {
                t.Draw(rw);
            }

            ScreenEffects.GetStaticEffect("vignette").Draw(rw);
            ScreenEffects.Draw(rw);
        }

        public void InitWorld()
        {
            CreateWorld();
            Camera.MinPosition = new Vector2f(0, 0);
            Camera.MaxPosition = new Vector2f(GameProperties.WorldSizeInTiles.X * cTile.GetTileSizeInPixelStatic() - 800, GameProperties.WorldSizeInTiles.Y * cTile.GetTileSizeInPixelStatic() - 600);

        }

        private void CreateWorld()
        {
            _world = new cWorld();

            

            cTile.TileSizeInPixels = 8.0f;
            Animal.TileSizeInPixels = 8.0f;
            

            IWorldInCreation worldInCreation = _world as IWorldInCreation;

            WorldGeneration.WorldGenerator.CreateWorld(ref worldInCreation, GameWorldCreationProperties);
            CreateRandomTribes();
        }


        public void AddTribe(Tribe tribe)
        {
            
            tribe.PositionInTiles = RandomGenerator.GetRandomVector2iInRect(new IntRect(0, 0, _world.GetWorldProperties().WorldSizeInTiles.X, _world.GetWorldProperties().WorldSizeInTiles.Y));

            Console.WriteLine("Spawning Tribe at " + tribe.PositionInTiles);

            for (int i = 0; i != 50; ++i)
            {
                tribe.SpawnAnimal();
            }

            _tribeList.Add(tribe);
        }


        public void CreateRandomTribes()
        {
           

            AnimalProperties properties = new AnimalProperties();
            properties.Agility = 1;
            properties.Diet = AnimalProperties.DietType.CARNIVORE;
            properties.GroupBehaviour = 1;
            properties.PreferredAltitude = 50;
            properties.PreferredTemperature = 300;
            properties.PreferredTerrain = AnimalProperties.TerrainType.LAND;
            properties.Stamina = 1;
            properties.Strength = 1;

            Tribe tribe = new Tribe(_world, properties);

            tribe.PositionInTiles = RandomGenerator.GetRandomVector2iInRect(new IntRect(0, 0, _world.GetWorldProperties().WorldSizeInTiles.X, _world.GetWorldProperties().WorldSizeInTiles.Y));
            for (int i = 0; i != 50; ++i)
            {
                tribe.SpawnAnimal();
            }
            Console.WriteLine("Spawning Tribe at " + tribe.PositionInTiles);
            _tribeList.Add(tribe);


            tribe = new Tribe(_world, properties);

            tribe.PositionInTiles = RandomGenerator.GetRandomVector2iInRect(new IntRect(0, 0, _world.GetWorldProperties().WorldSizeInTiles.X, _world.GetWorldProperties().WorldSizeInTiles.Y));
            for (int i = 0; i != 50; ++i)
            {
                tribe.SpawnAnimal();
            }
            Console.WriteLine("Spawning Tribe at " + tribe.PositionInTiles);
            _tribeList.Add(tribe);
        }

        public void SpawnTribe(AnimalProperties properties)
        {
            Tribe tribe = new Tribe(_world, properties);
            AddTribe(tribe);
        }



        #endregion Methods

    }
}
