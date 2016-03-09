using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    class MenuScreen : Screen
    {
        Texture2D MenuImage;
        float ElapsedTime = 0;
        
        public override void Initialize()
        {
            ScreenMessage = null;
            ScreenStatus = "RUNNING";
        }

        public override void LoadContent(ContentManager Content, GraphicsDevice GD)
        {
            MenuImage = Content.Load<Texture2D>("Splash/Menu");
        }

        public override void UnloadContent()
        {
            MenuImage.Dispose();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (input.KeyPressed(Keys.Enter))
            {
                // Send message to ScreenManager, transition on.
                ScreenMessage = new TransitionMessage(new PlayScreen(), TransitionMessage.NextActionEnum.DEAD, TransitionMessage.ScreenStackPosEnum.FRONT);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(MenuImage, Vector2.Zero, Color.White);
        }
    }
}
