using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Afterhour.Code.Game.Scenes.Overworld.Entities {
    public class Bounds {

        public Vector2 worldPos;
        public int width;
        public int height;
    

        public Bounds(Vector2 pos, int width, int height) {
            this.worldPos = pos;
            this.width = width;
            this.height = height;
        }

        public Bounds(Point pos, int width, int height) {
            this.worldPos = pos.ToVector2();
            this.width = width;
            this.height = height;
        }

        public Bounds(int x, int y, int width, int height) {
            this.worldPos = new Vector2(x, y);
            this.width = width;
            this.height = height;
        }

    }
}
