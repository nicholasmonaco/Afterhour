using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.FileHandling;
using Afterhour.Code.Game.Scenes.Battle.Moves;

namespace Afterhour.Code.Game.Scenes.Battle {
    public class Move {

        //Move ID Dictionary
        public static Dictionary<String, int> ID_DICT = new Dictionary<String, int>();
        public static Dictionary<int, String> NAME_DICT = new Dictionary<int, String>();

        public static void LoadMoveDictionary() {
            ID_DICT.Add("Flare", 0);
            NAME_DICT.Add(0, "Flare");
        }

        //

        //Primary Anim IDs
        public static int PRIMARY_SHOOT = 0;
        //


        public Texture2D iconTex;
        public List<FrameAnim> primaryAnims; //This is the animations that actually contain the player sprites
        public List<FrameAnim> secondaryAnims; //This is projectiles, auras, etc.
        private List<int> breakpointAnimIDs;

        public String moveName;
        public List<int> moveTypes;

        public Vector2 targetPos;

        private bool moveToEnemy;
        private int animIDToMoveToEnemy;
        private double speedOfMoveToEnemy;
        private double projSpeed;

        public int curAnimID;

        public Vector2 originalPlayerPos;
        public Vector2 curMoveToPos;

        private int curMoveType;
        private int curMoveTypeID;
        private int maxMoveTypeID;

        public Vector2 extrapolatedProjPos;

        public int frameCount = 0;
        public bool finished = false;

        //

        public Move(String moveName, List<int> moveTypes, bool moveToEnemy, int animIDToMoveToEnemy, List<int> breakpointAnimIDs) {
            this.moveName = moveName;
            this.moveTypes = moveTypes;
            this.curMoveTypeID = 0;
            this.maxMoveTypeID = moveTypes.Count() - 1;

            this.moveToEnemy = moveToEnemy;
            this.animIDToMoveToEnemy = animIDToMoveToEnemy;

            this.breakpointAnimIDs = breakpointAnimIDs;
        }

        public void LoadContent(List<FrameAnim> primaryAnims, List<FrameAnim> secondaryAnims) {
            this.primaryAnims = primaryAnims;
            this.secondaryAnims = secondaryAnims;

            this.curAnimID = 0;
        }

        public void PrimeMove(Vector2 playerPos, Vector2 targetPos, double speedOfMoveToEnemy, double projSpeed) {
            this.originalPlayerPos = playerPos;
            this.extrapolatedProjPos = playerPos;

            this.targetPos = targetPos;
            this.speedOfMoveToEnemy = speedOfMoveToEnemy;
            this.projSpeed = projSpeed;
        }

        public void UpdateMove(double elapsedMS) {
            //This will work simialr to the drawmove method, just with a curMoveType and a curMovetypeCompleted bool value
            switch (this.curMoveType) {
                case MoveLoader.MOVETYPE_ATTACK:
                    this.primaryAnims[curAnimID].Update(elapsedMS);
                    if (!moveToEnemy) {
                        ExtrapolateProjPos(elapsedMS);
                        this.secondaryAnims[0].Update(elapsedMS);//this may need to change from 0 to a variablistic value later
                    }
                    if (TestForFinishCondition()) {
                        TestForFinish();
                        if (this.breakpointAnimIDs.Contains(curAnimID)) {
                            if (this.curMoveTypeID < this.maxMoveTypeID) {
                                this.curMoveType = moveTypes[curMoveTypeID + 1];
                            }
                        } 
                    }
                    break;
                case MoveLoader.MOVETYPE_STATUS:
                    this.primaryAnims[curAnimID].Update(elapsedMS);
                    if (!moveToEnemy) {
                        ExtrapolateProjPos(elapsedMS);
                        this.secondaryAnims[0].Update(elapsedMS);
                    }
                    if (TestForFinishCondition()) {
                        TestForFinish();
                        if (this.breakpointAnimIDs.Contains(curAnimID)) {
                            if (this.curMoveTypeID < this.maxMoveTypeID) {
                                this.curMoveType = moveTypes[curMoveTypeID + 1];
                            }
                        }
                    }
                    break;
                case MoveLoader.MOVETYPE_HEAL:
                    this.primaryAnims[curAnimID].Update(elapsedMS);
                    if (!moveToEnemy) {
                        ExtrapolateProjPos(elapsedMS);
                        this.secondaryAnims[0].Update(elapsedMS);
                    }
                    if (TestForFinishCondition()) {
                        TestForFinish();
                        if (this.breakpointAnimIDs.Contains(curAnimID)) {
                            if (this.curMoveTypeID < this.maxMoveTypeID) {
                                this.curMoveType = moveTypes[curMoveTypeID + 1];
                            }
                        }
                    }
                    break;
                case MoveLoader.MOVETYPE_DEFEND:
                    this.primaryAnims[curAnimID].Update(elapsedMS);
                    if (!moveToEnemy) {
                        ExtrapolateProjPos(elapsedMS);
                        this.secondaryAnims[0].Update(elapsedMS);
                    }
                    if (TestForFinishCondition()) {
                        TestForFinish();
                    }
                    break;
            }
        }

        public void DrawMove(SpriteBatch sb) {
            if (moveTypes.Contains(MoveLoader.MOVETYPE_ATTACK)) {
                Attack.DrawMove(this, sb);
                this.curMoveType = MoveLoader.MOVETYPE_ATTACK;
            }

            if (moveTypes.Contains(MoveLoader.MOVETYPE_STATUS)) {
                //Status.DrawMove(this, sb);
                this.curMoveType = MoveLoader.MOVETYPE_STATUS;
            }

            if (moveTypes.Contains(MoveLoader.MOVETYPE_HEAL)) {
                //Heal.DrawMove(this, sb);
                this.curMoveType = MoveLoader.MOVETYPE_HEAL;
            }

            if (moveTypes.Contains(MoveLoader.MOVETYPE_DEFEND)) {
                //Defend.DrawMove(this, sb);
                this.curMoveType = MoveLoader.MOVETYPE_DEFEND;
            }

        }

        private void TestForFinish() {
            curAnimID++;
            if (curAnimID > primaryAnims.Count() - 1) {
                finished = true;
                curAnimID = 0;
                this.curMoveType = this.moveTypes[0];

                this.extrapolatedProjPos = originalPlayerPos;
                for(int i = 0; i < primaryAnims.Count(); i++) {
                    this.primaryAnims[i].loops = 0;
                }
            } 
        }

        private void ExtrapolateProjPos(double elapsedMS) {
            //System.Diagnostics.Debug.WriteLine("BEFORE: Current extrapolated position: " + extrapolatedProjPos + "  Target pos: " + targetPos);

            if (extrapolatedProjPos.X != targetPos.X) {
                //extrapolatedProjPos.X = Math.Min((int)(extrapolatedProjPos.X + (projSpeed * elapsedMS)), (int)targetPos.X); //this is the old way, which doesnt work, but i saved it anyway
                extrapolatedProjPos.X += (float)Math.Min((double)(projSpeed * elapsedMS), (double)(targetPos.X - extrapolatedProjPos.X));
            }

            if (extrapolatedProjPos.Y != targetPos.Y) {
                //extrapolatedProjPos.Y = Math.Min((int)(extrapolatedProjPos.Y + (projSpeed * elapsedMS)), (int)targetPos.Y);
                if(Math.Abs((double)((projSpeed / 8) * elapsedMS)) > Math.Abs((double)(targetPos.Y - extrapolatedProjPos.Y))) {
                    extrapolatedProjPos.Y += (float)(targetPos.Y - extrapolatedProjPos.Y);
                } else {
                    extrapolatedProjPos.Y += (float)((projSpeed / 8) * elapsedMS * Math.Sign(targetPos.Y-extrapolatedProjPos.Y));
                }
            }

            //System.Diagnostics.Debug.WriteLine("AFTER: Current extrapolated position: " + extrapolatedProjPos + "  Target pos: " + targetPos);
        }

        private bool TestForFinishCondition() {
            bool retVal = false;
            if (this.moveToEnemy) {
                if(this.primaryAnims[curAnimID].loops > 1) {
                    retVal = true;
                }
            }else {
                if(Math.Abs(Vector2.Distance(this.extrapolatedProjPos, targetPos)) < 3) {
                    retVal = true;
                }
            }
            return retVal;
        }

    }
}
