using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    class SplashScreen : Screen
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

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ElapsedTime > 4)
                ScreenStatus = "GOTO_MENUSCREEN_AND_FADE_OUT";
            if (ElapsedTime > 6)
                ScreenStatus = "DEAD";
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 imgpos = Vector2.Zero;
            if (ScreenStatus == "RUNNING")
                imgpos = new Vector2(20, 20);
            if (ScreenStatus == "FADING_OUT")
                imgpos = new Vector2(200, 20);
            sb.Draw(SplashImage, imgpos, Color.White);
        }
    }
}
