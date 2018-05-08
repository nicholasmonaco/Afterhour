using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Menu {
    public class ClickBox {

        private Texture2D tex;

        private Vector2 pos;
        private Rectangle bounds;

        public bool clicked { get; set; } = false;



        public ClickBox(Vector2 pos) {
            this.pos = pos;
        }


        public void LoadContent(Texture2D tex) {
            this.tex = tex;

            this.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, tex.Width, tex.Height);
        }

        public void Update(InputHandler input) {
            if (input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) {
                if (this.bounds.Contains(input.mouseState.Position)) {
                    clicked = true;
                }
            }
        }


        public void Draw(SpriteBatch sb) {
            sb.Draw(this.tex, this.bounds, Color.White);
        }

    }
}
