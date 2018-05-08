using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Afterhour.Code.Game.Scenes.Overworld.Entities;

namespace Afterhour.Code.Game.Scenes.Battle {
    public static class EnemyData {

        private const String filePath_Idle = "Content/BattleData/EnemyAnimData_Idle.dat";
        private const String filePath_Flee = "Content/BattleData/EnemyAnimData_Flee.dat";
        private const String filePath_Fight = "Content/BattleData/EnemyAnimData_Fight.dat";

        public static List<int> enemyIDs = new List<int>();

        public static Dictionary<int, int> frameCounts_Idle = new Dictionary<int, int>();
        public static Dictionary<int, int> frameTimes_Idle = new Dictionary<int, int>();

        public static Dictionary<int, int> frameCounts_Flee = new Dictionary<int, int>();
        public static Dictionary<int, int> frameTimes_Flee = new Dictionary<int, int>();

        public static Dictionary<int, int> attackCountPerEnemy = new Dictionary<int, int>();
        public static Dictionary<int, List<int>> frameCounts_Fight = new Dictionary<int, List<int>>();
        public static Dictionary<int, List<int>> frameTimes_Fight = new Dictionary<int, List<int>>();

        public static Dictionary<int, int> expRewardVals = new Dictionary<int, int>();
        public static Dictionary<int, int> moneyRewardVals = new Dictionary<int, int>();


        //

        public static void Create() { //This will be an unused method in the end, I'm just using it to write the binary files
            //Make the list of all the enemy ID values
            for(int i = 0; i < Enemy.MAX_ID; i++) {
                enemyIDs.Add(i);
            }
            //

            //Manually add all of the idle animation frame counts and frame times
            frameCounts_Idle.Add(Enemy.ID_SLIME, 4);
            frameTimes_Idle.Add(Enemy.ID_SLIME, 100);
            //

            //Save the idle animations to a binary file
            BinaryWriter writer_Idle = new BinaryWriter(new FileStream(filePath_Idle, FileMode.Create));
            for(int i = 0; i < frameCounts_Idle.Count(); i++) {
                writer_Idle.Write(frameCounts_Idle[i]);
                writer_Idle.Write(frameTimes_Idle[i]);
            }
            writer_Idle.Close();
            //


            //Manually add all of the fleeing animation frame counts and frame times
            frameCounts_Flee.Add(Enemy.ID_SLIME, 1);
            frameTimes_Flee.Add(Enemy.ID_SLIME, 1000);
            //

            //Save the flee animations to a binary file
            BinaryWriter writer_Flee = new BinaryWriter(new FileStream(filePath_Flee, FileMode.Create));
            for (int i = 0; i < frameCounts_Flee.Count(); i++) {
                writer_Flee.Write(frameCounts_Flee[i]);
                writer_Flee.Write(frameTimes_Flee[i]);
            }
            writer_Flee.Close();
            //


            //Manually state how many attacks each enemy has
            attackCountPerEnemy.Add(Enemy.ID_SLIME, 2); //This means the slime enemy has 2 attacks
            //

            //Manually add all of the fighting animation frame counts and frame times per the number of attacks each enemy has
            frameCounts_Fight.Add(Enemy.ID_SLIME, new List<int>() { 1, 1 });
            frameTimes_Fight.Add(Enemy.ID_SLIME, new List<int>() { 1000, 1020 });
            //

            //Save the fighting animations to a binary file
            BinaryWriter writer_Fight = new BinaryWriter(new FileStream(filePath_Fight, FileMode.Create));

            writer_Fight.Write(attackCountPerEnemy.Count());

            List<int> attackCountPerEnemyList = attackCountPerEnemy.Keys.ToList<int>();
            for(int i = 0; i < attackCountPerEnemy.Count; i++) {
                writer_Fight.Write(attackCountPerEnemyList[i]);
            }

            for (int i = 0; i < attackCountPerEnemy.Count(); i++) {
                for(int j = 0; j < attackCountPerEnemy[i]; j++) {
                    writer_Fight.Write(frameCounts_Fight[i][j]);
                    writer_Fight.Write(frameTimes_Fight[i][j]);
                }
            }

            writer_Fight.Close();
            //

        }

        public static void Construct() { //This is the method to read the data from the pre-created .dat binary file
            for (int i = 0; i <= Enemy.MAX_ID; i++) {
                enemyIDs.Add(i);
            }

            LoadRewardData();

            BinaryReader reader_Idle = new BinaryReader(new FileStream(filePath_Idle, FileMode.Open));
            for (int i = 0; i < enemyIDs.Count(); i++) {
                frameCounts_Idle.Add(i, reader_Idle.ReadInt32());
                frameTimes_Idle.Add(i, reader_Idle.ReadInt32());
            }
            reader_Idle.Close();


            BinaryReader reader_Flee = new BinaryReader(new FileStream(filePath_Flee, FileMode.Open));
            for (int i = 0; i < enemyIDs.Count(); i++) {
                frameCounts_Flee.Add(i, reader_Flee.ReadInt32());
                frameTimes_Flee.Add(i, reader_Flee.ReadInt32());
            }
            reader_Flee.Close();


            BinaryReader reader_Fight = new BinaryReader(new FileStream(filePath_Fight, FileMode.Open));
            int attackCountPerEnemyLength = reader_Fight.ReadInt32();
            for(int i=0; i < attackCountPerEnemyLength; i++) {
                attackCountPerEnemy.Add(enemyIDs[i], reader_Fight.ReadInt32());
            }

            for (int i = 0; i < attackCountPerEnemy.Count(); i++) {
                List<int> frameCounts = new List<int>();
                List<int> frameTimes = new List<int>();
                for (int j = 1; j < attackCountPerEnemy[i]; j++) { //Starts at one because the saved attack count starts at 1, not index 0 (actually i dont think it matters, whatever :/)
                    frameCounts.Add(reader_Fight.ReadInt32());
                    frameTimes.Add(reader_Fight.ReadInt32());
                }
                frameCounts_Fight.Add(i, frameCounts);
                frameTimes_Fight.Add(i, frameTimes);
            }
            reader_Fight.Close();
        }


        private static void LoadRewardData() {
            expRewardVals.Add(Enemy.ID_SLIME, 20);
            moneyRewardVals.Add(Enemy.ID_SLIME, 5);
        }

    }
}
