using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;

using FarseerPhysics.DebugView;

namespace LEJEU.Shared
{
	public class Game1 : Game
    {
		GraphicsDeviceManager graphics;
		SpriteBatch sb;

		SpriteFont font;

        ScreenManager screenManager;
        ResolutionManager resolutionManager;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

            screenManager = new ScreenManager();
            resolutionManager = new ResolutionManager(Window);

            
            graphics.IsFullScreen = false;
			graphics.ApplyChanges();
		}

		protected override void Initialize()
		{
			Window.AllowUserResizing = true;
			IsMouseVisible = true;

            screenManager.Initialize();
            resolutionManager.Initialize(graphics);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			sb = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Font1");

            screenManager.LoadContent(Content);
		}

		protected override void UnloadContent()
		{

		}

		protected override void Update(GameTime gameTime)
		{
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            screenManager.Update(gameTime);

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			sb.Begin();
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                screenManager.Draw(sb);

                sb.DrawString(font, ((int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds)).ToString(), new Vector2(GraphicsDevice.Viewport.Width - 40, 0), Color.Blue);
            }
			sb.End();

            base.Draw(gameTime);
		}
	}
}
