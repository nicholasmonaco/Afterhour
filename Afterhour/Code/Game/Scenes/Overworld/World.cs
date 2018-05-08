using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.Game.Scenes.Overworld.Map;

namespace Afterhour.Code.Game.Scenes.Overworld {
    public class World {

        public List<Zone> zones = new List<Zone>();
        public int curZoneID = 0;

        public Minimap minimap;
        private bool drawMinimap = false;

        public World() {
            zones.Add(new Zone(0));


            minimap = new Minimap(0.1f, zones[curZoneID].map);
        }


        public void LoadContent(Resources res) {
            foreach (Zone zone in zones) {
                zone.LoadContent(res);
            }

            minimap.LoadContent(res, zones[curZoneID].map);
            
        }

        public void Update(GameHandler gameHandler) {
            if (zones[curZoneID] != null) {
                zones[curZoneID].Update(gameHandler);
            }

            drawMinimap = gameHandler.drawMinimap;
            if (drawMinimap) {
                minimap.Update(zones[curZoneID].player.worldPos, zones[curZoneID].enemies);
            }
        }

        public void Draw(SpriteBatch sb, GFXHandler gh) {
            if(zones[curZoneID] != null) {
                zones[curZoneID].Draw(sb);
            }

            if (drawMinimap) {
                minimap.Draw(sb, gh);
            }
        }

    }
}
