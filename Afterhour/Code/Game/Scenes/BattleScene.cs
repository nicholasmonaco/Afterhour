using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Game.Scenes.Battle;

namespace Afterhour.Code.Game.Scenes {
    public class BattleScene : Scene{

        private Texture2D fadeTex;

        private const double fadeTimer_max = 600;
        private double fadeTimer = fadeTimer_max;

        private BattleInst curBattle;

        private Resources reloadRes;

        public bool readyForOverworld = true; //This is the fade screen testing variable


        public BattleScene() {

        }


        public override void LoadContent(Resources res, GameHandler gameHandler) {
            this.fadeTex = res.Battle_FadeRect;

            this.reloadRes = res;
        }

        public override void Update(GameHandler gameHandler) {
            if(fadeTimer >= 0) {
                fadeTimer -= gameHandler.gameTime.ElapsedGameTime.Milliseconds;
            }else {
                if(curBattle != null) {
                    curBattle.Update(gameHandler);
                    if (!gameHandler.switchingToBattle) {

                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb) {
            if(curBattle != null) {
                curBattle.Draw(sb);
            }

            if (fadeTimer >= 0) {
                sb.Draw(fadeTex, new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.X), Color.White * (float)(fadeTimer / fadeTimer_max));
            }
        }


        public void startBattle(List<int> enemyIDs, int backgroundTexID, int floorTexID, GameHandler gameHandler) {
            this.fadeTimer = fadeTimer_max;
            this.curBattle = new BattleInst(enemyIDs/*, gameHandler*/);
            this.curBattle.LoadContent(reloadRes, gameHandler, backgroundTexID, floorTexID); //This is a really bad way of doing this I think
        }

        public void winBattle(GameHandler gameHandler) {
            gameHandler.switchingToOverworld = false;
            //fading screen stuff here too
        }

    }
}
