using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;
using Afterhour.Code.Game.Scenes.Overworld.Map;
using Afterhour.Code.Game.Scenes.Overworld.Entities.Enemies;
using Afterhour.Code.Game.Scenes.Overworld.Entities.NPCs;

namespace Afterhour.Code.Game.Scenes.Overworld.Entities {
    public class Entity {

        public const int ID_ENEMY_SLIME = 0;

        public const int MAX_ENEMYID = 0; //Enemy ids should go from 0-199


        public const int ID_NPC_VILLAGER = 200;

        public const int MAX_NPCID = 200;



        public int id;
        public Bounds Bounds;
        public double size = 1;


        public virtual void LoadContent(Resources res) {

        }

        public virtual void Update(GameHandler gameHandler) {

        }

        public virtual void Draw(SpriteBatch sb){

        }
        

        public static Entity MakeEntity(int id, Point pos) {
            /*Entity e = entityTemplates[id];
            e.setWorldPos(pos.ToVector2());
            e.id = id;
            return e;*/

            Entity e = null;
            switch (id) {
                case ID_ENEMY_SLIME:
                    e = new Slime(pos.ToVector2());
                    break;
            }

            return e;
        }


        public virtual bool IsColliding(TileMap map, Rectangle bounds, Vector2 addPos) { //make a version of this that returns another Vector2 that is the distance between current position and the max possible moved direction
            bool isColliding = false;
            List<Vector2> testPositions = new List<Vector2>();
            testPositions.Add(new Vector2(bounds.X + addPos.X, bounds.Y + addPos.Y));
            testPositions.Add(new Vector2(bounds.X + addPos.X + bounds.Width, bounds.Y + addPos.Y));
            testPositions.Add(new Vector2(bounds.X + addPos.X, bounds.Y + addPos.Y + bounds.Height));
            testPositions.Add(new Vector2(bounds.X + addPos.X + bounds.Width, bounds.Y + addPos.Y + bounds.Height));

            foreach(Vector2 testPos in testPositions) {
                MapCell testCell = map.GetCellAtWorldPoint(testPos);
                if(testPos.X < 0 || testPos.Y < 0) {
                    isColliding = true;
                }
                if (!testCell.walkable) { //More conditionals can be added to this later
                     isColliding = true;
                }
            }

            return isColliding;
        }

        public virtual Vector2 SolveMaxMoveableDistance(TileMap map, Rectangle bounds, Vector2 addPos) {
            Vector2 maxMoveableDist = Vector2.Zero;
            Vector2 origAddPos = addPos;

            if(addPos.X > 0) {
                addPos.X = addPos.X + bounds.Width;
            }

            if (addPos.Y > 0) {
                addPos.Y = addPos.Y + bounds.Height;
            }

            MapCell testCell = map.GetCellAtWorldPoint(new Vector2(bounds.X + addPos.X, bounds.Y + addPos.Y));

            if (!testCell.walkable) {
                Point mapCellPos = map.WorldToMapCell(new Vector2(bounds.X + addPos.X, bounds.Y + addPos.Y));
                maxMoveableDist = new Vector2((mapCellPos.X*Tile.TileWidth) - (bounds.X + addPos.X), (mapCellPos.Y* Tile.TileHeight) - (bounds.Y + addPos.Y));

                //System.Diagnostics.Debug.WriteLine(maxMoveableDist);

            } else {
                maxMoveableDist = origAddPos;
            }


            return maxMoveableDist;
        }

        public virtual void setPos(Vector2 newPos) {
            this.Bounds.worldPos.X = (int)newPos.X;
            this.Bounds.worldPos.Y = (int)newPos.Y;
        }

        public virtual Bounds getBounds() {
            return Bounds;
        }

    }
}
