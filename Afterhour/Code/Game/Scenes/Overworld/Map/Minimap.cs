using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling;
using Afterhour.Code.Game.Scenes.Overworld.Map;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.Game.Scenes.Overworld.Entities;

namespace Afterhour.Code.Game.Scenes.Overworld.Map {
    public class Minimap {

        private Texture2D frameTex;
        private Texture2D frameOutlineTex;
        private Texture2D fillTex;
        private Texture2D outlineTex;

        private Texture2D icon_PlayerTex;
        private Texture2D icon_EnemyTex;




        private TileMap map;
        private Vector2 scaledPlayerPos;

        private Vector2 pos;
        private Rectangle bounds;

        private Vector2 scaledMovePos;

        private float scale = 1f;
        private Vector2 mapOffset = new Vector2(25, 25);


        private List<Enemy> enemies;
        private List<Vector2> enemyMinimapPositions = new List<Vector2>();


        private TextureMasker minimapMask;
        private Texture2D minimapBorder;



        public Minimap(float scale, TileMap map) {
            this.map = map;
            this.scale = scale;
        }


        public void LoadContent(Resources res) {
            this.frameTex = res.Minimap_Border;
            this.frameOutlineTex = res.Minimap_Border;
            this.fillTex = res.Minimap_Fill;
            this.outlineTex = res.Minimap_Outline;

            this.pos = new Vector2((int)(Window.resolution.X - frameTex.Width - 15), 15);
            this.bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)(fillTex.Width * map.mapWidth * scale), (int)(fillTex.Height * map.mapHeight * scale));

            minimapMask = new TextureMasker(res.Minimap_MapTex, res.Minimap_MaskTex);
            this.minimapBorder = res.Minimap_MaskBorder;
        }

        public void LoadContent(Resources res, TileMap map) {
            this.frameTex = res.Minimap_Border;
            this.frameOutlineTex = res.Minimap_Border;
            this.fillTex = res.Minimap_Fill;
            this.outlineTex = res.Minimap_Outline;
            this.icon_PlayerTex = res.Minimap_Icon_Player;
            this.icon_EnemyTex = res.Minimap_Icon_Enemy;
            

            this.map = map;
            this.pos = new Vector2((int)(Window.resolution.X - frameTex.Width - 15), 15);
            this.bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)(fillTex.Width * map.mapWidth * scale), (int)(fillTex.Height * map.mapHeight * scale));
            
            minimapMask = new TextureMasker(res.Minimap_MapTex, res.Minimap_MaskTex);
            this.minimapBorder = res.Minimap_MaskBorder;
        }

        public void Update(Vector2 playerPos, List<Enemy> enemies) {

            /*int scaledPlayerX = (int)(((map.mapWidth * fillTex.Width * scale - icon_PlayerTex.Width +2) / (map.mapWidth * Tile.TileWidth)) * playerPos.X);
            int scaledPlayerY = (int)(((map.mapHeight * fillTex.Height * scale - icon_PlayerTex.Height +2) / (map.mapHeight * Tile.TileHeight)) * playerPos.Y);

            MinimapCam.Move(new Vector2(scaledPlayerX, scaledPlayerY));

            this.scaledPlayerPos = new Vector2(mapOffset.X + this.pos.X + scaledPlayerX, mapOffset.Y + this.pos.Y + scaledPlayerY);

            mapOffset.X = -scaledPlayerX + frameTex.Width / 2;
            mapOffset.Y = -scaledPlayerY + frameTex.Height / 2;*/


            //New - After Texture Masking

            this.enemies = enemies;

            int XIncrementVal = (int)(minimapMask.backTex.Width / (map.mapWidth / Tile.TileWidth));
            int YIncrementVal = (int)(minimapMask.backTex.Height / (map.mapHeight / Tile.TileHeight));

            //get the player pos, divide it by tile size,
            int xPos = (int)((playerPos.X /*- Camera.viewWidth*/) / Tile.TileWidth);
            int yPos = (int)((playerPos.Y /*- Camera.viewHeight*/) / Tile.TileHeight);

            int playerX = (int)(playerPos.X / XIncrementVal);
            int playerY = (int)(playerPos.Y / YIncrementVal);

            this.scaledPlayerPos = new Vector2(this.pos.X + minimapMask.maskTex.Width/2, this.pos.Y + +minimapMask.maskTex.Height / 2);

            this.enemyMinimapPositions.Clear();
            for(int i = 0; i < enemies.Count(); i++) {
                this.enemyMinimapPositions.Add(new Vector2(this.scaledPlayerPos.X + (enemies[i].superPos.X-playerPos.X)/Tile.TileWidth, this.scaledPlayerPos.Y + (enemies[i].superPos.Y - playerPos.Y) / Tile.TileHeight)); //make this right, then change the thing below to correspond to how 
            }

            MinimapCam.Move(new Vector2(playerPos.X/Tile.TileWidth - 1 - minimapMask.maskTex.Width / 2, playerPos.Y/Tile.TileHeight - 1 - minimapMask.maskTex.Height / 2));
        }

        public void DrawOLD(SpriteBatch sb) {
            sb.Draw(this.frameTex, new Rectangle((int)pos.X, (int)pos.Y, frameTex.Width, frameTex.Height), Color.White);

            for(int y = 0; y < this.map.mapHeight; y++) {
                for (int x = 0; x < this.map.mapWidth; x++) {
                    //sb.Draw(fillTex, new Rectangle(25 + (x*fillTex.Width), 25 + (y * fillTex.Height), fillTex.Width, fillTex.Height), Color.Green);
                    Vector2 testPos = new Vector2(mapOffset.X + (x * fillTex.Width * scale) + pos.X, mapOffset.Y + (y * fillTex.Height * scale) + pos.Y);
                    if (testPos.X > (/*MinimapCam.location.X+*/this.pos.X) && testPos.X < (MinimapCam.bounds.Width + this.pos.X)) {
                        if (testPos.Y > (/*MinimapCam.location.Y + */this.pos.Y) && testPos.Y < (MinimapCam.bounds.Height + this.pos.Y)) {
                            sb.Draw(fillTex, testPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                    }
                    //sb.Draw(fillTex, testPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }

            float scaleX = (float)((float)(map.mapWidth * fillTex.Width * scale) / outlineTex.Width);
            float scaleY = (float)((float)(map.mapHeight * fillTex.Height * scale) / outlineTex.Width);

            /* Here's one way to try the "in the box" thing, but it doesnt work quite right
             * 
             Vector2 tp = Vector2.Zero;

            tp = new Vector2(mapOffset.X + pos.X, mapOffset.Y + pos.Y - 1);
            if(tp.X > this.pos.X && tp.Y > this.pos.Y) {
                sb.Draw(outlineTex, tp, null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f); //Draws the top border bar
            }

            tp = new Vector2(mapOffset.X + pos.X, mapOffset.Y + pos.Y + (map.mapHeight * fillTex.Height * scale));
            if (tp.X < (this.pos.X+this.frameTex.Width) && tp.Y < (this.pos.Y +this.frameTex.Height)) {
                sb.Draw(outlineTex, tp, null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f); //Draws the bottom border bar
            }

            tp = new Vector2(mapOffset.X + pos.X, mapOffset.Y + pos.Y);
            if (tp.X > this.pos.X && tp.Y > this.pos.Y) {
                sb.Draw(outlineTex, tp, null, Color.White, (float)Math.PI / 2, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f); //Draws the left border bar
            }

            tp = new Vector2(mapOffset.X + pos.X + (map.mapHeight * fillTex.Height * scale) + 1, mapOffset.Y + pos.Y);
            if (tp.X < (this.pos.X + this.frameTex.Width) && tp.Y < (this.pos.Y + this.frameTex.Height)) {
                sb.Draw(outlineTex, tp, null, Color.White, (float)Math.PI / 2, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f); //Draws the right border bar
            }
            */


            //Draw the borders
            /*sb.Draw(outlineTex, new Vector2(mapOffset.X + pos.X, mapOffset.Y + pos.Y - 1), null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f);
            sb.Draw(outlineTex, new Vector2(mapOffset.X + pos.X, mapOffset.Y + pos.Y + (map.mapHeight * fillTex.Height*scale) ), null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f);

            sb.Draw(outlineTex, new Vector2(mapOffset.X + pos.X , mapOffset.Y + pos.Y), null, Color.White, (float)Math.PI/2, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f); 
            sb.Draw(outlineTex, new Vector2(mapOffset.X + pos.X + (map.mapHeight * fillTex.Height * scale) + 1, mapOffset.Y + pos.Y), null, Color.White, (float)Math.PI/2, Vector2.Zero, new Vector2(scaleX, 1f), SpriteEffects.None, 1f);
            */

            //sb.Draw(icon_PlayerTex, scaledPlayerPos, Color.White);
            sb.Draw(icon_PlayerTex, new Vector2(this.pos.X +  this.frameTex.Width/2, this.pos.Y + this.frameTex.Height / 2), Color.White);


            sb.Draw(this.frameOutlineTex, new Rectangle((int)pos.X, (int)pos.Y, frameOutlineTex.Width, frameOutlineTex.Height), Color.White);

        }


        public void Draw(SpriteBatch sb, GFXHandler gh) {
            scaledMovePos = new Vector2(this.pos.X - MinimapCam.bounds.X, this.pos.Y - MinimapCam.bounds.Y);
            minimapMask.Draw(gh.graphicsDeviceManager, sb, this.pos, this.scaledMovePos);
            sb.Draw(this.minimapBorder, this.pos, Color.White);

            for(int i = 0; i < this.enemies.Count(); i++) {
                sb.Draw(this.icon_EnemyTex, this.enemyMinimapPositions[i], Color.White);
            }

            sb.Draw(icon_PlayerTex, scaledPlayerPos, Color.White);

            //sb.Draw(this.frameOutlineTex, new Rectangle((int)pos.X, (int)pos.Y, frameOutlineTex.Width, frameOutlineTex.Height), Color.White); //Draws the minimap outline
        }

    }

    static class MinimapCam {
        public static Vector2 location = Vector2.Zero;
        public static Rectangle bounds = new Rectangle(0, 0, 148, 148); //Make this work right

        public static void Move(Vector2 moveVar) {
            location = moveVar;
            bounds.X = (int)location.X;
            bounds.Y = (int)location.Y;
        }
    }
}
