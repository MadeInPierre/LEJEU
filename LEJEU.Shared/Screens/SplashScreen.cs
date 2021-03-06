﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    class SplashScreen : Screen
    {
        Texture2D SplashImage;
        float ElapsedTime = 0;
        float transp = 1;

        public override void Initialize()
        {
            ScreenMessage = null;
            ScreenStatus = "RUNNING";
        }

        public override void LoadContent(ContentManager Content, GraphicsDevice GD)
        {
            SplashImage = Content.Load<Texture2D>("Splash/studiomana_logo");
        }

        public override void UnloadContent()
        {
            SplashImage.Dispose();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ScreenStatus == "RUNNING" && (ElapsedTime > 4 || input.KeyPressed(Keys.Enter)))
            {
                ScreenMessage = new TransitionMessage(new MenuScreen(), TransitionMessage.NextActionEnum.FADING_OUT, TransitionMessage.ScreenStackPosEnum.BELOW);
                ScreenStatus = "FADING_OUT";
                ElapsedTime = 0;
            }
            if (ScreenStatus == "FADING_OUT" && ElapsedTime > 2)
                ScreenMessage = new TransitionMessage(TransitionMessage.NextActionEnum.DEAD);

            if (ScreenStatus == "FADING_OUT")
                transp = (float)(-0.5 * ElapsedTime + 1);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(SplashImage, Vector2.Zero, Color.White * transp);
        }
    }
}
