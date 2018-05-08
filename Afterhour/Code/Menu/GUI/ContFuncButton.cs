using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Menu.GUI {
    public class ContFuncButton {

        public const int PLAY = 0;
        public const int DELETE = 1;
        //

        private int funcID;

        private Texture2D tex;
        private Vector2 pos;
        private Rectangle bounds;

        public bool clicked = false;
        //
        public ContFuncButton(int funcID, Vector2 pos) {
            this.funcID = funcID;
            this.pos = pos;
        }


        public void LoadContent(Texture2D tex) {
            switch (funcID) {
                case PLAY:
                    this.tex = tex;
                    break;
                case DELETE:
                    this.tex = tex;
                    break;
            }

            this.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, this.tex.Width, this.tex.Height);
        }

        public void Update(InputHandler input) {
            if (input.mouseState.LeftButton == ButtonState.Pressed && input.mouseState_old.LeftButton == ButtonState.Released) {
                if (this.bounds.Contains(input.mouseState.Position)) {
                    this.clicked = true;
                }
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.tex, this.bounds, Color.White);
        }

    }
}
