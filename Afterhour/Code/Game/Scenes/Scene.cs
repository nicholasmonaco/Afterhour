using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Game.Scenes
{
    public abstract class Scene
    {
        public const int ID_OVERWORLD = 0;
        public const int ID_BATTLE = 1;


        public abstract void LoadContent(Resources res, GameHandler gameHandler);

        public abstract void Update(GameHandler gameHandler);

        public abstract void Draw(SpriteBatch sb);


    }
}
