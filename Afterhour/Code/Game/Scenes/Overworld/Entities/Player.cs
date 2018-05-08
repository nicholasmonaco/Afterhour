using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Game.Scenes.Overworld.Entities {
    public class Player : Entity {

        public Vector2 worldPos;
        private Vector2 screenPos;

        private int movementSpeed = 50;

        private FrameAnim spriteAnim;

        private Rectangle bounds;

        private Texture2D debugTex;
        private SoundEffect pulseFX;

        //Variables

        public Player(Vector2 worldPos) {
            this.worldPos = worldPos;
            this.screenPos = Camera.WorldToScreen(worldPos);
        }


        public override void LoadContent(Resources res) {
            spriteAnim = res.MovePrimaryAnims[0]; //This is super placeholder

            this.bounds = new Rectangle((int)this.worldPos.X, (int)this.worldPos.Y, spriteAnim.frameWidth, spriteAnim.frameHeight);

            this.debugTex = res.Debug_BoundsRect;
            this.pulseFX = res.Player_Affinity_ManaPulse;
        }

        public override void Update(GameHandler gameHandler) {
            //System.Diagnostics.Debug.WriteLine("gameHandler.inputHandler: " + gameHandler.inputHandler);

            KeyboardState keyboardState = gameHandler.inputHandler.keyboardState;
            KeyboardState keyboardState_old = gameHandler.inputHandler.keyboardState_old;
            Vector2 offsetVector = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.LeftShift)) {
                movementSpeed = 50;
            } else {
                movementSpeed = 5;
            }

            if (keyboardState.IsKeyDown(Keys.W)) {
                offsetVector.Y = -movementSpeed;
            }else if (keyboardState.IsKeyDown(Keys.S)) {
                offsetVector.Y = movementSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A)) {
                offsetVector.X = -movementSpeed;
            } else if (keyboardState.IsKeyDown(Keys.D)) {
                offsetVector.X = movementSpeed;
            }


            if(!IsColliding(gameHandler.tileMap, this.bounds, new Vector2(offsetVector.X, 0))) { //Checks to see if the player is colliding with anything in the X dimension
                this.worldPos.X += offsetVector.X;
                this.bounds.X = (int)worldPos.X;
            }

            if (!IsColliding(gameHandler.tileMap, this.bounds, new Vector2(0, offsetVector.Y))) { //Checks to see if the player is colliding with anything in the Y dimension
                this.worldPos.Y += offsetVector.Y;
                this.bounds.Y = (int)worldPos.Y;
            }

            //this.worldPos.X += SolveMaxMoveableDistance(gameHandler.tileMap, this.bounds, new Vector2(offsetVector.X, 0)).X;
            //this.worldPos.Y += SolveMaxMoveableDistance(gameHandler.tileMap, this.bounds, new Vector2(0, offsetVector.Y)).Y;


            this.bounds.X = (int)worldPos.X;
            this.bounds.Y = (int)worldPos.Y;


            //Sound FX Testing
            if(keyboardState.IsKeyDown(Keys.F) && keyboardState_old.IsKeyDown(Keys.F)) {
                SoundEffectInstance pulseFXInst =  pulseFX.CreateInstance();
                pulseFXInst.Volume = 0.4f;
                pulseFXInst.Pitch = 0.0f;
                pulseFXInst.Pan = 0.0f;
                pulseFXInst.IsLooped = false;
                pulseFXInst.Play();
            }
            //

            UpdateCamera();
            this.screenPos = Camera.WorldToScreen(worldPos);

            this.spriteAnim.Update(gameHandler.gameTime);

            //System.Diagnostics.Debug.WriteLine(gameHandler.tileMap.GetCellAtWorldPoint(this.worldPos.ToPoint()).TileID);
            //System.Diagnostics.Debug.WriteLine(this.worldPos);
        }

        public override void Draw(SpriteBatch sb) {
            spriteAnim.Draw(sb, screenPos);

            //sb.Draw(this.debugTex, new Rectangle((int)this.screenPos.X, (int)this.screenPos.Y, this.bounds.Width, this.bounds.Height), Color.White);
        }


        public void UpdateCamera() {
            int xDist = 200;
            int yDist = 200;

            Vector2 pos = screenPos;

            if (pos.X < xDist) {
                Camera.Move(new Vector2(pos.X - xDist, 0));
            }
            if (pos.X > (Camera.viewWidth - xDist)) {
                Camera.Move(new Vector2(pos.X - (Camera.viewWidth - xDist), 0));
            }
            if (pos.Y < yDist) {
                Camera.Move(new Vector2(0, pos.Y - yDist));
            }
            if (pos.Y > (Camera.viewHeight - yDist)) {
                Camera.Move(new Vector2(0, pos.Y - (Camera.viewHeight - yDist)));
            }

        }
    }
}
