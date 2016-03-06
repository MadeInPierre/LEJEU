using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class ScreenManager
    {
        List<Screen> ActiveScreens;
        ContentManager Content;

        public void Initialize()
        {
            ActiveScreens = new List<Screen>();
            ActiveScreens.Add(new SplashScreen());

            foreach (Screen screen in ActiveScreens)
            {
                screen.Initialize();
            }
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = Content;
            foreach (Screen screen in ActiveScreens)
            {
                screen.LoadContent(Content);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Screen screen in ActiveScreens)
            {
                screen.Update(gameTime);
            }

            for(int i = 0; i < ActiveScreens.Count; i++)
            { 
                if(ActiveScreens[i].ScreenStatus.Contains("GOTO"))
                {
                    /*
                    When the screen asks for a new screen, just add and initialize it right away.
                    */
                    if(ActiveScreens[i].ScreenStatus.Contains("GOTO_MENUSCREEN"))
                    {
                        ActiveScreens.Add(new MenuScreen());
                        ActiveScreens[ActiveScreens.Count - 1].Initialize();
                        ActiveScreens[ActiveScreens.Count - 1].LoadContent(Content);
                    }

                    /*
                    If the screen wants to fade out, change it's state.
                    */
                    if (ActiveScreens[i].ScreenStatus.Contains("FADE_OUT"))
                        ActiveScreens[i].ScreenStatus = "FADING_OUT";
                }
                
                /*
                When the screen is done, remove it from the Screens list.
                */
                if (ActiveScreens[i].ScreenStatus.Contains("DEAD"))
                {
                    ActiveScreens[i].UnloadContent();
                    ActiveScreens.Remove(ActiveScreens[i]);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Screen screen in ActiveScreens)
            {
                screen.Draw(sb);
            }
        }
    }
}
