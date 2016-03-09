using LEJEU.Shared;
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
        GraphicsDevice GD;

        public void Initialize()
        {
            ActiveScreens = new List<Screen>();
            ActiveScreens.Add(new PlayScreen());

            foreach (Screen screen in ActiveScreens)
            {
                screen.Initialize();
            }
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GD)
        {
            this.Content = Content;
            this.GD = GD;
            foreach (Screen screen in ActiveScreens)
            {
                screen.LoadContent(Content, GD);
            }
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            for(int i = 0; i < ActiveScreens.Count; i++)
            { 
                if(ActiveScreens[i].ScreenMessage != null)
                {
                    int new_screen_index = i;

                    if (ActiveScreens[i].ScreenMessage.NextScreen != null)
                    {
                        // When the screen asks for a new screen, just add and initialize it right away.
                        ActiveScreens[i].ScreenMessage.NextScreen.Initialize();
                        ActiveScreens[i].ScreenMessage.NextScreen.LoadContent(Content, GD);

                        // Insert the new screen in the stack, at the requested position.
                        if (ActiveScreens[i].ScreenMessage.ScreenStackPos == TransitionMessage.ScreenStackPosEnum.BELOW)
                        {
                            ActiveScreens.Insert(i, ActiveScreens[i].ScreenMessage.NextScreen);
                            new_screen_index = i + 1;
                        }
                        else if (ActiveScreens[i].ScreenMessage.ScreenStackPos == TransitionMessage.ScreenStackPosEnum.FRONT)
                        {
                            ActiveScreens.Insert(i + 1, ActiveScreens[i].ScreenMessage.NextScreen);
                        }
                        else if (ActiveScreens[i].ScreenMessage.ScreenStackPos == TransitionMessage.ScreenStackPosEnum.CUSTOM)
                        {
                            ActiveScreens.Insert(ActiveScreens[i].ScreenMessage.ScreenStackIndex, ActiveScreens[i].ScreenMessage.NextScreen);
                            if(ActiveScreens[i].ScreenMessage.ScreenStackIndex <= i)
                                new_screen_index = i + 1;
                        }
                    }


                    // When the screen is done, remove it from the Screens list.
                    if (ActiveScreens[new_screen_index].ScreenMessage.NextAction == TransitionMessage.NextActionEnum.DEAD)
                    {
                        ActiveScreens[new_screen_index].UnloadContent();
                        ActiveScreens.Remove(ActiveScreens[new_screen_index]);
                    }
                    // When we treated the message, delete it so that we won't read it again next time.
                    else ActiveScreens[new_screen_index].ScreenMessage = null; 
                }

            }


            foreach (Screen screen in ActiveScreens)
            {
                screen.Update(gameTime, input);
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



    public class TransitionMessage
    {
        public Screen NextScreen;

        public enum NextActionEnum
        {
            DEAD,
            FADING_OUT
        }
        public NextActionEnum NextAction;

        public enum ScreenStackPosEnum
        {
            BELOW,
            FRONT,
            CUSTOM // Custom number is chosen with ScreenStackIndex.
        }
        public ScreenStackPosEnum ScreenStackPos;
        public int ScreenStackIndex;

        public TransitionMessage(Screen next_screen, NextActionEnum next_action, ScreenStackPosEnum stack_pos, int screen_index = 0)
        { // Complete transition toward a new screen
            NextScreen = next_screen;
            NextAction = next_action;
            ScreenStackPos = stack_pos;
            ScreenStackIndex = screen_index;
        }

        public TransitionMessage(NextActionEnum next_action = NextActionEnum.DEAD)
        { // Just to say that the screen is dead
            NextAction = next_action;

            NextScreen = null;
            ScreenStackPos = 0;
        }
    }
}