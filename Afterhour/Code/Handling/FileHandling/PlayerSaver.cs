using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;


namespace Afterhour.Code.Handling.FileHandling {
    public class PlayerSaver {

        public const int STAT_ATK = 0;
        public const int STAT_DEF = 1;
        public const int STAT_VIT = 2;
        public const int STAT_DEX = 3;
        public const int STAT_INT = 4;
        public const int STAT_WIS = 5;
        public const int STAT_VIR = 6;
        public const int STAT_EVL = 7;

        //Format = format inputted data for saving
        //Create = make totally new from data


        public static void SaveNewGame(CharData charData, List<int> stats) {
            UpdatePlayerSaveFile(charData, CreateWorldData(stats));
        }

        public static void SaveGame(CharData charData, WorldData worldData) {
            UpdatePlayerSaveFile(charData, worldData);
        }

        public static SaveData ContinueGame(String name) {
            SaveData data = LoadPlayerSaveFile(name);
            return data;
        }



        public static CharData FormatCharData(String playerName, int gender, int hairstyleID, 
                                                    Color manaColor, Color skinColor, Color hairColor, Color shirtColor) { // in order, stats go: [ATK, DEF, VIT, DEX, INT, WIS, VIR, EVL]

            CharData playerData = new CharData(playerName, gender, hairstyleID, manaColor, skinColor, hairColor, shirtColor);

            return playerData;
        }


        public static WorldData FormatWorldData(int level, int exp,
                                                int curHP, int curMP, int curMoney,
                                                List<int> stats, 
                                                List<int> inventoryItems, List<int> inventoryItemCounts, 
                                                List<int> unlockedSkillIDs, List<double> skillXPs, 
                                                int curZoneID, Vector2 curWorldPos, 
                                                List<int> curSummonIDs) {

            WorldData saveData = new WorldData(level, exp, curHP, curMP, curMoney, stats, inventoryItems, inventoryItemCounts, unlockedSkillIDs, skillXPs, curZoneID, curWorldPos, curSummonIDs);
            return saveData;
        }

        public static WorldData CreateWorldData(List<int> stats) {
            WorldData saveData = new WorldData(1, 0, 100, 100, 0, stats, new List<int>(), new List<int>(), new List<int>() { 0 }, new List<double>(), 0, Vector2.Zero, new List<int>());
            return saveData;
        }








        public static void UpdatePlayerSaveFile(CharData charData, WorldData worldData) {
            String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split(new List<char>() { '\\' }.ToArray(), 2)[1];
            //PLACEHOLDER
            //userName = "Nick";
            //PLACEHOLDER
            if (!Directory.Exists("C:/Users/" + userName + "/Documents/My Games/Afterhour/")) {
                Directory.CreateDirectory("C:/Users/" + userName + "/Documents/My Games/Afterhour/");
            }

            String filepath = "C:/Users/" + userName + "/Documents/My Games/Afterhour/" + charData.name + ".ah";

            using (BinaryWriter writer = new BinaryWriter(File.Open(filepath, FileMode.Create))) {
                //Char Data
                writer.Write(charData.name);
                writer.Write(charData.gender);
                writer.Write(charData.hairstyleID);

                writer.Write(charData.manaColor.R);
                writer.Write(charData.manaColor.G);
                writer.Write(charData.manaColor.B);

                writer.Write(charData.skinColor.R);
                writer.Write(charData.skinColor.G);
                writer.Write(charData.skinColor.B);

                writer.Write(charData.hairColor.R);
                writer.Write(charData.hairColor.G);
                writer.Write(charData.hairColor.B);

                writer.Write(charData.shirtColor.R);
                writer.Write(charData.shirtColor.G);
                writer.Write(charData.shirtColor.B);
                //

                //World Data
                writer.Write(worldData.curLevel); //Level
                writer.Write(worldData.curExp); //Experience

                writer.Write(worldData.curHP); //HP
                writer.Write(worldData.curMP); //MP

                writer.Write(worldData.curMoney); //Money

                writer.Write(worldData.stats.Count()); //Stats
                foreach(int stat in worldData.stats) {
                    writer.Write(stat);
                }

                writer.Write(worldData.inventoryItems.Count()); //Inventory Items
                foreach (int item in worldData.inventoryItems) {
                    writer.Write(item);
                }

                writer.Write(worldData.inventoryItemCounts.Count()); //Inventory Item counts
                foreach (int count in worldData.inventoryItemCounts) {
                    writer.Write(count);
                }

                writer.Write(worldData.unlockedSkillIDs.Count()); //Unlocked Skill IDs
                foreach (int skillID in worldData.unlockedSkillIDs) {
                    writer.Write(skillID);
                }

                writer.Write(worldData.skillXPs.Count()); //Unlocked Skill XP Values
                foreach (double skillXPVal in worldData.skillXPs) {
                    writer.Write(skillXPVal);
                }

                writer.Write(worldData.curZoneID); //Current zone ID

                writer.Write((int)worldData.curWorldPos.X); //WorldPos X
                writer.Write((int)worldData.curWorldPos.Y); //WorldPos Y

                writer.Write(worldData.curSummonIDs.Count()); //Current avaliable summon ID's
                foreach (int summonID in worldData.curSummonIDs) {
                    writer.Write(summonID);
                }
                //
            }
        }

        public static SaveData LoadPlayerSaveFile(String name) {
            String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split(new List<char>() { '\\' }.ToArray(), 2)[1];

            System.Diagnostics.Debug.WriteLine("Current reading filepath: " + "C:/Users/" + userName + "/Documents/My Games/Afterhour/" + name + ".ah");
            
            //PLACEHOLDER
            //userName = "Nick";
            //PLACEHOLDER
            String filepath = "C:/Users/" + userName + "/Documents/My Games/Afterhour/" + name + ".ah";

            CharData loadedCharData = new CharData();
            WorldData loadedWorldData = new WorldData();
            SaveData fullLoadedData = null;

            if (File.Exists(filepath)) {
                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open))) {
                    //Char Data
                    loadedCharData.name = reader.ReadString();
                    loadedCharData.gender = reader.ReadInt32();
                    loadedCharData.hairstyleID = reader.ReadInt32();

                    Color tempManaColor = Color.White;
                    tempManaColor.R = reader.ReadByte();
                    tempManaColor.G = reader.ReadByte();
                    tempManaColor.B = reader.ReadByte();
                    loadedCharData.manaColor = tempManaColor;

                    Color tempSkinColor = Color.White;
                    tempSkinColor.R = reader.ReadByte();
                    tempSkinColor.G = reader.ReadByte();
                    tempSkinColor.B = reader.ReadByte();
                    loadedCharData.skinColor = tempSkinColor;

                    Color tempHairColor = Color.White;
                    tempHairColor.R = reader.ReadByte();
                    tempHairColor.G = reader.ReadByte();
                    tempHairColor.B = reader.ReadByte();
                    loadedCharData.hairColor = tempHairColor;

                    Color tempShirtColor = Color.White;
                    tempShirtColor.R = reader.ReadByte();
                    tempShirtColor.G = reader.ReadByte();
                    tempShirtColor.B = reader.ReadByte();
                    loadedCharData.shirtColor = tempShirtColor;
                    //

                    //World Data
                    loadedWorldData.curLevel = reader.ReadInt32(); //Level
                    loadedWorldData.curExp = reader.ReadInt32(); //Experienece

                    loadedWorldData.curHP = reader.ReadInt32();
                    loadedWorldData.curMP = reader.ReadInt32();

                    loadedWorldData.curMoney = reader.ReadInt32();

                    int statCount = reader.ReadInt32(); //Stats
                    for (int i=0;i<statCount;i++) {
                        loadedWorldData.stats.Add(reader.ReadInt32());
                    }

                    int inventoryItemCount = reader.ReadInt32(); //Inventory Items
                    for (int i = 0; i < inventoryItemCount; i++) {
                        loadedWorldData.inventoryItems.Add(reader.ReadInt32());
                    }

                    int inventoryItemNumCount = reader.ReadInt32(); //Inventory Item counts
                    for (int i = 0; i < inventoryItemNumCount; i++) {
                        loadedWorldData.inventoryItemCounts.Add(reader.ReadInt32());
                    }

                    int unlockedSkillIDsCount = reader.ReadInt32(); //Unlocked Skill IDs
                    for (int i = 0; i < unlockedSkillIDsCount; i++) {
                        loadedWorldData.unlockedSkillIDs.Add(reader.ReadInt32());
                    }

                    double skillXPsCount = reader.ReadInt32(); //Unlocked Skill XP Values
                    for (int i = 0; i < skillXPsCount; i++) {
                        loadedWorldData.skillXPs.Add(reader.ReadDouble());
                    }

                    loadedWorldData.curZoneID = reader.ReadInt32(); //Current zone ID

                    Vector2 tempWorldPos = new Vector2(0, 0);
                    tempWorldPos.X = reader.ReadInt32(); //WorldPos X
                    tempWorldPos.Y = reader.ReadInt32(); //WorldPos Y
                    loadedWorldData.curWorldPos = tempWorldPos;

                    int summonableCount = reader.ReadInt32(); //Current avaliable summon ID's
                    for (int i = 0; i < summonableCount; i++) {
                        loadedWorldData.curSummonIDs.Add(reader.ReadInt32());
                    }
                    //
                }
                fullLoadedData = new SaveData(loadedCharData, loadedWorldData);
            }else {
                System.Diagnostics.Debug.WriteLine("File at path " + filepath + " not found.");
            }


            return fullLoadedData;
        }

    }




    public class CharData {
        public String name { get; set; }
        public int gender { get; set; }
        public int hairstyleID { get; set; }
        public Color manaColor { get; set; }
        public Color skinColor { get; set; }
        public Color hairColor { get; set; }
        public Color shirtColor { get; set; }


        public CharData(String name, int gender, int hairstyleID, Color manaColor, Color skinColor, Color hairColor, Color shirtColor) {
            this.name = name;
            this.gender = gender;
            this.hairstyleID = hairstyleID;
            this.manaColor = manaColor;
            this.skinColor = skinColor;
            this.hairColor = hairColor;
            this.shirtColor = shirtColor;
        }

        public CharData() {
            this.manaColor = Color.White;
            this.skinColor = Color.White;
            this.hairColor = Color.White;
            this.shirtColor = Color.White;
        }
    }




    public class WorldData {
        public int curLevel { get; set; }
        public int curExp { get; set; }
        public int curHP { get; set; }
        public int curMP { get; set; }
        public int curMoney { get; set; }
        public List<int> stats { get; set; }
        public List<int> inventoryItems { get; set; }
        public List<int> inventoryItemCounts { get; set; }
        public List<int> unlockedSkillIDs { get; set; }
        public List<double> skillXPs { get; set; }
        public int curZoneID { get; set; }
        public Vector2 curWorldPos { get; set; }
        public List<int> curSummonIDs { get; set; }


        public WorldData(int curLevel, int curExp, int curHP, int curMP, int curMoney,
                         List<int> stats, List<int> inventoryItems, List<int> inventoryItemCounts,
                         List<int> unlockedSkillIDs, List<double> skillXPs,
                         int curZoneID, Vector2 curWorldPos,
                         List<int> curSummonIDs) {

            this.curLevel = curLevel;
            this.curExp = curExp;
            this.curHP = curHP;
            this.curMP = curMP;
            this.curMoney = curMoney;
            this.stats = stats;
            this.inventoryItems = inventoryItems;
            this.inventoryItemCounts = inventoryItemCounts;
            this.unlockedSkillIDs = unlockedSkillIDs;
            this.skillXPs = skillXPs;
            this.curZoneID = curZoneID;
            this.curWorldPos = curWorldPos;
            this.curSummonIDs = curSummonIDs;
        }

        public WorldData() {
            this.curLevel = 1;
            this.curExp = 0;
            this.curHP = 100;
            this.curMP = 100;
            this.curMoney = 0;
            this.stats = new List<int>();
            this.inventoryItems = new List<int>();
            this.inventoryItemCounts = new List<int>();
            this.unlockedSkillIDs = new List<int>();
            this.skillXPs = new List<double>();
            this.curWorldPos = new Vector2(0, 0);
            this.curSummonIDs = new List<int>();
        }
    }


    public class SaveData {

        public CharData charData { get; set; }
        public WorldData worldData { get; set; }

        public SaveData(CharData charData, WorldData worldData) {
            this.charData = charData;
            this.worldData = worldData;
        }

    }

}
