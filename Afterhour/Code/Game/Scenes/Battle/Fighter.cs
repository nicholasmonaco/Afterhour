using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Game.Scenes.Battle {
    public class Fighter {

        public const int STATE_IDLE = 0;
        public const int STATE_FLEE = 1;
        public const int STATE_FIGHT = 2;

        protected Texture2D healthBarTex_empty;
        protected Texture2D healthBarTex_full;


        public virtual void LoadContent(Resources res) {
            this.healthBarTex_empty = res.Battle_UI_HealthBarEmpty;
            this.healthBarTex_full = res.Battle_UI_HealthBarFull;
        }

        public virtual void Update(double fightTimeMS, int curState) {

        }

        public virtual void Draw(SpriteBatch sb) {

        }

    }
}
