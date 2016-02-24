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
	public class GameScreen
	{
		protected ContentManager content;
		protected List<List<string>> attributes, contents;
		public GameScreen() {}
		public virtual void Initialize() { }

		public virtual void LoadContent(ContentManager Content, InputManager inputManager)
		{
			//content = new ContentManager(Content.ServiceProvider, "Content");
            content = Content;
            attributes = new List<List<string>>();
			contents = new List<List<string>>();
		}

		public virtual void UnloadContent()
		{
			content.Unload();
			attributes.Clear();
			contents.Clear();
		}

		public virtual void Update(GameTime gameTime, InputManager input) { }
		public virtual void Draw(SpriteBatch spriteBatch){ }
	}
}
