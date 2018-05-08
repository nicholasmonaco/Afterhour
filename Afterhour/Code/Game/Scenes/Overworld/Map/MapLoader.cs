using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Game.Scenes.Overworld.Entities;

namespace Afterhour.Code.Game.Scenes.Overworld.Map {
    public class MapLoader {

        //

        public static int getWidth(List<String> mapData) {
            return Int32.Parse(mapData[0]); //These 0 and 1 values of the array are probably wrong, fix it later when we build the importer
        }

        public static int getHeight(List<String> mapData) {
            return Int32.Parse(mapData[1]);
        }




        public static List<String> LoadZone(Resources res, int zoneID) {
            List<String> mapData = new List<String>();

            String width = "1000";
            String height = "1000";

            mapData.Add(width); //Map Width
            mapData.Add(height); //Map Height

            Random rand = new Random(); //this is placeholder

            for(int y = 0; y < Int32.Parse(height); y++) {
                for (int x = 0; x < Int32.Parse(width); x++) {
                    mapData.Add(rand.Next(4).ToString()); //change this to use the stuff from the imported file's data
                }
            }




            return mapData;
        }


        /*public static List<MapRow> TileMapFromData(List<String> mapData) {
            List<MapRow> rows = new List<MapRow>();
            
            for(int y = 0;y < Int32.Parse(mapData[1]); y++) {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < Int32.Parse(mapData[0]); x++) {
                    thisRow.Columns.Add(new MapCell(Int32.Parse(mapData[2 + (y * Int32.Parse(mapData[0]) + x)])));
                    //thisRow.Columns.Add(new MapCell(0));
                }
                rows.Add(thisRow);
            }


            return rows;
        }*/


        public static FullWorldData LoadMap(String mapName) {
            BinaryReader reader = new BinaryReader(new FileStream(Environment.CurrentDirectory + "/Content/Maps/" + mapName + ".ahm", FileMode.Open));

            int mapWidth = reader.ReadInt32();
            int mapHeight = reader.ReadInt32();

            TileMap map = new TileMap(mapWidth, mapHeight);

            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    map.Rows[y].Columns[x] = new MapCell(reader.ReadInt32());
                }
            }

            List<Object> entityList = new List<Object>();
            int entityCount = reader.ReadInt32();

            for (int e = 0; e < entityCount; e++) {
                int id = reader.ReadInt32();
                int xPos = reader.ReadInt32();
                int yPos = reader.ReadInt32();
                entityList.Add(Entity.MakeEntity(id, new Point(xPos, yPos)));
            }

            FullWorldData worldData = new FullWorldData(map, entityList);
            return worldData;
        }

    }

    public class FullWorldData {
        public TileMap map;
        public List<Object> entityList;

        public FullWorldData(TileMap map, List<Object> entityList) {
            this.map = map;
            this.entityList = entityList;
        }
    }
}
