using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Afterhour.Code.Game.Scenes.Battle.Moves {
    public class Attack {




        //Attack Move Code

        public static void DrawMove(Move move, SpriteBatch sb) {
            //System.Diagnostics.Debug.WriteLine("Attempting to draw move with name: " + move.moveName);

            String moveName = move.moveName;
            switch (moveName) {
                case "Mana_Sphere":
                    //put the code here
                    break;


                case "Mana_Bolt":
                    //sb.Draw(move.)
                    break;


                case "Flare":
                    move.primaryAnims[move.curAnimID].Draw(sb, move.originalPlayerPos);
                    move.secondaryAnims[0].Draw(sb, move.extrapolatedProjPos);
                    break;

                case "Mana_B":
                    break;
                case "Mana_C":
                    break;
                case "Mana_D":
                    break;
                case "Mana_E":
                    break;
                case "Mana_F":
                    break;
                case "Mana_G":
                    break;
                case "Mana_H":
                    break;
                case "Mana_I":
                    break;
                case "Mana_J":
                    break;
                case "Mana_K":
                    break;
                case "Mana_L":
                    break;
            }
        }

    }
}
