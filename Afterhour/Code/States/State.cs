using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.GFXHandling;

namespace Afterhour.Code.States {
    public abstract class State {

        public const int MENU = 0;
        public const int GAME = 1;
        public const int OPTIONS = 2;

        //------

        public abstract void LoadContent(Resources res);

        protected void UnloadContent(ContentManager content) { }

        protected void Init() { }

        public abstract void Update(StateHandler sh, GameTime gameTime);

        public abstract void Draw(SpriteBatch sb, GFXHandler gh, GameTime gameTime);
    }
}
