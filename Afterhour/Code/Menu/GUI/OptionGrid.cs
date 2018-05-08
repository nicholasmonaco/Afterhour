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

namespace Afterhour.Code.Menu {
    public class OptionGrid {

        private Vector2 pos;
        private int rows;
        private int columns;

        private List<Texture2D> icons;
        private Texture2D iconFrame;
        private Point currentGridPos = new Point(0, 0);
        private int curIconID = 0;
        private int spacing = 3;
        private Rectangle gridRect;

        public int curSelectedID { get; set; }


        public OptionGrid(Vector2 pos, int rows, int columns) {
            this.pos = pos;
            this.rows = rows;
            this.columns = columns;
        }


        public void LoadContent(List<Texture2D> icons, Resources res) {
            this.icons = icons;

            this.iconFrame = res.Icons_Frame;

            this.gridRect = new Rectangle((int)this.pos.X, (int)this.pos.Y,
                                          (this.columns * icons[0].Width) + (spacing * (this.columns-1)),
                                          (this.rows * icons[0].Height) + (spacing * (this.rows-1)));

            this.curSelectedID = 0;
        }

        public void Update(InputHandler input) {
            //Point mousePos = input.mouseState.Position;

            if(input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) {
                if (this.gridRect.Contains(input.mouseState.Position)) {
                    currentGridPos = TranslateGridPointFromID(TranslateIDFromCoordPoint(input.mouseState.Position));
                }
            }

            curSelectedID = TranslateIDFromGridPoint(currentGridPos);

            //curIconID = TranslateIDFromCoordPoint(mousePos);
        }

        public void Draw(SpriteBatch sb) {
                for (int y = 0; y < this.rows; y++) {
                    for (int x = 0; x < this.columns; x++) {
                        Texture2D drawnIconTex = null;

                        if (TranslateIDFromGridPoint(new Point(x, y)) >= icons.Count()) {
                            drawnIconTex = icons[0];
                        } else {
                            drawnIconTex = icons[TranslateIDFromGridPoint(new Point(x, y))];
                        }

                        sb.Draw(drawnIconTex,
                                new Rectangle((int)this.pos.X + (icons[curIconID].Width * x) + (spacing * x),
                                              (int)this.pos.Y + (icons[curIconID].Height * y) + (spacing * y),
                                              icons[curIconID].Width, icons[curIconID].Height),
                                Color.White);
                    }
                }

            sb.Draw(iconFrame, new Rectangle((int)this.pos.X + (iconFrame.Width*currentGridPos.X) - (3 * (currentGridPos.X+1)), (int)this.pos.Y + (iconFrame.Height * currentGridPos.Y) - (3 * (currentGridPos.Y+1)), iconFrame.Width, iconFrame.Height), Color.White);
        }


        public int TranslateIDFromGridPoint(Point pos) {

            return (pos.Y * this.rows) + pos.X; //idk if this works right
        }

        public Point TranslateGridPointFromID(int id) {

            return new Point(id % this.columns, id / this.columns);
        }



        public int TranslateIDFromCoordPoint(Point pos) {
            Vector2 solvePos = new Vector2(pos.X - this.pos.X, pos.Y - this.pos.Y);
            solvePos.X = (float)Math.Ceiling(solvePos.X / icons[0].Width) - 1;
            solvePos.Y = (float)Math.Ceiling(solvePos.Y / icons[0].Height) - 1;
            double answer = (solvePos.Y * this.rows) + solvePos.X;

            return (int)answer;
        }



    }
}
