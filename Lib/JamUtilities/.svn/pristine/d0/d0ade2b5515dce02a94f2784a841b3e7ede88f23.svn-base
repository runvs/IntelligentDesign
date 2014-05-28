using SFML.Window;

namespace JamUtilities
{
    public static class Mouse
    {
        public static SFML.Graphics.RenderWindow Window { get; set; }

        /// <summary>
        /// This Method gets the absolute MousePosition on your Screen
        /// </summary>
        public static Vector2i MousePositionOnScreen { get; private set; }

        /// <summary>
        /// This Method geths the absolute Mouse Position in the Window
        /// </summary>
        public static Vector2i MousePositionInWindow { get; private set; }

        public static bool IsMouseInWindow { get; private set; }

        public static void Update()
        {
            MousePositionOnScreen = SFML.Window.Mouse.GetPosition();
            if (Window != null)
            {
                MousePositionInWindow = SFML.Window.Mouse.GetPosition(Window);

                if (MousePositionInWindow.X >= 800 || MousePositionInWindow.Y >= 600)
                {
                    IsMouseInWindow = false;
                }
                else
                {
                    IsMouseInWindow = true;
                }
            }
            else
            {
                MousePositionInWindow = MousePositionOnScreen;
            }

        }
    }
}
