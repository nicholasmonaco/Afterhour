using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Afterhour.Code.Game.Scenes;
using Afterhour.Code.Game.Scenes.Overworld.Entities;
using Afterhour.Code.Game.Scenes.Overworld.Map;
using Afterhour.Code.Handling.FileHandling;

namespace Afterhour.Code.Handling {
    public class GameHandler {

        //Variables
        public InputHandler inputHandler { get; set; }
        public GameTime gameTime { get; set; }

        public int curSceneID { get; set; }

        public Player player { get; set; }
        public TileMap tileMap { get; set; }

        public bool drawMinimap { get; set; } = true;

        public WorldData worldData { get; set; }


        //Battle Variables
        public bool switchingToBattle = false;
        public bool switchingToOverworld = false;
        public List<int> battle_enemyIDs;

        public Enemy lastEncounteredEnemy;

        public bool ranFromBattle = false;
        //
       
        //


        public GameHandler() {

        }


        public GameHandler(InputHandler ih, GameTime gt) {
            this.inputHandler = ih;
            this.gameTime = gt;
        }



        public void startBattle(Enemy encounteredEnemy) {
            List<int> entityIDs = encounteredEnemy.fighterList;
            this.switchingToBattle = true;
            this.battle_enemyIDs = entityIDs;

            this.lastEncounteredEnemy = encounteredEnemy;
        }

        public void winBattle(int winExp, int winMoney) { //maybe add winItems later for loot
            //System.Diagnostics.Debug.WriteLine("curxp before win: " + this.worldData.curExp);

            this.worldData.curExp += winExp;
            this.worldData.curMoney += winMoney;

            this.switchingToOverworld = true;
            this.switchingToBattle = false;

            //System.Diagnostics.Debug.WriteLine("curxp after win: " + this.worldData.curExp);
        }


    }
}
