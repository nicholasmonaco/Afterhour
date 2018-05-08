using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Game.Scenes.Overworld.Menus {
    public class ExpBar {

        private Texture2D ExpBarTex_Frame;
        private Texture2D ExpBarTex_Fill;
        private Texture2D ExpBarTex_Indexers;

        private double fillWidth = 0;

        private int origExp;
        private int curExp;

        private int curIncVal = 1;
        private double curIncreaseSpeed = 1;


        public ExpBar() {

        }

        public void LoadContent(Resources res) {
            ExpBarTex_Frame = res.Inventory_ExpBar_Frame;
            ExpBarTex_Fill = res.Inventory_ExpBar_Fill;
            ExpBarTex_Indexers = res.Inventory_ExpBar_Indexers;
        }

        public void Update(double elapsedMS, int initialXP) {
            this.fillWidth = (initialXP % 99.0) / 99.0;
            //System.Diagnostics.Debug.WriteLine("remainder from mod: " + fillWidth);

            if (curIncVal != 1) {
                if(curExp != (origExp + curIncVal)) {
                    curExp += (int)(curIncVal * curIncreaseSpeed);
                }
            }
        }

        public void Draw(SpriteBatch sb, Vector2 drawPos) {
            sb.Draw(ExpBarTex_Frame, drawPos, Color.White);
            sb.Draw(ExpBarTex_Fill, new Rectangle((int)drawPos.X+1, (int)drawPos.Y, (int)(ExpBarTex_Fill.Width * this.fillWidth), ExpBarTex_Fill.Height), Color.White);
            sb.Draw(ExpBarTex_Indexers, drawPos, Color.White);
        }


        public void IncreaseExp(int incVal, double increaseSpeed) {
            origExp = curExp;
            this.curIncreaseSpeed = increaseSpeed;
            this.curIncVal = incVal;
        }

    }
}
