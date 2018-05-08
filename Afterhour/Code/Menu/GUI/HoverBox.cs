using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Menu {
    public class HoverBox {

        private Texture2D tex;
        public String text;

        private Vector2 pos;
        public Rectangle bounds;

        public bool hovering = false;



        public HoverBox(Vector2 pos) {
            this.pos = pos;
        }


        public void LoadContent(Texture2D tex, String text) {
            this.tex = tex;
            this.text = text;

            this.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, tex.Width, tex.Height);
        }

        public void Update(InputHandler input) {
            hovering = false;
            if (this.bounds.Contains(input.mouseState.Position)) {
                hovering = true;
            }


        }


        public void Draw(SpriteBatch sb, SpriteFont font) {
            sb.Draw(this.tex, this.bounds, Color.White);

            
        }

    }
}
