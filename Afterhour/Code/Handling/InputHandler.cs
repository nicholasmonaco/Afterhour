using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Afterhour.Code.Handling {
    public class InputHandler {

        public static List<String> Alpha = new List<String> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                              "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public KeyboardState keyboardState { get; set; }
        public KeyboardState keyboardState_old { get; set; }
        public MouseState mouseState { get; set; }
        public MouseState mouseState_old { get; set; }


        public InputHandler() {}

        public InputHandler(KeyboardState keyboardState, MouseState mouseState) {
            this.keyboardState = keyboardState;
            this.keyboardState_old = keyboardState;
            this.mouseState = mouseState;
            this.mouseState_old = mouseState;
        }


        public void UpdateVars(KeyboardState newKeyboardState, MouseState newMouseState) {
            this.keyboardState_old = keyboardState;
            this.mouseState_old = mouseState;
            this.keyboardState = newKeyboardState;
            this.mouseState = newMouseState;
        }

    }
}
