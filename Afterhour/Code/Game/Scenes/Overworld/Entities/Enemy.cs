using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;
using Afterhour.Code.Game.Scenes.Overworld.Entities.Enemies;


namespace Afterhour.Code.Game.Scenes.Overworld.Entities {
    public class Enemy : Entity{

        public const int ID_SLIME = Entity.ID_ENEMY_SLIME;


        public const int MAX_ID = Entity.MAX_ENEMYID;
        //

        protected Vector2 worldPos;

        public bool removeFlag = false;

        public double distFromPlayer;
        public bool engaged = false;
        public bool beaten = false;
        public bool stunned = false;

        public int level = 0;

        public List<int> fighterList = new List<int>();

        public int enemyID;

        public static int maxHealth;

        public Vector2 superPos = Vector2.Zero;


        public virtual void LoadContent(Resources res) {

        }

        public virtual void Update(GameHandler gameHandler) {

        }

        public virtual void Draw(SpriteBatch sb) {

        }


        protected void PopulateWithThisFighter(int count) {
            for(int i = 0; i < count; i++) {
                this.fighterList.Add(enemyID);
            }
        }

        protected void PopulateWithThisFighterRandomCount() {
            Random rand = new Random();
           int count = (rand.Next(5) + 1);
            for (int i = 0; i < count; i++) {
                this.fighterList.Add(enemyID);
            }
        }

        public static int getMaxHealthFromID(int id) {
            int health = 1;
            switch (id) {
                case ID_SLIME:
                    health = Slime.maxHealth;
                    break;
            }
            return health;
        }


        protected FrameAnim getFrameAnimFromID(Resources res) {
            List<int> idleAnimData = res.GetBasicBattleAnimData(this.enemyID, "idle");
            Texture2D idleAnimSheet = res.GetBasicBattleSpriteSheet(this.enemyID, "idle");
            return new FrameAnim(idleAnimSheet, idleAnimData[0], idleAnimSheet.Width / idleAnimData[0], idleAnimSheet.Height, idleAnimData[1], true, true);
        }

    }
}
