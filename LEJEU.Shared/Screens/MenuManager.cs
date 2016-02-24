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
	public class MenuManager
	{
		ContentManager content;

		public void LoadContent(ContentManager content, string id)
		{
			this.content = new ContentManager(content.ServiceProvider, "Content");
		}

		public void UnloadContent()
		{
			content.Unload();			
		}

		public void Update(GameTime gameTime, InputManager input)
		{
			/*
			if (axis == 1)
			{
				if (input.KeyPressed(Keys.Right, Keys.D) || input.ButtonPressed(Buttons.DPadRight))
					itemNumber++;
				else if (input.KeyPressed(Keys.Left, Keys.A) || input.ButtonPressed(Buttons.DPadLeft))
					itemNumber--;
			}
			else
			{
				if (input.KeyPressed(Keys.Down, Keys.S) || input.ButtonPressed(Buttons.DPadDown))
					itemNumber++;
				else if (input.KeyPressed(Keys.Up, Keys.W) || input.ButtonPressed(Buttons.DPadUp))
					itemNumber--;
			}
			*/
			if(input.KeyPressed(Keys.Enter, Keys.Space) || input.ButtonPressed(Buttons.A))
			{
				Type newClass = Type.GetType("LEJEU.Shared." + "GamePlayScreen");
				ScreenManager.AddScreen((GameScreen)Activator.CreateInstance(newClass));
			}
			
		}

		public void Draw(SpriteBatch sb)
		{
			
		}
	}
}
