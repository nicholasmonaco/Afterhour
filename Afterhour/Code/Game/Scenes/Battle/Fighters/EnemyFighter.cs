using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Game.Scenes.Overworld.Entities;

namespace Afterhour.Code.Game.Scenes.Battle.Fighters {
    public class EnemyFighter : Fighter{

        public int enemyID;
        private Vector2 standbyScreenPos;
        public Vector2 curScreenPos;

        public int level = 1;

        public int curState;

        public bool isFighting = false;
        public bool hasAttacked = true;

        private FrameAnim idleAnim;
        private List<FrameAnim> fightAnims = new List<FrameAnim>();
        private FrameAnim fleeAnim;

        public Vector2 baseDims;

        private SpriteFont healthFont;
        public int curHealth = 0;
        public int maxHealth = 1;



        public EnemyFighter(int enemyID, int level, Vector2 standbyScreenPos) {
            this.enemyID = enemyID;
            this.level = level;
            this.standbyScreenPos = standbyScreenPos;
            this.curScreenPos = standbyScreenPos;
        }

        public override void LoadContent(Resources res) {
            base.LoadContent(res);

            List<int> idleAnimData = res.GetBasicBattleAnimData(this.enemyID, "idle"); //This block loads the idle animation
            Texture2D idleAnimSheet = res.GetBasicBattleSpriteSheet(this.enemyID, "idle");
            this.idleAnim = new FrameAnim(idleAnimSheet, idleAnimData[0], idleAnimSheet.Width / idleAnimData[0], idleAnimSheet.Height, idleAnimData[1], true, true);

            this.baseDims = new Vector2(idleAnimSheet.Width / idleAnimData[0], idleAnimSheet.Height);


            List<int> fleeAnimData = res.GetBasicBattleAnimData(this.enemyID, "flee"); //This block loads the fleeing animation
            Texture2D fleeAnimSheet = res.GetBasicBattleSpriteSheet(this.enemyID, "flee");
            this.fleeAnim = new FrameAnim(fleeAnimSheet, fleeAnimData[0], fleeAnimSheet.Width / fleeAnimData[0], fleeAnimSheet.Height, fleeAnimData[1], true, false);

            List<Texture2D> fightAnimSheets = res.GetFightBattleSpriteSheets(this.enemyID); //This block loads all of the fighting animations
            List<List<int>> fightAnimSheetsData = res.GetFightBattleAnimData(this.enemyID);
            for(int i = 0; i < fightAnimSheets.Count(); i++) {
                Texture2D sheetTex = fightAnimSheets[i];
                List<int> frameData = fightAnimSheetsData[i];
                this.fightAnims.Add(new FrameAnim(sheetTex, frameData[0], sheetTex.Width / frameData[0], sheetTex.Height, frameData[1], true, false));
            }

            this.healthFont = res.Battle_HealthFont;
            this.maxHealth = Enemy.getMaxHealthFromID(this.enemyID);
            this.curHealth = this.maxHealth;
        }

        public override void Update(double fightTimeMS, int curState) {
            this.curState = curState;
            switch (this.curState) {
                case Fighter.STATE_IDLE:
                    StandbyUpdate(fightTimeMS);
                    break;
                case Fighter.STATE_FLEE:

                    break;
                case Fighter.STATE_FIGHT:
                    FightUpdate(fightTimeMS);
                    break;
            }
        }

        public override void Draw(SpriteBatch sb) {
            switch (this.curState) {
                case Fighter.STATE_IDLE:
                    StandbyDraw(sb);
                    break;
                case Fighter.STATE_FLEE:

                    break;
                case Fighter.STATE_FIGHT:
                    FightDraw(sb);
                    break;
            }
        }


        //Standby Logic & Rendering
        public virtual void StandbyUpdate(double fightTimeMS) {
            this.idleAnim.Update(fightTimeMS);
        }

        public virtual void StandbyDraw(SpriteBatch sb) {
            //System.Diagnostics.Debug.WriteLine("sb: " + sb + " |  idleAnim: " + idleAnim + " |   " + this.standbyScreenPos);
            
            this.idleAnim.Draw(sb, this.standbyScreenPos);
            sb.Draw(this.healthBarTex_empty, new Vector2(this.standbyScreenPos.X + 2, this.standbyScreenPos.Y - 10), Color.White);
            sb.Draw(this.healthBarTex_full, new Rectangle((int)this.standbyScreenPos.X + 2, (int)this.standbyScreenPos.Y - 10,
                                                          (int)(this.healthBarTex_full.Width * ((double)curHealth/maxHealth)), this.healthBarTex_full.Height), Color.White);

            sb.DrawString(healthFont, curHealth + "/" + maxHealth, new Vector2((int)this.standbyScreenPos.X - 2, (int)this.standbyScreenPos.Y - 30), Color.Red);
        }
        //

        //Fighting Logic & Rendering
        public virtual void FightUpdate(double fightTimeMS) {
            if (!isFighting) {
                this.idleAnim.Update(fightTimeMS);
            } else {

            }
        }

        public virtual void FightDraw(SpriteBatch sb) {
            if (!isFighting) {
                this.idleAnim.Draw(sb, this.standbyScreenPos);
            }else {

            }
        }
        //

    }
}
