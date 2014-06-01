using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;

namespace JamUtilities
{
    public static class Camera
    { 
        public static SFML.Window.Vector2f CameraPosition { get; set;}
        public static SFML.Window.Vector2f CameraVelocity { get; private set; }
        public static float CameraMaxVelocity = 200.0f;

        public static Vector2f MinPosition;
        public static Vector2f MaxPosition;

        public static Vector2f ShouldBePosition { get; set; }

        private static void EnsurePositionRanges (ref Vector2f newCamPos)
        {
            if (newCamPos.X <= MinPosition.X)
            {
                newCamPos.X = MinPosition.X;
            }
            if (newCamPos.Y <= MinPosition.Y)
            {
                newCamPos.Y = MinPosition.Y;
            }

            if (newCamPos.X >= MaxPosition.X)
            {
                newCamPos.X = MaxPosition.X;
            }

            if (newCamPos.Y >= MaxPosition.Y)
            {
                newCamPos.Y = MaxPosition.Y;
            }
        }


        public static void DoCameraMovement(JamUtilities.TimeObject deltaT)
        {
            //Vector2f newCamPos = new Vector2f((_player.ActorPosition.X - 6) * GameProperties.TileSizeInPixel, (_player.ActorPosition.Y - 6) * GameProperties.TileSizeInPixel);


            Vector2f playerPosInPixels = ShouldBePosition;
            EnsurePositionRanges(ref playerPosInPixels);
            ShouldBePosition = playerPosInPixels;
            
            float DistanceXSquared = (float)(Math.Sign(CameraPosition.X - playerPosInPixels.X)) * (CameraPosition.X - playerPosInPixels.X) * (CameraPosition.X - playerPosInPixels.X);
            float DistanceYSquared = (float)(Math.Sign(CameraPosition.Y - playerPosInPixels.Y)) * (CameraPosition.Y - playerPosInPixels.Y) * (CameraPosition.Y - playerPosInPixels.Y);

            Vector2f newCamVelocity = 0.125f * new Vector2f(-DistanceXSquared, -DistanceYSquared);
            if (newCamVelocity.X >= CameraMaxVelocity)
            {
                newCamVelocity.X = CameraMaxVelocity;
            }
            else if (newCamVelocity.X <= -CameraMaxVelocity)
            {
                newCamVelocity.X = -CameraMaxVelocity;
            }
            if (newCamVelocity.Y >= CameraMaxVelocity)
            {
                newCamVelocity.Y = CameraMaxVelocity;
            }
            else if (newCamVelocity.Y <= -CameraMaxVelocity)
            {
                newCamVelocity.Y = -CameraMaxVelocity;
            }

            CameraVelocity = newCamVelocity;

            Vector2f newCamPos = CameraPosition + CameraVelocity * deltaT.ElapsedRealTime;
            EnsurePositionRanges(ref newCamPos);

            CameraPosition = newCamPos;
        }
    }
}
