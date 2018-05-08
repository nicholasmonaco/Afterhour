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
    public class NextButton {

        private Vector2 pos;
        private String text;
        private Texture2D buttonTex;
        private SpriteFont font;
        private Rectangle bounds;

        public bool clicked { get; set; } = false;

        private Color curColor = Color.White;


        public NextButton(Vector2 pos, String text) {
            this.pos = pos;
            this.text = text;
        }


        public void LoadContent(Resources res) {
            this.buttonTex = res.Menu_NextButton;
            this.font = res.Menu_CreationFont;

            this.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, buttonTex.Width, buttonTex.Height);
        }

        public void Update(InputHandler input) {
            this.curColor = Color.White;
            if (this.bounds.Contains(input.mouseState.Position)) {
                if (input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) {
                    clicked = true; //I don't think there needs to be a part where this turns back false, because another one will just be created instead.
                }
                this.curColor = Color.Purple;
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.buttonTex, new Rectangle((int)this.pos.X, (int)this.pos.Y, this.buttonTex.Width, this.buttonTex.Height), Color.White);
            sb.DrawString(this.font, this.text, new Vector2(this.pos.X + (this.buttonTex.Width - this.font.MeasureString(this.text).X)/2, this.pos.Y + (this.buttonTex.Height - this.font.MeasureString(this.text).Y) / 2), curColor); 
        }

    }
}
