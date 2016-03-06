using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class ResolutionManager
    {
        public Vector2 DeviceResolution;
        public Vector2 VirtualResolution;

        GameWindow Window;

        public ResolutionManager(GameWindow Window)
        {
            this.Window = Window;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {

            DeviceResolution = new Vector2(graphics.GraphicsDevice.Viewport.Width,
                                           graphics.GraphicsDevice.Viewport.Height);
            graphics.PreferredBackBufferWidth  = (int)DeviceResolution.X;
            graphics.PreferredBackBufferHeight = (int)DeviceResolution.Y;

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            VirtualResolution = new Vector2(1920, 1080);

            Console.WriteLine(DeviceResolution);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
