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

using FarseerPhysics.DebugView;

namespace LEJEU.Shared
{
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
		SpriteBatch sb;

		InputManager input;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
            graphicsDevice = GraphicsDevice;
			Content.RootDirectory = "Content";

			/// Setting the window's properties
			graphics.IsFullScreen = false;
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			graphics.ApplyChanges();
		}

		protected override void Initialize()
		{
			input = new InputManager();
			ScreenManager.Initialize();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			sb = new SpriteBatch(GraphicsDevice);
            ScreenManager.LoadContent(Content, input);
		}

		protected override void UnloadContent()
		{

		}

		protected override void Update(GameTime gameTime)
		{
			if (input.KeyDown(Keys.LeftWindows) && input.KeyPressed(Keys.Q)) this.Exit();

		    ScreenManager.Update(gameTime, input);

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			sb.Begin();
			    GraphicsDevice.Clear(Color.CornflowerBlue);
                ScreenManager.Draw(sb);
			sb.End();

            base.Draw(gameTime);
        }
	}
}
