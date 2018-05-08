using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Game.Scenes.Overworld.Entities.Enemies {
    public class Slime : Enemy {

        private Texture2D tex;
        private FrameAnim anim;

        private Vector2 screenPos;

        private double speed = 2;


        public Slime(Vector2 worldPos) {
            superPos = worldPos;
            this.worldPos = worldPos;
        }


        public override void LoadContent(Resources res) {
            this.enemyID = Enemy.ID_SLIME;
            this.tex = res.Enemy_Slime;

            this.anim = getFrameAnimFromID(res);

            this.level = 1;

            this.PopulateWithThisFighterRandomCount();
            maxHealth = 30;

            this.Bounds = new Bounds((int)this.superPos.X, (int)this.superPos.Y, (int)(this.anim.frameWidth * this.size), (int)(this.anim.frameHeight * this.size));
        }

        public override void Update(GameHandler gameHandler) {
            this.distFromPlayer = Math.Sqrt(Math.Pow(this.worldPos.X - gameHandler.player.worldPos.X, 2) + Math.Pow(this.worldPos.Y - gameHandler.player.worldPos.Y, 2)); //replace this with the Vector2.Distance() method

            if (this.distFromPlayer <= (8*32)) {
                TrackPlayer(gameHandler.player, gameHandler.gameTime);
            }

            this.screenPos = Camera.WorldToScreen(this.worldPos);

            this.anim.Update(gameHandler.gameTime.ElapsedGameTime.Milliseconds);

            superPos = worldPos;
        }

        public override void Draw(SpriteBatch sb) {
            //sb.Draw(this.tex, new Rectangle((int)this.screenPos.X, (int)this.screenPos.Y, this.tex.Width, this.tex.Height), Color.White);
            this.anim.Draw(sb, this.screenPos, (float)this.size);
        }


        private void TrackPlayer(Player p, GameTime gameTime) {
            if(this.worldPos.X < p.worldPos.X) {
                this.worldPos.X += (int)(speed);
            }else if (this.worldPos.X > p.worldPos.X) {
                this.worldPos.X -= (int)(speed);
            }

            if (this.worldPos.Y < p.worldPos.Y) {
                this.worldPos.Y += (int)(speed);
            } else if (this.worldPos.Y > p.worldPos.Y) {
                this.worldPos.Y -= (int)(speed);
            }


            if(this.distFromPlayer <= this.speed*4) {
                engaged = true;
            }
        }

    }
}
