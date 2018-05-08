using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Game.Scenes.Overworld.Map {
    public class TileMap {

        public List<MapRow> Rows = new List<MapRow>();
        public int mapWidth { get; set; } = 30;
        public int mapHeight { get; set; } = 30;

        public Texture2D mouseMap { get; set; }



        public double waterAnimFrameMS = 500;
        private double curWaterAnimFrameMS = 0;

        public int curWaterAnimFrame = 0;
        public int waterMaxAnimFrame = 3;
        //add more later


        public TileMap(int mapWidth, int mapHeight) {

            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;

            for (int y = 0; y < mapWidth; y++) {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < mapWidth; x++) {
                    thisRow.Columns.Add(new MapCell(0));
                }
                Rows.Add(thisRow);
            }

            /*for (int y = 0; y < mapWidth; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    //set all tiles to a value here
                }
            }*/
        }

        public void UpdateTileAnims(double elapsedMS) {//i wonder if theres some way to streamline this with for loops
            curWaterAnimFrameMS += elapsedMS;
            if(curWaterAnimFrameMS >= waterAnimFrameMS) {
                curWaterAnimFrame++;
                curWaterAnimFrameMS = 0;
                if(curWaterAnimFrame > waterMaxAnimFrame) {
                    curWaterAnimFrame = 0;
                }
            }
        }


        public TileMap(int width, int height, List<MapRow> rows) { //This is the one we will use almsot all of the time
            this.mapWidth = width;
            this.mapHeight = height;

            this.Rows = rows;
        }


        public Point WorldToMapCell(Point worldPoint, out Point localPoint) { //This might not work, I'll find out eventually
            Point mapCell = new Point((int)(worldPoint.X / Tile.TileWidth),
                                      ((int)(worldPoint.Y / Tile.TileHeight)));

            int localPointX = worldPoint.X % Tile.TileWidth;
            int localPointY = worldPoint.Y % Tile.TileHeight;


            localPoint = new Point(localPointX, localPointY);

            return mapCell;
        }

        public Point WorldToMapCell(Point worldPoint) {
            Point dummy;
            return WorldToMapCell(worldPoint, out dummy);
        }

        public Point WorldToMapCell(Vector2 worldPoint) {
            return WorldToMapCell(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public MapCell GetCellAtWorldPoint(Point worldPoint) {
            Point mapPoint = WorldToMapCell(worldPoint);
            MapCell ret = new MapCell(128);
            ret.walkable = false;
            try {
                ret = Rows[mapPoint.Y].Columns[mapPoint.X];
            } catch (Exception) {
                //System.Diagnostics.Debug.WriteLine("Exeption with getting cell at worldPoint: " + worldPoint);
            }

            return ret;
        }

        public MapCell GetCellAtWorldPoint(Vector2 worldPoint) {
            return GetCellAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }


    }

    public class MapRow {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
