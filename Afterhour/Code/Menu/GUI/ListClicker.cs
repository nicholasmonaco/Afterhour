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
    public class ListClicker {

        private Vector2 pos;
        private String elementTitle;
        private int elementCount;
        public int curElementID { get; set; } = 0;
        private SpriteFont font;
        private Texture2D arrowTex;

        private Vector2 textDims;

        private Rectangle leftArrowRect;
        private Rectangle rightArrowRect;


        public ListClicker(Vector2 pos, String elementTitle, int elementCount) {
            this.pos = pos;
            this.elementTitle = elementTitle;
            this.elementCount = elementCount;
        }


        public void LoadContent(Resources res) {
            this.arrowTex = res.Menu_ListClicker_Arrow;
            this.font = res.Menu_CreationFont;

            this.textDims = font.MeasureString(this.elementTitle + (curElementID + 1).ToString());

            this.leftArrowRect = new Rectangle((int)this.pos.X, (int)this.pos.Y, arrowTex.Width, arrowTex.Height);
            this.rightArrowRect = new Rectangle((int)(this.pos.X + arrowTex.Width + textDims.X + 20), (int)this.pos.Y, arrowTex.Width, arrowTex.Height);
        }

        public void Update(InputHandler input) {
            if(input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) {
                Point mousePos = input.mouseState.Position;
                if (leftArrowRect.Contains(mousePos)) {
                    if(curElementID <= 0) {
                        curElementID = elementCount - 1;
                    } else {
                        curElementID--;
                    }
                }else if (rightArrowRect.Contains(mousePos)) {
                    if (curElementID >= elementCount - 1) {
                        curElementID = 0;
                    } else {
                        curElementID++;
                    }
                }
                this.textDims = font.MeasureString(this.elementTitle + (curElementID + 1).ToString());
                this.rightArrowRect = new Rectangle((int)(this.pos.X + arrowTex.Width + textDims.X + 20), (int)this.pos.Y, arrowTex.Width, arrowTex.Height);
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(arrowTex, leftArrowRect, Color.White);
            sb.Draw(arrowTex, rightArrowRect, arrowTex.Bounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);

            sb.DrawString(this.font, this.elementTitle + (curElementID+1).ToString(), new Vector2(this.pos.X + this.arrowTex.Width + 10, this.pos.Y +(Math.Abs(textDims.Y - arrowTex.Height) / 2)), Color.White);
        }

    }
}
