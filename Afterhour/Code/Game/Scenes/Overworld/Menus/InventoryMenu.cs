using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.FileHandling;
using Afterhour.Code.Handling.DataFiles;

namespace Afterhour.Code.Game.Scenes.Overworld.Menus {
    public class InventoryMenu {

        private List<int> itemList;
        private List<int> itemCounts;

        private String playerName;
        private int curHP;
        private int maxHP;
        private int curMP;
        private int maxMP;
        private int money;

        private List<int> statValues;
        private List<Texture2D> statIcons;


        private SpriteFont titleFont;
        private Texture2D emptySquare;
        private Texture2D fullSqaure;

        private Texture2D backgroundTint;
        private Rectangle backgroundTintRect;

        private ExpBar expBar;

        private int squareSpacing = 8;

        private List<InventoryFolder> folders = new List<InventoryFolder>();
        private List<String> folderTitles = new List<String>();

        private FrameAnim folderArrow_Right;
        private FrameAnim folderArrow_Left;

        private int curFolder = 0; //Type of inventory category; i.e. weapons, armor, etc
        private int curPage = 0; //Page of inventory

        //Variables
        public InventoryMenu() {
            this.expBar = new ExpBar();
        }

        public InventoryMenu(List<int> itemList, List<int> itemCounts) {
            this.itemList = itemList;
            this.itemCounts = itemCounts;

            this.expBar = new ExpBar();

            folders.Add(new InventoryFolder(itemList, itemCounts)); //itemList needs to split again into what each folder's category is (armor, weapons, food, etc.)
        }

        public void Reconstruct(SaveData playerData) {
            this.itemList = playerData.worldData.inventoryItems;
            this.itemCounts = playerData.worldData.inventoryItemCounts;

            this.playerName = playerData.charData.name;

            this.curHP = playerData.worldData.curHP;
            this.curMP = playerData.worldData.curMP;

            this.maxHP = 100;
            this.maxMP = 100;

            this.money = playerData.worldData.curMoney;

            this.statValues = playerData.worldData.stats;

            this.folderTitles.Add("Materials");
            this.folderTitles.Add("Food");
            this.folderTitles.Add("Weapons");
            this.folderTitles.Add("Arcana");
            this.folderTitles.Add("Armor");
            this.folderTitles.Add("Key Items");

            SplitInventoryItems(itemList, itemCounts);

        }


        public void LoadContent(Resources res) {
            this.titleFont = res.Menu_Font;
            this.emptySquare = res.Inventory_Square_Empty;
            this.fullSqaure = res.Inventory_Square_Full;

            this.statIcons = new List<Texture2D>();
            this.statIcons.Add(res.Menu_Stats_ATK);
            this.statIcons.Add(res.Menu_Stats_DEF);
            this.statIcons.Add(res.Menu_Stats_VIT);
            this.statIcons.Add(res.Menu_Stats_DEX);
            this.statIcons.Add(res.Menu_Stats_INT);
            this.statIcons.Add(res.Menu_Stats_WIS);
            this.statIcons.Add(res.Menu_Stats_VIR);
            this.statIcons.Add(res.Menu_Stats_EVL);

            this.folderArrow_Right = new FrameAnim(res.Inventory_ArrowAnim_Right, 8, 8, 15, 50, true, true);
            this.folderArrow_Left = new FrameAnim(res.Inventory_ArrowAnim_Left, 8, 8, 15, 50, false, true);

            this.backgroundTint = res.Battle_FadeRect;
            this.backgroundTintRect = new Rectangle(0, 0, (int)Window.resolution.X, (int)Window.resolution.Y);

            this.expBar.LoadContent(res);
        }

        public void Update(GameHandler gh) {
            InputHandler input = gh.inputHandler;
            if(input.keyboardState.IsKeyUp(Keys.A) && input.keyboardState_old.IsKeyDown(Keys.A)) {
                if (this.curFolder - 1 < 0) {
                    this.curFolder = folders.Count() - 1;
                }else {
                    this.curFolder--;
                }
            }

            if (input.keyboardState.IsKeyUp(Keys.D) && input.keyboardState_old.IsKeyDown(Keys.D)) {
                if (this.curFolder + 1 > folders.Count()-1) {
                    this.curFolder = 0;
                } else {
                    this.curFolder++;
                }
            }

            folderArrow_Left.Update(gh.gameTime);
            folderArrow_Right.Update(gh.gameTime);

            expBar.Update(gh.gameTime.ElapsedGameTime.Milliseconds, gh.worldData.curExp);
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.backgroundTint, this.backgroundTintRect, Color.Black * 0.75f);

            sb.DrawString(this.titleFont, "Inventory", new Vector2(Window.resolution.X / 2 - this.titleFont.MeasureString("Inventory").X / 2, 40), Color.White);

            Vector2 folderNamePos = new Vector2((int)(25 + (squareSpacing * 2) + (emptySquare.Width * 2) + (emptySquare.Width / 2) - (titleFont.MeasureString(folderTitles[curFolder]).X / 2)), 108);
            sb.DrawString(titleFont, folderTitles[curFolder], folderNamePos, Color.White);
            folderArrow_Left.Draw(sb, new Vector2(folderNamePos.X - 7 - folderArrow_Left.frameWidth, folderNamePos.Y));
            folderArrow_Right.Draw(sb, new Vector2(folderNamePos.X + 7 + titleFont.MeasureString(folderTitles[curFolder]).X, folderNamePos.Y));

            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 5; x++) {
                    if (folders[curFolder].pages[curPage].squares[x + (y * 5)].isEmpty) {                        
                        sb.Draw(this.emptySquare, new Vector2(25 + (squareSpacing * x) + (x * emptySquare.Width), 150 + (squareSpacing * y) + (y * emptySquare.Width)), Color.White);
                    }else {
                        sb.Draw(this.fullSqaure, new Vector2(25 + (squareSpacing * x) + (x * fullSqaure.Width), 150 + (squareSpacing * y) + (y * fullSqaure.Width)), Color.White);

                    }
                    //System.Diagnostics.Debug.WriteLine("Square (" + x + ", " + y + ") is empty: " + folders[curFolder].pages[curPage].squares[x + y * 5].isEmpty);
                }
            }


            //Draw name, hp, mp, and money
            sb.DrawString(this.titleFont, this.playerName, new Vector2(Window.resolution.X - 205, Window.resolution.Y - 430), Color.White);
            sb.DrawString(this.titleFont, "HP: " + this.curHP + "/" + this.maxHP, new Vector2(Window.resolution.X - 225, Window.resolution.Y - 405), Color.White);
            sb.DrawString(this.titleFont, "MP: " + this.curMP + "/" + this.maxMP, new Vector2(Window.resolution.X - 225, Window.resolution.Y - 380), Color.White);
            sb.DrawString(this.titleFont, "Money: " + this.money, new Vector2(Window.resolution.X - 220, Window.resolution.Y - 355), Color.White);
            //

            //Left column
            for (int i = 0; i < 4; i++) {
                sb.Draw(this.statIcons[i], new Vector2(Window.resolution.X - 265, Window.resolution.Y - 320 + (45*i)), Color.White);
                sb.DrawString(this.titleFont, this.statValues[i].ToString(), new Vector2(Window.resolution.X - 220, Window.resolution.Y - 320 + (45 * i) + 10), Color.White);
            }
            //

            //Right column
            for (int j = 4; j < 8; j++) {
                int i = j - 4;
                sb.Draw(this.statIcons[j], new Vector2(Window.resolution.X - 160, Window.resolution.Y - 320 + (45 * i)), Color.White);
                sb.DrawString(this.titleFont, this.statValues[j].ToString(), new Vector2(Window.resolution.X - 115, Window.resolution.Y - 320 + (45 * i) + 10), Color.White);
            }
            //

            expBar.Draw(sb, new Vector2(558, 500));

        }



        private void SplitInventoryItems(List<int> itemList, List<int> itemCounts) {
            List<int> materialList = new List<int>();
            List<int> foodList = new List<int>();
            List<int> weaponList = new List<int>();
            List<int> arcanaList = new List<int>();
            List<int> armorList = new List<int>();
            List<int> keyList = new List<int>();

            List<int> materialCounts = new List<int>();
            List<int> foodCounts = new List<int>();
            List<int> weaponCounts = new List<int>();
            List<int> arcanaCounts = new List<int>();
            List<int> armorCounts = new List<int>();
            List<int> keyCounts = new List<int>();

            for (int i = 0; i < ItemData.ITEMCOUNT; i++) {
                if(i+1 <= itemList.Count()) {
                    int curItemID = itemList[i];
                    int curItemCount = itemCounts[i];

                    if (curItemID <= 500) {
                        materialList.Add(curItemID);
                        materialCounts.Add(curItemCount);
                    } else if (curItemID > 500 && curItemID <= 800) {
                        foodList.Add(curItemID);
                        foodCounts.Add(curItemCount);
                    } else if (curItemID > 800 && curItemID <= 1100) {
                        weaponList.Add(curItemID);
                        weaponCounts.Add(curItemCount);
                    } else if (curItemID > 1100 && curItemID <= 1400) {
                        arcanaList.Add(curItemID);
                        arcanaCounts.Add(curItemCount);
                    } else if (curItemID > 1400 && curItemID <= 1600) {
                        armorList.Add(curItemID);
                        armorCounts.Add(curItemCount);
                    } else if (curItemID > 1600 && curItemID <= 1700) {
                        keyList.Add(curItemID);
                        keyCounts.Add(curItemCount);
                    }
                }else {
                    break;
                }
            }

            folders.Add(new InventoryFolder(materialList, materialCounts));
            folders.Add(new InventoryFolder(foodList, foodCounts));
            folders.Add(new InventoryFolder(weaponList, weaponCounts));
            folders.Add(new InventoryFolder(arcanaList, arcanaCounts));
            folders.Add(new InventoryFolder(armorList, armorCounts));
            folders.Add(new InventoryFolder(keyList, keyCounts));


        }


    }


    public class InventorySquare {
        public bool isEmpty;
        public int itemID;
        public int itemCount;

        public InventorySquare(bool empty, int itemID, int itemCount) {
            this.isEmpty = empty;
            this.itemID = itemID;
            this.itemCount = itemCount;
        }
    }

    public class InventoryPage {
        public List<InventorySquare> squares = new List<InventorySquare>();

        public InventoryPage(List<int> itemIDs, List<int> itemCounts) {
            for(int i = 0; i < 20; i++) {
                    if (i + 1 <= itemIDs.Count()) {
                        this.squares.Add(new InventorySquare(false, itemIDs[i], itemCounts[i]));
                    } else {
                        this.squares.Add(new InventorySquare(true, 0, 0));
                    }
                
            }
        }
    }

    public class InventoryFolder {
        public List<InventoryPage> pages = new List<InventoryPage>();

        public InventoryFolder(List<InventoryPage> pages) {
            this.pages = pages;
        }

        public InventoryFolder(List<int> itemIDs, List<int> itemCounts) {
            int pageCount = Math.Max(1, (int)Math.Ceiling((double)itemIDs.Count() / 25.0));
            int remainder = itemIDs.Count() % 25;
              
            for(int i = 0; i < pageCount; i++) {
                int range = 25;
                if(i*25 >= itemIDs.Count()-remainder) {
                    range = remainder;
                }

                this.pages.Add(new InventoryPage(itemIDs.GetRange(i * 25, range), itemCounts.GetRange(i * 25, range)));
            }
        }

    }


}
