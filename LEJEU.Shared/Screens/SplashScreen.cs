using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LEJEU.Shared
{
	public class SplashScreen : GameScreen
	{
		List<Texture2D> images;
		float timer;
		int imageCount;

		public override void Initialize() { }

		public override void LoadContent(ContentManager Content, InputManager inputManager)
		{
			images = new List<Texture2D>();
			images.Add(Content.Load<Texture2D>("Splash/image1"));
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		public override void Update(GameTime gameTime, InputManager input)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			imageCount = (int)(timer % 0.5);

			if (imageCount >= images.Count - 1 || input.KeyPressed(Keys.Escape))
			{
				ScreenManager.AddScreen(new TitleScreen());
				ScreenManager.RemoveScreen(this);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(images[imageCount], new Vector2(), Color.White);
		}
	}
}