using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class ResolutionManager
    {
        public static Vector2 DeviceResolution;
        public static Vector2 VirtualResolution;

        GameWindow Window;

        public ResolutionManager(GameWindow Window)
        {
            this.Window = Window;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            VirtualResolution = new Vector2(1920, 1080);
            //HARDCODED
            DeviceResolution = VirtualResolution;
            
            graphics.PreferredBackBufferWidth  = (int)DeviceResolution.X;
            graphics.PreferredBackBufferHeight = (int)DeviceResolution.Y;
            
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        public void Update(GameTime gameTime)
        {

        }

        /*
        THINGS TO IMPLEMENT :
            2D Matrix that scales the whole content evenly, with black bars on the side if the aspect ratio isn't right.
        */
    }
}
