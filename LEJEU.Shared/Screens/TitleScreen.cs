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
    public class TitleScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;

		public override void Initialize() { }

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");
            menu = new MenuManager();
            menu.LoadContent(content, "Title");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menu.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            menu.Update(gameTime, input);
        }

        public override void Draw(SpriteBatch sb)
        {
			menu.Draw(sb);
        }
    }
}
