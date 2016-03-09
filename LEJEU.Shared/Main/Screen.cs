using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class Screen
    {
        public TransitionMessage ScreenMessage;
        protected string ScreenStatus;

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent(ContentManager Content, GraphicsDevice GD)
        {

        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime, InputManager input)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
