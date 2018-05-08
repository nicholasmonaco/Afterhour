using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.States;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Handling {
    public class StateHandler {

        private Resources res;

        public State curState { get; set; }
        public int curStateID;


        public StateHandler(Resources res) {
            this.res = res;
        }

        public StateHandler(Resources res, int curStateID) {
            this.res = res;
            setCurState(curStateID);
        }


        public void setCurState(int newStateID) {
            switch (newStateID) {
                case State.MENU:
                    this.curState = new MenuState();
                    break;
                case State.GAME:
                    this.curState = new GameState();
                    break;
                    //Add for options and game later
            }

            this.curStateID = newStateID;
            this.curState.LoadContent(res);
        }

    }
}
