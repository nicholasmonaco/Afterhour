using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Game.Scenes.Overworld.Entities.NPCs {
    public class Villager : NPC {

        public Vector2 worldPos { get; set; }
        private Vector2 screenPos;

        private bool drawWelcomeBox = false;
        private bool openingBox = false;
        private bool closingBox = false;

        private FrameAnim villagerAnim;

        private FrameAnim welcomeBoxOpenAnimation;
        private FrameAnim welcomeBoxCloseAnimation;
        private Texture2D welcomeBoxTex;
        private SpriteFont welcomeBoxFont;
        private String welcomeBoxMessage = "Hello!";


        //

        public Villager(Vector2 worldPos) {
            this.worldPos = worldPos;
            this.screenPos = Vector2.Zero;
        }

        public override void LoadContent(Resources res) {
            this.villagerAnim = new FrameAnim(res.NPC_VillagerAnim, 2, 32, 64, 1000, true, true);

            this.welcomeBoxTex = res.World_Interaction_TalkWindowOpen;
            this.welcomeBoxOpenAnimation = new FrameAnim(res.World_Interaction_TalkWindowAnim, 15, 64, 32, 1, true, false);
            this.welcomeBoxCloseAnimation = new FrameAnim(res.World_Interaction_TalkWindowAnim, 15, 64, 32, 1, false, false);
        }

        public override void Update(GameHandler gameHandler) { //Check if the player is within a certain distance
            this.screenPos = Camera.WorldToScreen(worldPos);

            this.villagerAnim.Update(gameHandler.gameTime);

            //drawWelcomeBox = false;

            if (this.worldPos.X - gameHandler.player.worldPos.X <= 50) {
                if (this.worldPos.Y - gameHandler.player.worldPos.Y <= 50) {
                    if(openingBox == false) {
                        drawWelcomeBox = true;
                        openingBox = true;
                        closingBox = false;
                        welcomeBoxOpenAnimation.loops = 0;
                        welcomeBoxCloseAnimation.loops = 0;
                    }
                }else {
                    closingBox = true;
                    openingBox = false;
                }
            } else {
                closingBox = true;
                openingBox = false;
            }

            if (drawWelcomeBox) {
                if (openingBox) {
                    if(welcomeBoxOpenAnimation.loops <= 0) {
                        welcomeBoxOpenAnimation.Update(gameHandler.gameTime);
                    }else {
                        closingBox = true;
                    }
                }else if(closingBox){
                    if (welcomeBoxCloseAnimation.loops <= 0) {
                        welcomeBoxCloseAnimation.Update(gameHandler.gameTime);
                    }else {
                        closingBox = false;
                        drawWelcomeBox = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb) {
            this.villagerAnim.Draw(sb, this.screenPos);

            if (drawWelcomeBox) {
                if (openingBox) {
                    if (welcomeBoxOpenAnimation.loops <= 0) {
                        welcomeBoxOpenAnimation.Draw(sb, this.screenPos);
                    }else {
                        sb.Draw(this.welcomeBoxTex, new Rectangle((int)this.screenPos.X, (int)this.screenPos.Y, welcomeBoxTex.Width, welcomeBoxTex.Height), Color.White);
                    }
                } else if(closingBox){
                    if (welcomeBoxCloseAnimation.loops <= 0) {
                        welcomeBoxCloseAnimation.Draw(sb, this.screenPos);
                    }
                }
            }
        }

    }
}
