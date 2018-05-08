using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Game.Scenes.Overworld.Map;
using Afterhour.Code.Game.Scenes.Overworld.Entities;
using Afterhour.Code.Game.Scenes.Overworld.Entities.NPCs;
using Afterhour.Code.Game.Scenes.Overworld.Entities.Enemies;

namespace Afterhour.Code.Game.Scenes.Overworld {
    public class Zone {

        private const int ID_OVERWORLD = 0;
        private const int ID_SANCTUM_HEART = 1;
        private const int ID_SANCTUM_MIND = 2;
        private const int ID_SANCTUM_BODY = 3;
        private const int ID_SANCTUM_SOUL = 4;

        private Dictionary<int, String> zoneFilePaths = new Dictionary<int, String>() { { ID_OVERWORLD, "Map_Overworld" } };


        //

        public int zoneID;
        public TileMap map;
        public int tilesetID;
        public Texture2D tilesetTex;

        public int squaresAcross;
        public int squaresDown;

        public Player player;
        public List<NPC> NPCs = new List<NPC>();
        public List<Enemy> enemies = new List<Enemy>();

        private double runTimer = 0;

        private Dictionary<int, Texture2D> tileAnimSpiteSheets = new Dictionary<int, Texture2D>();

        ///

        public Zone(int zoneID) {
            this.zoneID = zoneID;
        }


        public void LoadContent(Resources res) {
            FullWorldData zoneData = MapLoader.LoadMap(zoneFilePaths[this.zoneID]);
            this.map = zoneData.map;

            //tilesetID = ZoneLoader.getTilesetID(mapData);
            this.tilesetTex = res.Tilesets_test1;
            Tile.TileSetTexture = this.tilesetTex;

            squaresAcross = map.mapWidth;
            squaresDown = map.mapHeight;

            //Entity Loading
            this.player = new Player(new Vector2(200, 200));
            this.player.LoadContent(res);

            //this.NPCs.Add(new Villager(new Vector2(400, 100)));

            this.enemies.Add(new Slime(new Vector2(400, 100)));
            this.enemies.Add(new Slime(new Vector2(1000, 100)));

            for(int i = 0; i < zoneData.entityList.Count(); i++) {
                System.Diagnostics.Debug.WriteLine("Loaded Entity Count: " + i);

                int id = ((Entity)zoneData.entityList[i]).id;

                if (id <= Entity.MAX_ENEMYID){ 
                    this.enemies.Add((Enemy)Entity.MakeEntity(id, ((Entity)zoneData.entityList[i]).getBounds().worldPos.ToPoint()));
                }else if(id > Entity.MAX_ENEMYID && id <= Entity.MAX_NPCID) {
                    //this.NPCs.Add((NPC)zoneData.entityList[i]));
                }
            }



            foreach (NPC npc in NPCs) {
                npc.LoadContent(res);
            }

            foreach (Enemy enemy in enemies) {
                enemy.LoadContent(res);
            }
            //


            tileAnimSpiteSheets.Add(Tile.OVERWORLD_WATER, res.TileAnims_Water);


            Camera.viewWidth = (int)Window.resolution.X;
            Camera.viewHeight = (int)Window.resolution.Y;
            Camera.worldWidth = map.mapWidth * Tile.TileWidth;
            Camera.worldHeight = map.mapWidth * Tile.TileHeight;
            Camera.displayOffset = new Vector2(0, 0);
        }

        public void Update(GameHandler gameHandler) {
            gameHandler.player = this.player;
            gameHandler.tileMap = this.map;

            map.UpdateTileAnims(gameHandler.gameTime.ElapsedGameTime.Milliseconds);


            //Entity Updating
            this.player.Update(gameHandler);

            foreach (NPC npc in NPCs) {
                npc.Update(gameHandler);
            }

            List<Enemy> newEnemies = new List<Enemy>();
            for(int e = 0; e < enemies.Count(); e++) {
                if (!enemies[e].removeFlag) {
                    newEnemies.Add(enemies[e]);
                }
            }
            enemies = newEnemies;

            Enemy lastEnemy = null;
            foreach (Enemy enemy in enemies) {
                enemy.Update(gameHandler);

                if(lastEnemy != null) {
                    if (enemy.id == Entity.ID_ENEMY_SLIME && lastEnemy.id == Entity.ID_ENEMY_SLIME) {
                        if (Vector2.Distance(new Vector2(enemy.getBounds().worldPos.X + 16, enemy.getBounds().worldPos.Y+16), new Vector2(lastEnemy.getBounds().worldPos.X + 16, lastEnemy.getBounds().worldPos.Y + 16)) < 100) {
                            ((Slime)lastEnemy).size += ((Slime)enemy).size;
                            enemy.removeFlag = true;
                            continue;
                        }
                    }
                }

                if(runTimer <= 0) {
                    if (enemy.engaged) {
                        if (gameHandler.ranFromBattle) {
                            this.runTimer = 1500; //1.5 seconds after running to get away
                            gameHandler.ranFromBattle = false;
                            enemy.engaged = false;
                        }else {
                            gameHandler.startBattle(enemy);
                        }
                    }
                }else {
                    runTimer -= gameHandler.gameTime.ElapsedGameTime.Milliseconds;
                    enemy.engaged = false;
                }

                lastEnemy = enemy;
            }
            //

            //Keypress Checking
            ToggleMinimap(gameHandler);
            //
        }

        public void Draw(SpriteBatch sb) {
            DrawTilemap(sb);

            //Entity Drawing
            this.player.Draw(sb);

            foreach (NPC npc in NPCs) {
                npc.Draw(sb);
            }

            foreach (Enemy enemy in enemies) {
                enemy.Draw(sb);
            }
            //
        }


        public void DrawTilemap(SpriteBatch sb) {
            Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.TileWidth, Camera.Location.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //System.Diagnostics.Debug.WriteLine("firstX: " + firstX + "   firstY: " + firstY);

            squaresAcross = (int)Math.Ceiling((double)(Camera.viewWidth / Tile.TileWidth)) + 1;
            squaresDown = (int)Math.Ceiling((double)(Camera.viewHeight / Tile.TileHeight)) + 2;

            //System.Diagnostics.Debug.WriteLine("Squaresdown: " + squaresDown + "   SquaresAcross: " + squaresAcross);

            for (int y = 0; y < squaresDown; y++) {
                for (int x = 0; x < squaresAcross; x++) {

                    int mapX = firstX + x;
                    int mapY = firstY + y;


                    if ((mapX >= map.mapWidth) || (mapY >= map.mapHeight)) { //Makes sure we're not out of bounds
                        continue;
                    }

                    foreach (int tileID in map.Rows[mapY].Columns[mapX].BaseTiles) {
                        if (tileID == Tile.OVERWORLD_WATER) { //maybe changethis to an isAnimated value later
                            FrameAnim tileAnim = new FrameAnim(this.tileAnimSpiteSheets[Tile.OVERWORLD_WATER], map.waterMaxAnimFrame + 1, Tile.TileWidth, Tile.TileHeight, map.waterAnimFrameMS, true, false);
                            tileAnim.curFrameID = map.curWaterAnimFrame;
                            tileAnim.Draw(sb, Camera.WorldToScreen(new Vector2(mapX * Tile.TileWidth, mapY * Tile.TileHeight)));

                        }else {
                            sb.Draw(Tile.TileSetTexture,//this calls the tileset that the tile texture comes from
                                Camera.WorldToScreen(new Vector2(mapX * Tile.TileWidth, mapY * Tile.TileHeight)),
                                Tile.GetSourceRectangle(tileID),//this calls the position from the tileset of the tile
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                1.0f,
                                SpriteEffects.None,
                                1.0f);
                        }
                    }
                }
            }
        }


        public void ToggleMinimap(GameHandler gameHandler) {
            InputHandler input = gameHandler.inputHandler;
            if(input.keyboardState.IsKeyUp(Keys.Tab) && input.keyboardState_old.IsKeyDown(Keys.Tab)) {
                gameHandler.drawMinimap = !gameHandler.drawMinimap;
            }
        }


        }
}
