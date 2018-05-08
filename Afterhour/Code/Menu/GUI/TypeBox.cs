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
    public class TypeBox {

        public String text { get; set; } = "";
        private Texture2D boxTex;

        public Rectangle bounds;

        private bool stringFits = true;


        public TypeBox() {
        }

        public TypeBox(String initialText) {
            this.text = initialText;
        }


        public void LoadContent(Resources res) {
            this.boxTex = res.Menu_TypeBox;

            this.bounds = new Rectangle(0, 0, boxTex.Width * 2, boxTex.Height);
        }

        public void Update(InputHandler input) {
            if (stringFits) {
                if (input.keyboardState.GetPressedKeys().Count() > 0) {
                    if (!input.keyboardState_old.GetPressedKeys().Contains(input.keyboardState.GetPressedKeys()[0])) {
                        String pressedKey = input.keyboardState.GetPressedKeys()[0].ToString();
                        if (InputHandler.Alpha.Contains(pressedKey)) {
                            if (input.keyboardState.IsKeyDown(Keys.LeftShift)) {
                                this.text = text + pressedKey;
                            } else {
                                this.text = text + pressedKey.ToLower();
                            }
                        }
                    }
                }
            }

            if(this.text.Length > 0) {
                if (input.keyboardState.IsKeyUp(Keys.Back) && input.keyboardState_old.IsKeyDown(Keys.Back)) {
                    this.text = this.text.Remove(this.text.Length - 1);
                }
            }
        }

        public void Draw(SpriteBatch sb, Vector2 drawPos, SpriteFont font) {
            this.bounds = new Rectangle((int)drawPos.X, (int)drawPos.Y, boxTex.Width*2, boxTex.Height);

            sb.Draw(boxTex, bounds, Color.White);
            sb.DrawString(font, this.text, new Vector2(this.bounds.X + 4, this.bounds.Y + 4), Color.White); //idk how to position this right really, maybe google how to center text in a rectangle?

            if (font.MeasureString(this.text + "W").X >= boxTex.Width*2) {
                stringFits = false;
            }else {
                stringFits = true;
            }
        }


    }
}
