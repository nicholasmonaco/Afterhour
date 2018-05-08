using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Afterhour.Code.Game.Scenes.Overworld {
    static class Camera {

        public static int viewWidth { get; set; }
        public static int viewHeight { get; set; }
        public static int worldWidth { get; set; }
        public static int worldHeight { get; set; }

        public static Vector2 displayOffset { get; set; }

        public static Vector2 location = Vector2.Zero; //Default camera location

        public static Vector2 Location { //Current camera location
            get {
                return location;
            }
            set {
                location = new Vector2(MathHelper.Clamp(value.X, 0f, worldWidth - viewWidth),    //x
                                       MathHelper.Clamp(value.Y, 0f, worldHeight - viewHeight)); //y
            }
        }


        public static Vector2 WorldToScreen(Vector2 worldPos) {
            return (worldPos - Location + displayOffset);
        }

        public static Vector2 ScreenToWorld(Vector2 screenPos) {
            return (screenPos + Location - displayOffset);
        }

        public static void Move(Vector2 offset) {
            Location += offset; //Adds or subtracts based on negative/positive values
        }


    }
}
