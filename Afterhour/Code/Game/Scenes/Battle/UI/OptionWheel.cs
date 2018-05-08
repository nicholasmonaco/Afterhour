using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Game.Scenes.Battle.UI {
    public class OptionWheel {

        private Vector2 centerSquarePos;
        private int optionCount;

        private FrameAnim highlightAnim;

        private List<Texture2D> optionTextures = new List<Texture2D>();
        private List<Vector2> relativeScreenPositions = new List<Vector2>();
        private List<Vector2> baseRelativeScreenPositions = new List<Vector2>();


        private int radius = 30;

        public bool isRotating = false;
        private int curRotationSign;
        private double curRotationTime = 0;

        public int curRotationIndex = 0;

        public int curSelectedID;

        //

        public OptionWheel(Vector2 pos, int optionCount) {
            this.centerSquarePos = pos;
            this.optionCount = optionCount;
        }

        public void LoadContent(Resources res) {
            highlightAnim = new FrameAnim(res.Battle_UI_OptionWheel_HighlighterSpriteSheet, 2, 20, 20, 200, true, true);

            if(optionCount == 4) {
                this.optionTextures.Add(res.Battle_UI_OptionWheel_Fight);
                this.optionTextures.Add(res.Battle_UI_OptionWheel_Item);
                this.optionTextures.Add(res.Battle_UI_OptionWheel_Pass);
                this.optionTextures.Add(res.Battle_UI_OptionWheel_Flee);
                curSelectedID = 1;
            }

            SolveScreenPositionsByPosID(optionCount);
        }

        public void LoadContent(Resources res, List<Texture2D> wheelTextures) {
            LoadContent(res);
            this.optionTextures = wheelTextures;
        }

        public void Update(InputHandler input, GameTime gameTime) {
            if (!isRotating) {
                if (input.keyboardState.IsKeyDown(Keys.D)) {
                    isRotating = true;
                    curRotationSign = 1;
                } else if (input.keyboardState.IsKeyDown(Keys.A)) {
                    isRotating = true;
                    curRotationSign = -1;
                }
            }




            if (isRotating) {
                Rotate(120, gameTime);
            }
        }

        public void Draw(SpriteBatch sb) {
            for(int i = 0; i < optionCount; i++) {
                sb.Draw(optionTextures[i], this.relativeScreenPositions[i], Color.White);
            }
        }


        public void SolveScreenPositionsByPosID(int optionCount) {
            for(int i = 0; i < optionCount; i++) {
                double curAngle = ((2 * Math.PI) / optionCount) * i;
                double XDistFromCenter = radius * Math.Cos(curAngle);
                double YDistFromCenter = radius * Math.Sin(curAngle);

                this.relativeScreenPositions.Add(new Vector2((int)(this.centerSquarePos.X + XDistFromCenter), (int)(this.centerSquarePos.Y + YDistFromCenter)));
            }
            baseRelativeScreenPositions = relativeScreenPositions;
        }


        private void Rotate(double rotationTime, GameTime gameTime) {
            if(curRotationTime <= rotationTime) {
                for(int i = 0; i < optionCount; i++) {
                    double curAngle = (((2 * Math.PI) / optionCount) * (i + curRotationIndex));
                    double addedAngle = (((2 * Math.PI)/optionCount) / rotationTime) * curRotationTime * curRotationSign;
                    double XDistFromCenter = radius * Math.Cos(curAngle + (addedAngle));
                    double YDistFromCenter = radius * Math.Sin(curAngle + (addedAngle));


                    this.relativeScreenPositions[i] = new Vector2((int)(this.centerSquarePos.X +(XDistFromCenter)), (int)(this.centerSquarePos.Y + (YDistFromCenter)));
                }

                curRotationTime += gameTime.ElapsedGameTime.Milliseconds;
            }else {
                for (int i = 0; i < optionCount; i++) {
                    this.baseRelativeScreenPositions[i] = relativeScreenPositions[i];
                }

                isRotating = false;
                curRotationTime = 0;
                curRotationIndex += curRotationSign;

                curSelectedID -= curRotationSign;
                if(curSelectedID >= optionCount) {
                    curSelectedID = 0;
                }else if(curSelectedID < 0){
                    curSelectedID = optionCount - 1;
                }

            }
        }

    }
}
