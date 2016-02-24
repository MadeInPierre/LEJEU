using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor
{
    public class CameraEditor
    {

        Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }
        public Vector2 CameraPosition
        {
            get { return position; }
            set { position = value; }
        }
        /*public Vector2 ActiveDimensions
        {
            get { return activeDimensions; }
            set { activeDimensions = value; }
        }*/
        public Vector2 DeviceDimensions
        {
            get { return deviceDimensions; }
            set { deviceDimensions = value; }
        }
        //public static float Scale;
        public static float debugScale = 1;    // zoom


        Vector2 deviceDimensions;               // device screen dimensions

        public static Vector2 position;
        public static float scale { get; set; }


        public CameraEditor()
        {
        }

        public void Initialize(Vector2 spawnPos)
        {
            position = spawnPos;

            deviceDimensions = Game1.Dimensions;

            scale = deviceDimensions.Y / LevelProperties.zoneDimensions.Y;
            //Scale = scale;
        }


        public void Update(InputManager input)
        {
            //Scale = scale;
            if (Game1.form.MouseInXNA)
            {
                if (input.KeyDown(Keys.E)) debugScale *= 1.05f;
                if (input.KeyDown(Keys.A)) debugScale *= 0.95f;
                if (debugScale < 0.2) debugScale = 0.2f;
                if (debugScale > 2) debugScale = 2f;

                if (input.KeyDown(Keys.Z)) position.Y -= 15 * 1 / debugScale;
                if (input.KeyDown(Keys.S)) position.Y += 15 * 1 / debugScale;
                if (input.KeyDown(Keys.Q)) position.X -= 15 * 1 / debugScale;
                if (input.KeyDown(Keys.D)) position.X += 15 * 1 / debugScale;

                if (input.KeyPressed(Keys.H))
                {
                    position = Vector2.Zero;
                    debugScale = 1f;
                }
            }

            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0))
                       * Matrix.CreateScale(scale, scale, 1)
                       * Matrix.CreateScale(debugScale, debugScale, 1f)
                       ;//* Matrix.CreateTranslation(new Vector3(Game1.Dimensions / 2f,0));
        }


        public void UpdateScale(Vector2 newScreenDimensions)
        {
            scale = newScreenDimensions.Y / LevelProperties.zoneDimensions.Y;
            //Scale = scale;
        }
        public static Vector2 ScreenToWorld(Vector2 MousePos)
        {
            return (MousePos / CameraEditor.scale / CameraEditor.debugScale) + CameraEditor.position;
        }
    }
}