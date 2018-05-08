using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Game.Scenes.Battle.Fighters {
    public class PlayerFighter : Fighter {

        public Vector2 standbyScreenPos;
        public Vector2 curScreenPos;

        public int curState;

        public bool hasAttacked = true;
        public bool isFighting = false;
        public int curAttackID = 0;

        private FrameAnim idleAnim;
        private List<FrameAnim> fightAnims = new List<FrameAnim>();
        private FrameAnim fleeAnim;

        public Vector2 baseDims = new Vector2(32, 32); //Replace this preset later with frameanim sheet values

        public List<Move> attackList; //This is the list of things that shows up in the fight wheel

        //

        public PlayerFighter(Vector2 standbyScreenPos) {
            this.standbyScreenPos = standbyScreenPos;
            this.curScreenPos = standbyScreenPos;
        }


        public override void LoadContent(Resources res) {
            
        }

        public override void Update(double fightTimeMS, int curState) {
            
        }

        public override void Draw(SpriteBatch sb) {
            
        }

    }
}
