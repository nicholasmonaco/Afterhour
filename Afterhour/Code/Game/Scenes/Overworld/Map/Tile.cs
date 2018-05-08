using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Afterhour.Code.Game.Scenes.Overworld.Map {
    static class Tile {

        static public Texture2D TileSetTexture;

        static public int TileWidth = 32;
        static public int TileHeight = 32;


        public const int OVERWORLD_GRASS = 0;
        public const int OVERWORLD_DIRT = 1;
        public const int OVERWORLD_STONE = 2;
        public const int OVERWORLD_WATER = 3;

        public const int OVERWORLD_DOOR_HEART = 402;
        public const int OVERWORLD_DOOR_MIND = 402;
        public const int OVERWORLD_DOOR_SOUL = 402;
        public const int OVERWORLD_DOOR_BODY = 402;



        public const int SANCTUM_HEART_DOOR = 0; //Sanctum of the Heart, Mind, Soul, Body


        public static Rectangle GetSourceRectangle(int tileIndex) {
            int tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            int tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }

    }
}
