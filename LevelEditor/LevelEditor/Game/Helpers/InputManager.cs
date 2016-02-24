using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace LevelEditor
{
    public class InputManager
    {
        KeyboardState prevKeyState, keyState;
        GamePadState prevPadState, padState;

        public KeyboardState PrevKeyState
        {
            get { return prevKeyState; }
            set { prevKeyState = value; }
        }

        public KeyboardState KeyState
        {
            get { return keyState; }
            set { keyState = value; }
        }

        public GamePadState PrevPadState
        {
            get { return prevPadState; }
            set { prevPadState = value; }
        }

        public GamePadState PadState
        {
            get { return padState; }
            set { padState = value; }
        }

        public void Update()
        {
            prevKeyState = keyState;
            prevPadState = padState;

            keyState = Keyboard.GetState();
            //if(GamePad.GetState(PlayerIndex.One).IsConnected)
                //padState = GamePad.GetState(PlayerIndex.One);
        }

        public bool KeyPressed(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool ButtonPressed(Buttons button)
        {
            if (padState.IsButtonDown(button) && prevPadState.IsButtonUp(button))
                return true;
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool ButtonPressed(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (padState.IsButtonDown(button) && prevPadState.IsButtonUp(button))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool ButtonReleased(Buttons button)
        {
            if (padState.IsButtonUp(button) && prevPadState.IsButtonDown(button))
                return true;
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool ButtonReleased(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (padState.IsButtonUp(button) && prevPadState.IsButtonDown(button))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (keyState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public bool ButtonDown(Buttons button)
        {
            if (padState.IsButtonDown(button))
                return true;
            else
                return false;
        }

        public bool KeyUp(Keys key)
        {
            if (keyState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        public bool ButtonUp(Buttons button)
        {
            if (padState.IsButtonUp(button))
                return true;
            else
                return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool ButtonDown(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (padState.IsButtonDown(button))
                    return true;
            }
            return false;
        }

        public bool KeyUp(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool ButtonUp(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (padState.IsButtonUp(button))
                    return true;
            }
            return false;
        }
    }
}
