using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Game.Scenes.Overworld;

namespace Afterhour.Code.Handling.AssetHandling {
    public class FrameAnim {

        private Texture2D spriteSheet;
        private List<Rectangle> frameRects = new List<Rectangle>();
        private int frameCount;
        public int curFrameID = 0;

        public int frameWidth;
        public int frameHeight;

        private double msTimePerFrame;
        private double currentFrameMSTime = 0;

        private bool doReflux;
        private bool shiftingBack = false;

        private float curRotation = 0;

        public int loops = 0;

        //

        public FrameAnim(Texture2D spriteSheet, int frames, int width, int height, double msTimePerFrame, bool forward, bool reflux) {
            this.spriteSheet = spriteSheet;
            for(int i = 0; i < frames; i++) {
                this.frameRects.Add(new Rectangle(width * i, 0, width, height));
            }

            this.frameWidth = width;
            this.frameHeight = height;

            this.frameCount = frames;

            this.msTimePerFrame = msTimePerFrame;

            this.doReflux = reflux;

            if (!forward) {
                shiftingBack = true;
                curFrameID = frameCount - 1;
            }
        }


        public void LoadContent() {
            //I don't really know if this is a nessecary method, most/all of the stuff that would go here just goes in the constructor I think
        }

        public void Update(GameTime gameTime) {
            Update(gameTime.ElapsedGameTime.Milliseconds);
        }

        public void Update(double elapsedMS) {
            if (doReflux == false) { //This is when it runs through the thing once and then resets at frame 0
                if (currentFrameMSTime < msTimePerFrame) {
                    currentFrameMSTime += elapsedMS;
                } else if (currentFrameMSTime >= msTimePerFrame) {
                    currentFrameMSTime = 0;
                    //if (curFrameID <= frameCount - 1 && curFrameID >= 0) {
                    if (shiftingBack && curFrameID >= 0) {
                        curFrameID--;
                    } else if (!shiftingBack && curFrameID <= frameCount - 1) {
                        curFrameID++;
                    }
                    //}
                    if (curFrameID > frameCount - 1 && !shiftingBack) {
                        curFrameID = 0;
                        loops++;
                    } else if (curFrameID < 0 && shiftingBack) {
                        curFrameID = frameCount - 1;
                        loops++;
                    }
                }

            } else { //This is when it runs through and then runs back to frame 0 
                if (currentFrameMSTime < msTimePerFrame) {
                    currentFrameMSTime += elapsedMS;
                } else if (currentFrameMSTime >= msTimePerFrame) {
                    currentFrameMSTime = 0;
                    if (curFrameID < frameCount - 1 && curFrameID > 0) {
                        if (shiftingBack == false) {
                            curFrameID++;
                        } else {
                            curFrameID--;
                        }
                    } else if (curFrameID >= frameCount - 1) {
                        shiftingBack = true;
                        curFrameID--;
                    } else if (curFrameID <= 0) {
                        shiftingBack = false;
                        curFrameID++;
                        loops++;
                    }
                }
            }
        }


        public void Draw(SpriteBatch sb, Vector2 drawPos) {
            sb.Draw(spriteSheet,
                    drawPos,
                    frameRects[curFrameID], //y u be -1 loser
                    Color.White, //tint
                    0f,          //rotation
                    /*new Vector2(frameWidth / 2, frameHeight / 2),*/ //center
                    Vector2.Zero,
                    1f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, Vector2 drawPos, float scale) {
            sb.Draw(spriteSheet,
                    drawPos,
                    frameRects[curFrameID],
                    Color.White, //tint
                    0f,          //rotation
                                 /*new Vector2(frameWidth / 2, frameHeight / 2),*/ //center
                    Vector2.Zero,
                    scale, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, Vector2 drawPos, float scale, float rotationSpeed) {
            this.curRotation += rotationSpeed;
            Vector2 origin = new Vector2((int)this.frameWidth/2, (int)this.frameHeight/2);
            sb.Draw(spriteSheet,
                    drawPos,
                    frameRects[curFrameID],
                    Color.White, //tint
                    this.curRotation,          //rotation
                                 /*new Vector2(frameWidth / 2, frameHeight / 2),*/ //center
                    origin,
                    scale, SpriteEffects.None, 0);
        }

        //Make more draw methods later to take stuff like shading and stuff into account 

    }
}
