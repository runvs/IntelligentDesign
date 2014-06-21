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
        cWorldProperties _gameWorldCreationProperties;

        private List<Tribe> _tribeList;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
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

        private void InitGame()
        {
            CreateWorld();

            Camera.MinPosition = new Vector2f(0, 0);
            Camera.MaxPosition = new Vector2f(GameProperties.WorldSizeInTiles.X * cTile.GetTileSizeInPixelStatic() - 800, GameProperties.WorldSizeInTiles.Y * cTile.GetTileSizeInPixelStatic() - 600);

        }

        private void CreateWorld()
        {
            _world = new cWorld();

            _gameWorldCreationProperties = new WorldInterfaces.cWorldProperties();
            _gameWorldCreationProperties.HeightMapNoiseLength = 20.0f;
            _gameWorldCreationProperties.WorldSizeInTiles = new SFML.Window.Vector2i(GameProperties.WorldSizeInTiles.X, GameProperties.WorldSizeInTiles.Y);
            
            _gameWorldCreationProperties.MaxHeightInMeter = 100.0f;
            _gameWorldCreationProperties.AtmosphericHeatOutFluxPerSecond = 1.0f / 293.0f;
            _gameWorldCreationProperties.SunHeatInfluxPerSecond = 1.0f;
                        _gameWorldCreationProperties.DayNightCycleFrequency = 0.11f;
            _gameWorldCreationProperties.SunLightIntensityFactor = 0.75f * _gameWorldCreationProperties.DayNightCycleFrequency;

            _gameWorldCreationProperties.TileTemperatureChangeMaximum = 3;
            _gameWorldCreationProperties.TileTemperatureExchangeAmplification = 0.5f;

            _gameWorldCreationProperties.MountainHeight = 70;
            _gameWorldCreationProperties.WaterFreezingTemperature = 283.5f;
            _gameWorldCreationProperties.DesertGrassTransitionAtHeightZero = 304.0f;
            _gameWorldCreationProperties.DesertGrassTransitionAtMountainHeight = 298.50f;
            _gameWorldCreationProperties.WaterGrassTransitionAtHeightZero = 304.50f;
            _gameWorldCreationProperties.WaterGrassTransitionHeightAtWaterFreezingPoint = 30.0f;

            _gameWorldCreationProperties.RainWaterAmount  = 1.0f;
            _gameWorldCreationProperties.PlantGrowthWaterAmount = 3.0f;
            _gameWorldCreationProperties.PlantGrowthRate = 5.0f;
            _gameWorldCreationProperties.CloudNumber = 1;

            _gameWorldCreationProperties.TileTemperatureIntegrationTimer = 1.5f; 

            cTile.TileSizeInPixels = 8.0f;
            Animal.TileSizeInPixels = 8.0f;
            

            IWorldInCreation worldInCreation = _world as IWorldInCreation;

            WorldGeneration.WorldGenerator.CreateWorld(ref worldInCreation, _gameWorldCreationProperties);
            CreateRandomTribes();
        }


        public void AddTribe(Tribe tribe)
        {
            _tribeList.Add(tribe);
        }


        public void CreateRandomTribes()
        {
            _tribeList = new List<Tribe>();

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

            for (int i = 0; i != 10; ++i)
            {
                tribe.SpawnAninal();
            }

            _tribeList.Add(tribe);
        }


        #endregion Methods

    }
}
