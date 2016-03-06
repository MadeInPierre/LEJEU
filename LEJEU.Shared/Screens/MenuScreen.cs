using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    class MenuScreen : Screen
    {
        Texture2D SplashImage;
        float ElapsedTime = 0;

        public override void Initialize()
        {
            ScreenStatus = "RUNNING";
        }

        public override void LoadContent(ContentManager Content)
        {
            SplashImage = Content.Load<Texture2D>("Splash/image1");
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(ElapsedTime > 6)
            {
                // Send message to ScreenManager, transition on.
                ScreenStatus = "GOTO_MENUSCREEN_AND_DEAD";
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(SplashImage, new Vector2(200, 400), Color.White);
        }
    }
}
