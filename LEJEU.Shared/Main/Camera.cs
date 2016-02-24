using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;

namespace LEJEU.Shared
{
    public class Camera
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
//        public Vector2 DeviceDimensions
//        {
//			get { return ScreenManager.Instance.Dimensions; }
//			set { ScreenManager.Instance.Dimensions = value; }
//        }
        //public static float Scale;

        /*Vector2 lowDimensions = new Vector2(1366, 768);      // default dimensions
        Vector2 medDimensions = new Vector2(1920, 1200);
        Vector2 retDimensions = new Vector2(2880, 1800);     // rétina*/


        //Vector2 activeDimensions;               // default dimensions being used
        //Vector2 deviceDimensions;               // device screen dimensions

        private Vector2 position;
        //public float scale { get; set; }


        public Camera()
        {
        }

		public void Initialize(int spawnPos)
        {
			position.X = spawnPos;
//            if(Game1.graphics.IsFullScreen) 
//				ScreenManager.Instance.Dimensions  = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
//                                                                 GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
//			else ScreenManager.Instance.Dimensions = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width  * 3/4,
//                                                                 GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 3/4);

            //deviceDimensions = new Vector2(1900,300);
            //deviceDimensions = new Vector2(800,600);
            //deviceDimensions = new Vector2(1440,900);
            //deviceDimensions = new Vector2(1920,900);
            
            /*if (deviceDimensions.Y <= lowDimensions.Y) activeDimensions = lowDimensions;        // Détecte la plus 
            else if (deviceDimensions.Y <= medDimensions.Y) activeDimensions = medDimensions;     // basse résolution
            else activeDimensions = retDimensions;*/                                              // supérieure à l'écran


//			scale = ScreenManager.Instance.Dimensions.Y / LevelProperties.zoneDimensions.Y;
//            Scale = scale;
        }


        public void Update(InputManager input, Vector2 playerPos, LevelProperties LP, float debugScale)
        {
//			scale = ScreenManager.Instance.Dimensions.Y / LevelProperties.zoneDimensions.Y;
//            Scale = scale;
//            Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);


            /*float targetPosition = playerPos.X - (deviceDimensions.X / scale / 2);
            position.X = MathHelper.Lerp(position.X, targetPosition, 0.1f);           // lerp 0.075

            if (position.X < 0) position.X = 0;             // verifies that player doesn't go out of bounds
            else if (position.X > (LP.NZones + 1) * LevelProperties.zoneDimensions.X - (deviceDimensions.X / scale))
                position.X = (LP.NZones + 1) * LevelProperties.zoneDimensions.X - (deviceDimensions.X / scale);


            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0))
                       * Matrix.CreateScale(scale, scale, 1)
                       * Matrix.CreateScale(debugScale, debugScale, 1f);*/

			float targetPosition = playerPos.X - (ResolutionManager.WindowRes.X / ResolutionManager.WindowScale / 2);
			position.X = MathHelper.Lerp(position.X, targetPosition, 0.1f);           // lerp 0.075

			if (position.X < 0) position.X = 0;             // verifies that player doesn't go out of bounds
			else if (position.X > (LP.NZones + 1) * ResolutionManager.GameRes.X - (ResolutionManager.WindowRes.X / ResolutionManager.WindowScale))
				position.X = (LP.NZones + 1) * ResolutionManager.GameRes.X - (ResolutionManager.WindowRes.X / ResolutionManager.WindowScale);


            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0))
                * Matrix.CreateScale(ResolutionManager.WindowScale, ResolutionManager.WindowScale, 1f)
                         * Matrix.CreateScale(debugScale, debugScale, 1f);
        }
    }
}