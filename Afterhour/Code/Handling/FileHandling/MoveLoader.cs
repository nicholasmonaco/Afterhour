using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Game.Scenes.Battle;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Handling.FileHandling {
    public class MoveLoader {

        public const int MOVETYPE_ATTACK = 0;
        public const int MOVETYPE_DEFEND = 1;
        public const int MOVETYPE_HEAL = 2;
        public const int MOVETYPE_STATUS = 3;

        public static List<FileStream> moveDataFileStreams;
        public static String moveDataFileDirPath;

        //

        public static void LoadContent(Resources res) {
            moveDataFileStreams = res.MoveDataStreamList;
            moveDataFileDirPath = res.MoveDataStreamDirPath;
            //SaveMove(new List<int>() { MOVETYPE_ATTACK}, "Flare", false, 1, new List<int>(){ 0 }); //i guess we get to do thin manually for all of them. yay
        }

        public static void SaveMove(List<int> moveTypeIDs, String moveName, bool moveToEnemy, int animIDToMoveToEnemy, List<int> breakpointAnimIDs) {
            if (moveDataFileDirPath != null) {
                BinaryWriter writer = new BinaryWriter(new FileStream(moveDataFileDirPath + "/" + moveName + ".dat", FileMode.Create));
                writer.Write(moveTypeIDs.Count()); 
                for(int i = 0; i < moveTypeIDs.Count(); i++) {
                    writer.Write(moveTypeIDs[i]); //id of movetypes; indexed at the top of this class
                }

                writer.Write(moveToEnemy); //bool value that tells if the player should actually go to the entity or not; if not, it will try to move projectiles to the enemy
                writer.Write(animIDToMoveToEnemy); //the animationID that is used when the player moves to the enemy, if it happens

                writer.Write(breakpointAnimIDs.Count()); //The amount of breakpoint ID's (id's of anims that are switched to from one move type to the next one
                for(int i = 0; i < breakpointAnimIDs.Count(); i++) {
                    writer.Write(breakpointAnimIDs[i]);
                }
                writer.Close();
            }else {
                System.Diagnostics.Debug.WriteLine("Cannot save move! Save path '" + moveDataFileDirPath + "/" + moveName + "' does not exist.");
            }
        }

        public static Move LoadMove(String moveName) {
            Move move = null;
            //move = new Move()
            if (moveDataFileDirPath != null) {
                BinaryReader reader = new BinaryReader(new FileStream(moveDataFileDirPath + "/" + moveName + ".dat", FileMode.Open));
                List<int> moveTypeIDs = new List<int>();
                int moveTypeCount = reader.ReadInt32();
                for(int i = 0; i < moveTypeCount; i++) {
                    moveTypeIDs.Add(reader.ReadInt32());
                }

                bool moveToEnemy = reader.ReadBoolean(); //bool value that tells if the player should actually go to the entity or not; if not, it will try to move projectiles to the enemy
                int animIDToMoveToEnemy = reader.ReadInt32(); //the animationID that is used when the player moves to the enemy, if it happens

                int breakPointAnimCount = reader.ReadInt32(); //The amount of breakpoint ID's (id's of anims that are switched to from one move type to the next one
                List<int> breakpointAnimIDs = new List<int>();
                for (int i = 0; i < breakPointAnimCount; i++) {
                    breakpointAnimIDs.Add(reader.ReadInt32());
                }
                reader.Close();

                move = new Move(moveName, moveTypeIDs, moveToEnemy, animIDToMoveToEnemy, breakpointAnimIDs);
            } else {
                System.Diagnostics.Debug.WriteLine("Cannot load move! Load path does not exist.");
            }

            return move;
        }

        public static List<Move> ConvertIDsToMoves(List<int> idList) {
            //System.Diagnostics.Debug.WriteLine("Size of move idList: " + idList.Count());

            List<Move> moves = new List<Move>();
            for(int i = 0; i < idList.Count(); i++) {
                moves.Add(LoadMove(Move.NAME_DICT[idList[i]]));
            }

            //System.Diagnostics.Debug.WriteLine("Size of returned move list: " + moves.Count());

            return moves;
        }

    }
}
