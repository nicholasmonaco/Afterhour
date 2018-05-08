using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling;

namespace Afterhour.Code.Menu {
    public class OptionBox {

        private Vector2 pos;
        private int rows;
        private int columns;

        private List<Texture2D> icons;
        private Point currentGridPos = new Point(0,0);
        private int curIconID = 0;
        private Rectangle gridRect;

        private bool selecting = false;

        public OptionBox(Vector2 pos, int rows, int columns) {
            this.pos = pos;
            this.rows = rows;
            this.columns = columns;
        }


        public void LoadContent(List<Texture2D> icons) {
            this.icons = icons;

            this.gridRect = new Rectangle((int)this.pos.X, (int)this.pos.Y,
                                          (this.columns * icons[0].Width) + (2 * this.columns),
                                          (this.rows * icons[0].Height) + (2 * this.rows));
        }

        public void Update(InputHandler input) {
            if (selecting) { //Check to see if the mouse is clicked on an icon in the grid or outside the grid
                if (input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) { //If the left mouse button is clicked...
                    Point mousePos = input.mouseState.Position;

                    if (!gridRect.Contains(mousePos)) {
                        selecting = false;
                    }else {
                        curIconID = TranslateIDFromCoordPoint(mousePos);
                        selecting = false;
                    }
                }
            } else { //Check to see if the selectionbox is clicked
                if (input.mouseState.LeftButton == ButtonState.Released && input.mouseState_old.LeftButton == ButtonState.Pressed) { //If the left mouse button is clicked...
                    if (new Rectangle((int)this.pos.X, (int)this.pos.Y, this.icons[0].Width, this.icons[0].Height).Contains(input.mouseState.Position)) { //If the mouse's pos is in the bounds of the icon...
                        selecting = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb) {
            if (selecting) { //Draw the grid
                for(int y = 0; y < this.rows; y++) {
                    for (int x = 0; x < this.columns; x++) {
                        Texture2D drawnIconTex = null;

                        if(TranslateIDFromGridPoint(new Point(x, y)) >= icons.Count()) {
                            drawnIconTex = icons[0];
                        }else {
                            drawnIconTex = icons[TranslateIDFromGridPoint(new Point(x, y))];
                        }

                        sb.Draw(drawnIconTex,
                                new Rectangle((int)this.pos.X + (icons[curIconID].Width * x) + (2*x),
                                              (int)this.pos.Y + (icons[curIconID].Height * y) + (2*y),
                                              icons[curIconID].Width, icons[curIconID].Height),
                                Color.White);
                    }
                }
            }else { //Draw the selected icon at the given                 
                if (curIconID >= icons.Count()) {
                    curIconID = 0;
                }
                sb.Draw(icons[curIconID], new Rectangle((int)this.pos.X, (int)this.pos.Y, icons[curIconID].Width, icons[curIconID].Height), Color.White);
            }
        }


        public int TranslateIDFromGridPoint(Point pos) {

            return ( pos.Y * this.rows) + pos.X; //idk if this works right
        }

        public int TranslateIDFromCoordPoint(Point pos) {
            Vector2 solvePos = new Vector2(pos.X - this.pos.X, pos.Y - this.pos.Y);
            solvePos.X = (float)Math.Ceiling(solvePos.X / icons[0].Width) -1;
            solvePos.Y = (float)Math.Ceiling(solvePos.Y / icons[0].Height) -1;
            double answer = (solvePos.Y * this.rows) + solvePos.X; 

            return (int)answer; 
        }

    }
}
