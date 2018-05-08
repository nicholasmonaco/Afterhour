using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.Game.Scenes.Overworld;
using Afterhour.Code.Handling.FileHandling;
using Afterhour.Code.Game.Scenes;

namespace Afterhour.Code.States {
    public class GameState : State {

        //private Texture2D mousePointerTex;
        private FrameAnim mousePointer;


        private GameHandler gameHandler;

        public OverworldScene worldScene;

        private bool lastTickWasBattle = false;
        public BattleScene battleScene;


        //Save Data Stuff
        public SaveData playerData;

        //


        //
        public GameState() {

        }


        public override void LoadContent(Resources res) {
            //this.mousePointerTex = res.Menu_MousePointer;
            this.mousePointer = new FrameAnim(res.Menu_MousePointer_anim, 6, 16, 16, 80, false, false);


            this.gameHandler = new GameHandler();
            this.gameHandler.inputHandler = new InputHandler(Keyboard.GetState(), Mouse.GetState());
            this.gameHandler.curSceneID = Scene.ID_OVERWORLD;

            this.worldScene = new OverworldScene();
            this.worldScene.LoadContent(res, gameHandler);

            this.battleScene = new BattleScene();
            this.battleScene.LoadContent(res, gameHandler);
        }

        public override void Update(StateHandler sh, GameTime gameTime) {
            this.gameHandler.gameTime = gameTime;
            this.gameHandler.inputHandler.UpdateVars(Keyboard.GetState(), Mouse.GetState());


            if(gameHandler.curSceneID == Scene.ID_OVERWORLD) {
                this.worldScene.Update(gameHandler);
                this.lastTickWasBattle = false;

                if (gameHandler.switchingToBattle) {
                    this.worldScene.startBattle(this.gameHandler);
                    if (this.worldScene.readyForBattle) {
                        gameHandler.curSceneID = Scene.ID_BATTLE;
                    }
                }

            } else if(gameHandler.curSceneID == Scene.ID_BATTLE) {
                if(this.lastTickWasBattle == false) {
                    this.battleScene.startBattle(gameHandler.battle_enemyIDs, 0, 0, this.gameHandler); //Replace these zeroes with valus from a "getBattleStageDataFromID" method
                    this.lastTickWasBattle = true;
                }

                this.battleScene.Update(this.gameHandler);

                if (gameHandler.switchingToOverworld) {
                    this.battleScene.winBattle(this.gameHandler);
                    this.worldScene.winBattle();
                    if (this.battleScene.readyForOverworld) {
                        gameHandler.curSceneID = Scene.ID_OVERWORLD;
                        this.worldScene.gameWorld.zones[this.worldScene.gameWorld.curZoneID].enemies.Remove(gameHandler.lastEncounteredEnemy);
                    }
                }
            }






            //PLACEHOLDER
            if(gameHandler.inputHandler.keyboardState_old.IsKeyDown(Keys.Escape) && gameHandler.inputHandler.keyboardState.IsKeyUp(Keys.Escape)) {
                sh.setCurState(State.MENU);
            }
            //PLACEHOLDER


            mousePointer.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb, GFXHandler gh, GameTime gameTime) {
            if (gameHandler.curSceneID == Scene.ID_OVERWORLD) {
                this.worldScene.Draw(sb, gh);
            } else if (gameHandler.curSceneID == Scene.ID_BATTLE) {
                this.battleScene.Draw(sb);
            }



            //sb.Draw(this.mousePointerTex, new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, this.mousePointerTex.Width, this.mousePointerTex.Height), Color.White);
            mousePointer.Draw(sb, new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
        }



        public void UpdateSaveData(SaveData saveData) {
            this.playerData = saveData;
            this.gameHandler.worldData = saveData.worldData;
            worldScene.LoadInventory(saveData);
        }


        public void LoadSave(SaveData saveData) {

        }

    }
}
