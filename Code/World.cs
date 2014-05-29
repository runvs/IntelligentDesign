using SFML.Graphics;
using System;
using JamUtilities;
using JamUtilities.Particles;
using JamUtilities.ScreenEffects;
using WorldInterfaces;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        IWorld _gameWorld;
        cWorldProperties _gameWorldCreationProperties;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public void GetInput()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.C))
            {
                //ScreenEffects.ScreenFlash(SFML.Graphics.Color.Black, 4.0f);
            }

        }

        public void Update(TimeObject timeObject)
        {
            ScreenEffects.Update(timeObject);
            SpriteTrail.Update(timeObject);
            ParticleManager.Update(timeObject);
        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(SFML.Graphics.Color.Blue);
            ParticleManager.Draw(rw);

            

            ScreenEffects.GetStaticEffect("vignette").Draw(rw);
            ScreenEffects.Draw(rw);
        }

        private void InitGame()
        {
            CreateWorld();
        }

        private void CreateWorld()
        {

            _gameWorld = new WorldInterfaces.cWorld();
            _gameWorldCreationProperties = new WorldInterfaces.cWorldProperties();
            _gameWorldCreationProperties.HeightMapNoiseFrequency = 40.0f;
            _gameWorldCreationProperties.WorldSizeInTiles = new SFML.Window.Vector2i(100,100);

            IWorldInCreation worldInCreation = _gameWorld as IWorldInCreation;

            WorldGeneration.WorldGenerator.CreateWorld(ref worldInCreation, _gameWorldCreationProperties);
        }

        #endregion Methods

    }
}
