using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afterhour.Code.Game.Scenes.Overworld.Map {
    public class MapCell {

        public int TileID {
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; } //If the array is greater than 0, it will return the first basetile; if not, it will return 0
            set {
                if (BaseTiles.Count > 0)
                    BaseTiles[0] = value;
                else
                    AddBaseTile(value);
            }
        }


        public List<int> BaseTiles = new List<int>();

        public bool walkable { get; set; }
        public int teleportID { get; set; } //ok so if its 0, nothing happens. 1 is overworld, then 2-5 are for each of the sanctums. maybe more later idk



        //This is the real class

        public MapCell(int tileID) {
            TileID = tileID;
            walkable = true;
            teleportID = 0;

            switch (tileID) { //This is where exceptions for certain tiletypes can be made. There might be a better way to do this eventually, but idk. This is a rough area
                case Tile.OVERWORLD_WATER:
                    walkable = false;
                    break;
                case 128:
                    walkable = false;
                    break;
                case Tile.OVERWORLD_DOOR_HEART:
                    teleportID = 2;
                    break;
            }

        }


        public void AddBaseTile(int tileID) {
            BaseTiles.Add(tileID);
        }

    }
}
