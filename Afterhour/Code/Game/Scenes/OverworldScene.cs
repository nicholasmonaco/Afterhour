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
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.Game.Scenes.Overworld;
using Afterhour.Code.Game.Scenes.Overworld.Menus;
using Afterhour.Code.Handling.FileHandling;

namespace Afterhour.Code.Game.Scenes {
    public class OverworldScene : Scene {

        public World gameWorld;

        private Texture2D fadeTex;

        private const double fadeTimer_max = 600;
        private double fadeTimer = 601;
        private bool wasFading = false;
        public bool readyForBattle = false;

        private InventoryMenu inventory;
        private bool isInventoryOpen = false;



        public OverworldScene() {

        }


        public override void LoadContent(Resources res, GameHandler gameHandler) {
            this.gameWorld = new World();
            this.gameWorld.LoadContent(res);

            this.inventory = new InventoryMenu();
            this.inventory.LoadContent(res);

            this.fadeTex = res.Battle_FadeRect;
        }

        public override void Update(GameHandler gameHandler) {
            if(fadeTimer < fadeTimer_max) {
                fadeTimer += gameHandler.gameTime.ElapsedGameTime.Milliseconds;
                wasFading = true;
            } else {
                if (wasFading) {
                    readyForBattle = true;
                    wasFading = false;
                }

                if (gameHandler.inputHandler.keyboardState.IsKeyUp(Keys.E) && gameHandler.inputHandler.keyboardState_old.IsKeyDown(Keys.E)) { //Checks to see if the inventory is opened
                    isInventoryOpen = !isInventoryOpen;
                }

                if (isInventoryOpen) {
                    UpdateInventory(gameHandler);
                } else {
                    this.gameWorld.Update(gameHandler);

                    
                }

            }

        }

        public override void Draw(SpriteBatch sb) {} //This is the inherited method from the State class

        public void Draw(SpriteBatch sb, GFXHandler gh) { //This is the one that is used
            this.gameWorld.Draw(sb, gh);


            if (isInventoryOpen) {
                this.inventory.Draw(sb);
            }

            if (wasFading) {
                sb.Draw(fadeTex, new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.X), Color.White * (float)(fadeTimer / fadeTimer_max));
            }
        }


        public void LoadInventory(SaveData saveData) {
            this.inventory.Reconstruct(saveData);
        }
        

        public void UpdateInventory(GameHandler gameHandler) {
            this.inventory.Update(gameHandler);
        }



        public void winBattle() {
            this.fadeTimer = fadeTimer_max;
            wasFading = false;
        }

        public void startBattle(GameHandler gameHandler) {
            this.fadeTimer = 0;
            wasFading = true;
            gameHandler.switchingToBattle = false;
        }


    }
}
