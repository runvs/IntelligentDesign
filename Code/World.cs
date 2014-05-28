using SFML.Graphics;
using System;
using JamUtilities;
using JamUtilities.Particles;
using JamUtilities.ScreenEffects;

namespace JamTemplate
{
    public class World
    {

        #region Fields

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
        }

        #endregion Methods

    }
}
