using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.FileHandling;
using Afterhour.Code.Handling;
using Afterhour.Code.Game.Scenes.Battle.Fighters;
using Afterhour.Code.Game.Scenes.Battle.UI;

namespace Afterhour.Code.Game.Scenes.Battle {
    public class BattleInst {

        private List<int> enemyIDs;

        private SpriteFont countdownFont;

        private Texture2D backgroundTex;
        private Texture2D floorTex;
        private Texture2D timeSlowOverlayTex;

        private double fightTime;

        private const double timeMod_normal = 1;
        private const double timeMod_slow = 0.3;
        private double curTimeMod = 1;

        private int curBattleState = Fighter.STATE_IDLE;
        private double timeUntilFightState = 0;

        private bool doneSelecting = false;

        private PlayerFighter player;
        private EnemyFighter enemyTemplate;
        private List<EnemyFighter> enemies = new List<EnemyFighter>();

        private Texture2D fighterShadowTex;

        private Vector2 playerScreenPos = new Vector2(200, 368);
        private List<Vector2> enemyScreenPositions = new List<Vector2>() { new Vector2(535, 368), new Vector2(585, 312), new Vector2(585, 424),
                                                                           new Vector2(678, 312), new Vector2(678, 424), new Vector2(628, 368) };

        private OptionWheel mainSelectionWheel;
        private bool drawingFightWheel = false;
        private OptionWheel fightWheel;

        private bool selectingEnemy = false;
        private bool enemySelected = false;
        private int curEnemyHighlightID = 0;
        private int curEnemyHighlightIndex = 0;
        private int finalEnemySelectionID = 0;

        private FrameAnim enemySelectorAnim;
        private List<int> aliveEnemyPositions = new List<int>();
        private int removedCounter = 0;


        //Variables

        public BattleInst(List<int> enemyIDs) {
            this.enemyIDs = enemyIDs;
        }


        public void LoadContent(Resources res, GameHandler gameHandler, int backgroundTexID, int floorTexID) {
            this.countdownFont = res.Battle_CountDownFont;

            this.backgroundTex = res.Battle_BackgroundTexList[backgroundTexID];
            this.floorTex = res.Battle_FloorTexList[floorTexID];

            this.timeSlowOverlayTex = res.Battle_FadeRect;

            this.fighterShadowTex = res.Battle_FighterShadow;

            this.timeUntilFightState = 10 * 1000; //The battle starts with 10 seconds until fightMode

            LoadFighters(res, gameHandler);

            this.mainSelectionWheel = new OptionWheel(new Vector2(player.standbyScreenPos.X + 6, player.standbyScreenPos.Y - 30), 4);
            this.mainSelectionWheel.LoadContent(res);

            //System.Diagnostics.Debug.WriteLine("player attack count: " + this.player.attackList.Count());
            this.fightWheel = new OptionWheel(new Vector2(player.standbyScreenPos.X + 6, player.standbyScreenPos.Y - 30), this.player.attackList.Count());
            List<Texture2D> icons = new List<Texture2D>();
            for(int i = 0; i < this.player.attackList.Count(); i++) {
                icons.Add(res.MoveIconTexDict[Move.ID_DICT[this.player.attackList[i].moveName]]);
            }
            this.fightWheel.LoadContent(res, icons);

            this.enemySelectorAnim = new FrameAnim(res.Battle_UI_SelectorArrowSpriteSheet, 6, 32, 18, 24, true, false);
        }

        public void Update(GameHandler gameHandler) {
            if (curBattleState == Fighter.STATE_IDLE) {
                this.curTimeMod = timeMod_slow;
            } else {
                this.curTimeMod = timeMod_normal;
            }

            this.fightTime = gameHandler.gameTime.ElapsedGameTime.Milliseconds * curTimeMod;


            switch (this.curBattleState) {
                case Fighter.STATE_IDLE: //This is the "selecting moves" part
                    KeyboardState keyboardState = gameHandler.inputHandler.keyboardState;
                    KeyboardState keyboardState_old = gameHandler.inputHandler.keyboardState_old;

                    foreach (EnemyFighter enemy in enemies) {
                        enemy.Update(fightTime, this.curBattleState);
                    }

                    player.Update(fightTime, this.curBattleState);


                    if (!drawingFightWheel) {
                        this.mainSelectionWheel.Update(gameHandler.inputHandler, gameHandler.gameTime);
                        if (keyboardState_old.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space)) {
                            if (!mainSelectionWheel.isRotating) {
                                switch (mainSelectionWheel.curSelectedID) {
                                    case 0: //Attack
                                        //this.timeUntilFightState = 0;
                                        drawingFightWheel = true;
                                        selectingEnemy = false;
                                        enemySelected = false;
                                        break;
                                    case 1: //Item
                                        break;
                                    case 2: //Pass
                                        break;
                                    case 3: //Flee
                                        gameHandler.curSceneID = Scene.ID_OVERWORLD;
                                        gameHandler.ranFromBattle = true;
                                        break;
                                }
                            }
                        }
                    }else { //This is the else that is when fight wheel is being drawn
                        if (selectingEnemy) {
                            if (enemySelected) {
                                //execute selected move
                                this.timeUntilFightState = 10;
                                this.drawingFightWheel = false;
                                player.hasAttacked = false;
                            } else {
                                enemySelectorAnim.Update(fightTime);
                                if (keyboardState_old.IsKeyDown(Keys.W) && keyboardState.IsKeyUp(Keys.W) || keyboardState_old.IsKeyDown(Keys.A) && keyboardState.IsKeyUp(Keys.A)) { //If WA is used for going back
                                    if(curEnemyHighlightIndex <= 0) {
                                        curEnemyHighlightIndex = enemies.Count()-1;
                                    }else {
                                        curEnemyHighlightIndex--;
                                    }
                                    curEnemyHighlightID = /*aliveEnemyPositions[curEnemyHighlightIndex];*/ curEnemyHighlightIndex;

                                } else if(keyboardState_old.IsKeyDown(Keys.S) && keyboardState.IsKeyUp(Keys.S) || keyboardState_old.IsKeyDown(Keys.D) && keyboardState.IsKeyUp(Keys.D)) {  //If SD is used for going up
                                    if (curEnemyHighlightIndex >= enemies.Count()-1) {
                                        curEnemyHighlightIndex = 0;
                                    } else {
                                        curEnemyHighlightIndex++;
                                    }
                                    curEnemyHighlightID = /*aliveEnemyPositions[curEnemyHighlightIndex];*/ curEnemyHighlightIndex;

                                } else if(keyboardState_old.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space)) { //If the spacebar is pressed
                                    finalEnemySelectionID = curEnemyHighlightID;
                                    selectingEnemy = false;
                                    enemySelected = true;
                                    this.player.attackList[player.curAttackID].targetPos = enemyScreenPositions[finalEnemySelectionID];
                                    doneSelecting = true;
                                }

                            }
                        }else {
                            this.fightWheel.Update(gameHandler.inputHandler, gameHandler.gameTime);
                            if (keyboardState_old.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space)) {
                                if (!fightWheel.isRotating) {
                                    selectingEnemy = true;
                                }
                            }
                        }
                    }

                    this.timeUntilFightState -= gameHandler.gameTime.ElapsedGameTime.Milliseconds;
                    if (this.timeUntilFightState <= 0 || doneSelecting) { //This is the conditional for when the timer runs out or the player is done selecting
                        this.curBattleState = Fighter.STATE_FIGHT;

                        doneSelecting = false;
                        selectingEnemy = false;
                        enemySelected = false;
                        drawingFightWheel = false;
                        

                        this.player.hasAttacked = false;
                        foreach (EnemyFighter enemy in enemies) {
                            enemy.hasAttacked = false;
                        }
                    }
                    break;

                case Fighter.STATE_FIGHT: //This is when the fighting takes place, and where health/mana is affected
                    bool doneFighting = true;

                    if (!player.hasAttacked) {
                        player.isFighting = true;

                        //Move updating
                        player.attackList[player.curAttackID].UpdateMove(fightTime);
                        //

                        doneFighting = false;
                        if (player.attackList[player.curAttackID].finished) {

                            enemies[finalEnemySelectionID].curHealth -= gameHandler.worldData.stats[PlayerSaver.STAT_ATK];

                            if(enemies[finalEnemySelectionID].curHealth <= 0) {
                                enemies.Remove(enemies[finalEnemySelectionID]);
                                aliveEnemyPositions.Remove(finalEnemySelectionID);

                                if (enemies.Count() <= 0) {
                                    int wonExp = (int)(EnemyData.expRewardVals[gameHandler.lastEncounteredEnemy.enemyID] * enemyTemplate.level * (new Random().Next(100, 110) / 100.0)); //times the amount of enemies too
                                    int wonMoney = (int)(EnemyData.moneyRewardVals[gameHandler.lastEncounteredEnemy.enemyID] * enemyTemplate.level * (new Random().Next(100, 110) / 100.0));
                                    gameHandler.winBattle(wonExp, wonMoney);
                                }else {
                                    curEnemyHighlightID = aliveEnemyPositions[0];
                                    //this.finalEnemySelectionID = 0;
                                }
                            }

                            //doneFighting = true;
                            player.hasAttacked = true;
                            player.isFighting = false;
                            doneFighting = false;
                            player.attackList[player.curAttackID].finished = false;
                        }
                    }else {

                    }


                    foreach (EnemyFighter enemy in enemies) {
                        enemy.Update(fightTime, this.curBattleState);

                        if (!enemy.hasAttacked) {
                            //doneFighting = false; //This has to be on later, but its commented out so i can see what would happen after the enemies attack
                        }
                    }


                    if (doneFighting) {
                        this.curBattleState = Fighter.STATE_IDLE;
                        this.timeUntilFightState = 8 * 1000; //All subsequent idle stages are 8 seconds, unless modified by an ability
                    }

                    break;

                case Fighter.STATE_FLEE: //This is the case dealing with fleeing the battle
                    foreach (EnemyFighter enemy in enemies) {
                        enemy.Update(fightTime, this.curBattleState);
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.backgroundTex, new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.Y), Color.White);
            sb.Draw(this.floorTex, new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.Y), Color.White);

            if(curBattleState == Fighter.STATE_IDLE) { //This has to be here s othe gray screen is drawn before the actual stuff
                sb.Draw(this.timeSlowOverlayTex, new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.Y), Color.Black * 0.65f);
            }


            foreach (EnemyFighter enemy in enemies) {
                sb.Draw(fighterShadowTex, new Rectangle((int)enemy.curScreenPos.X, (int)enemy.curScreenPos.Y + 3, (int)enemy.baseDims.X, (int)enemy.baseDims.Y), Color.White);
                enemy.Draw(sb);
            }

            sb.Draw(fighterShadowTex, new Rectangle((int)player.curScreenPos.X, (int)player.curScreenPos.Y + 3, (int)player.baseDims.X, (int)player.baseDims.Y), Color.White);
            player.Draw(sb);


            if (curBattleState == Fighter.STATE_IDLE) {
                sb.DrawString(this.countdownFont, String.Format("{0:0.0}", this.timeUntilFightState / 1000), new Vector2(700 - countdownFont.MeasureString(String.Format("{0:0.0}", this.timeUntilFightState / 1000)).X / 2, 30), (this.timeUntilFightState / 1000 <= 2) ? Color.Red : Color.White);

                if (!drawingFightWheel) {
                    this.mainSelectionWheel.Draw(sb);
                } else {
                    if (selectingEnemy) {
                        enemySelectorAnim.Draw(sb, new Vector2(enemyScreenPositions[curEnemyHighlightID].X, enemyScreenPositions[curEnemyHighlightID].Y - 55));
                    } else {
                        this.fightWheel.Draw(sb);
                    }
                }
            } else if (curBattleState == Fighter.STATE_FIGHT) { //this is at the bottom bc moves should alwasy be drawn last
                if (player.isFighting) {
                    player.attackList[player.curAttackID].DrawMove(sb); //this isnt happening after the 1st time, something with these variavles
                } else if (player.hasAttacked) {
                    //enemy attack
                }
            }
        }


        private void LoadFighters(Resources res, GameHandler gameHandler) {
            for (int i = 0; i < /*this.enemyIDs.Count()*/ 1; i++) {
                this.enemies.Add(new EnemyFighter(this.enemyIDs[i], gameHandler.lastEncounteredEnemy.level, enemyScreenPositions[i]));
                this.enemies[i].LoadContent(res);
                this.aliveEnemyPositions.Add(i);
            }
            this.enemyTemplate = enemies[0];

            this.removedCounter = 0;

            this.player = new PlayerFighter(playerScreenPos);

            this.player.attackList = MoveLoader.ConvertIDsToMoves(gameHandler.worldData.unlockedSkillIDs);
            for(int i = 0; i < this.player.attackList.Count(); i++) {
                List<FrameAnim> primaryAnims = new List<FrameAnim>();
                for(int j = 0; j < res.MovePrimaryAnimsPerMove[Move.ID_DICT[this.player.attackList[i].moveName]].Count(); j++) {
                    primaryAnims.Add(res.MovePrimaryAnims[res.MovePrimaryAnimsPerMove[Move.ID_DICT[this.player.attackList[i].moveName]][j]]);
                }
                this.player.attackList[i].LoadContent(primaryAnims, res.MoveSecondaryAnims[Move.ID_DICT[player.attackList[i].moveName]]);
                this.player.attackList[i].PrimeMove(player.standbyScreenPos, enemies[0].curScreenPos, 50, .35);
            }


            
        }

    }
}
