using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HorsesAndGun
{
    internal class InputManager : Singleton<InputManager>
    {
        KeyboardState mCurrentKeyState;
        KeyboardState mPreviousKeyState;

        public void Update(GameTime gameTime)
        {
            mPreviousKeyState = mCurrentKeyState;

            mCurrentKeyState = Keyboard.GetState();
        }

        public bool AllKeysPressed(Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (!KeyPressed(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AnyKeysPressed(Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if(KeyPressed(key))
                {
                    return true;
                }
            }

            return false;
        }

        public bool KeyPressed(Keys key)
        {
            return mCurrentKeyState.IsKeyDown(key) && !mPreviousKeyState.IsKeyDown(key);
        }
    }
}
