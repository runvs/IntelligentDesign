﻿using SFML.Graphics;
using System;
using JamUtilities;
using JamUtilities.Particles;
using JamUtilities.ScreenEffects;
using WorldInterfaces;
using WorldEvolver;
using SFML.Window;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        cWorld _world;
        cWorldProperties _gameWorldCreationProperties;

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
        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(SFML.Graphics.Color.Blue);
            _world.Draw(rw);
            ParticleManager.Draw(rw);

            

            ScreenEffects.GetStaticEffect("vignette").Draw(rw);
            ScreenEffects.Draw(rw);
        }

        private void InitGame()
        {
            CreateWorld();

            Camera.MinPosition = new Vector2f(0, 0);
            Camera.MaxPosition = new Vector2f(GameProperties.WorldSizeInTiles.X * cTile.GetTileSizeInPixelStatic(), GameProperties.WorldSizeInTiles.Y * cTile.GetTileSizeInPixelStatic());

        }

        private void CreateWorld()
        {
            _world = new cWorld();

            _gameWorldCreationProperties = new WorldInterfaces.cWorldProperties();
            _gameWorldCreationProperties.HeightMapNoiseFrequency = 40.0f;
            _gameWorldCreationProperties.WorldSizeInTiles = new SFML.Window.Vector2i(100,100);
            _gameWorldCreationProperties.WaterLevelInMeter = 15.0f;
            _gameWorldCreationProperties.MaxHeightInMeter = 100.0f;

            cTile.TileSizeInPixels = 8.0f;

            IWorldInCreation worldInCreation = _world as IWorldInCreation;

            WorldGeneration.WorldGenerator.CreateWorld(ref worldInCreation, _gameWorldCreationProperties);
        }

        #endregion Methods

    }
}
