using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAGameLibrary.Input
{
    public class Input
    {
        private static KeyboardState lastState, currentState;
        private static MouseState lastMouseState, currentMouseState;

        public static void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return (!lastState.IsKeyDown(key) && currentState.IsKeyDown(key));
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool IsMouseClicked()
        {
            return (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static Point GetMousePosition()
        {
            return new Point(currentMouseState.X, currentMouseState.Y);
        }

        public static int GetMouseScrollValue()
        {
            int value = currentMouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;

            if (value > 0)
                return 1;
            else if (value < 0)
                return -1;
            else
                return 0;
        }
    }
}
