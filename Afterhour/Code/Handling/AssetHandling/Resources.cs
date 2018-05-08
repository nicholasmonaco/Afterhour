using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Game.Scenes.Overworld.Entities;
using Afterhour.Code.Game.Scenes.Battle;
using Afterhour.Code.Handling.FileHandling;

namespace Afterhour.Code.Handling.AssetHandling {
    public class Resources {

        //--- Resource List

        //Menu//
        public Texture2D Menu_Title;

        public SpriteFont Menu_Font;
        public SpriteFont Menu_CreationFont;
        public Texture2D Menu_MousePointer;
        public Texture2D Menu_MousePointer_anim;

        public Texture2D Menu_Background;
        public Texture2D Menu_CreationFrame;

        public Texture2D Menu_TypeBox;
        public Texture2D Menu_ListClicker_Arrow;

        public Texture2D Menu_HSVSlider_Pointer;
        public Texture2D Menu_HSVSlider_Base;
        public Texture2D Menu_HSVSlider_H;
        public Texture2D Menu_HSVSlider_S;
        public Texture2D Menu_HSVSlider_V;

        public Texture2D Menu_CharPrev_Frame;

        public Texture2D Menu_CharCreation_ManaGlow;
        public Texture2D Menu_CharCreation_Skin;
        public Texture2D Menu_CharCreation_Bottom;
        public Texture2D Menu_CharCreation_Shirt;


        public Texture2D Menu_CharCreation_Hair_1;

        public Texture2D Menu_NextButton;

        public Texture2D Menu_Stats_Minus;
        public Texture2D Menu_Stats_Plus;

        public Texture2D Menu_Stats_ATK;
        public Texture2D Menu_Stats_DEF;
        public Texture2D Menu_Stats_VIT;
        public Texture2D Menu_Stats_DEX;
        public Texture2D Menu_Stats_INT;
        public Texture2D Menu_Stats_WIS;
        public Texture2D Menu_Stats_VIR;
        public Texture2D Menu_Stats_EVL;
        public Texture2D Menu_Stats_LUK;

        public Texture2D Menu_Stats_HoverTextBackground;

        public Texture2D Menu_Continue_Frame;
        public Texture2D Menu_Continue_SaveBar;

        public Texture2D Menu_Continue_FuncButton_Play;
        public Texture2D Menu_Continue_FuncButton_Delete;

        public Texture2D Menu_LoadingAnim;

        public Texture2D Icons_Frame;

        public Texture2D Icons_GenderIcons_Female;
        public Texture2D Icons_GenderIcons_Male;
        //Menu//

        //Overworld//
        public Texture2D Tilesets_test1;

        public Texture2D TileAnims_Water;


        public Texture2D FrameAnim_test1;

        public Texture2D Player_testAnim1;
        public SoundEffect Player_Affinity_ManaPulse;

        public Texture2D NPC_VillagerAnim;

        public Texture2D Enemy_Slime;


        public Texture2D World_Interaction_TalkWindowAnim;
        public Texture2D World_Interaction_TalkWindowOpen;

        public Texture2D Minimap_Border;
        public Texture2D Minimap_Border_Outline;
        public Texture2D Minimap_Fill;
        public Texture2D Minimap_Outline;
        public Texture2D Minimap_Icon_Player;
        public Texture2D Minimap_Icon_Enemy;

        public Texture2D Minimap_MaskTex;
        public Texture2D Minimap_MapTex;
        public Texture2D Minimap_MaskBorder;
        //Overworld//

        //Inventory//
        public Texture2D Inventory_Square_Empty;
        public Texture2D Inventory_Square_Full;

        public Texture2D Inventory_ArrowAnim_Right;
        public Texture2D Inventory_ArrowAnim_Left;

        public Texture2D Inventory_ExpBar_Frame;
        public Texture2D Inventory_ExpBar_Fill;
        public Texture2D Inventory_ExpBar_Indexers;
        //Inventory//

        //Battle//
        public SpriteFont Battle_CountDownFont;
        public SpriteFont Battle_HealthFont;

        public Texture2D Battle_FadeRect;

        public List<Texture2D> Battle_BackgroundTexList = new List<Texture2D>();
        public List<Texture2D> Battle_FloorTexList = new List<Texture2D>();

        public List<Texture2D> Battle_IdleEnemySpriteSheets = new List<Texture2D>();
        public List<List<int>> Battle_IdleEnemyAnimData = new List<List<int>>();

        public List<Texture2D> Battle_FleeEnemySpriteSheets = new List<Texture2D>();
        public List<List<int>> Battle_FleeEnemyAnimData = new List<List<int>>();

        public List<List<Texture2D>> Battle_FightEnemySpriteSheets = new List<List<Texture2D>>();
        public List<List<List<int>>> Battle_FightEnemyAnimData = new List<List<List<int>>>();

        public Texture2D Battle_UI_HealthBarEmpty;
        public Texture2D Battle_UI_HealthBarFull;

        public Texture2D Battle_UI_OptionWheel_HighlighterSpriteSheet;
        public Texture2D Battle_UI_OptionWheel_Fight;
        public Texture2D Battle_UI_OptionWheel_Item;
        public Texture2D Battle_UI_OptionWheel_Pass;
        public Texture2D Battle_UI_OptionWheel_Flee;

        public Texture2D Battle_UI_SelectorArrowSpriteSheet;

        public Texture2D Battle_Move_Icon_Flare;

        public Texture2D Battle_FighterShadow;


        public List<FileStream> MoveDataStreamList = new List<FileStream>();
        public String MoveDataStreamDirPath;

        public Dictionary<int, Texture2D> MoveIconTexDict = new Dictionary<int, Texture2D>();
        public List<FrameAnim> MovePrimaryAnims = new List<FrameAnim>();

        public Dictionary<int, List<int>> MovePrimaryAnimsPerMove = new Dictionary<int, List<int>>();
        public Dictionary<int, List<FrameAnim>> MoveSecondaryAnims = new Dictionary<int, List<FrameAnim>>();
        //Battle//

        //Debug//
        public Texture2D Debug_BoundsRect;
        //Debug//

        //---

        private String contentPath;


        public Resources(ContentManager content) { //Constructor to load all of the content into the variables
            this.contentPath = content.RootDirectory;

            this.Menu_Title = content.Load<Texture2D>("Textures/Menu/Title");

            this.Menu_Font = content.Load<SpriteFont>("Fonts/MenuFont");
            this.Menu_CreationFont = content.Load<SpriteFont>("Fonts/MenuFont");
            this.Menu_MousePointer = content.Load<Texture2D>("Textures/Menu/MousePointer");
            this.Menu_MousePointer_anim = content.Load<Texture2D>("Textures/Menu/MousePointer_anim");

            this.Menu_Background = content.Load<Texture2D>("Textures/Menu/Background2");
            this.Menu_CreationFrame = content.Load<Texture2D>("Textures/Menu/CreationFrame");

            this.Menu_TypeBox = content.Load<Texture2D>("Textures/Menu/TypeBoxFrame");
            this.Menu_ListClicker_Arrow = content.Load<Texture2D>("Textures/Menu/Arrow");

            this.Menu_HSVSlider_Pointer = content.Load<Texture2D>("Textures/Menu/Slider_Pointer");
            this.Menu_HSVSlider_Base = content.Load<Texture2D>("Textures/Menu/Slider_Base");
            this.Menu_HSVSlider_H = content.Load<Texture2D>("Textures/Menu/Slider_H");
            this.Menu_HSVSlider_S = content.Load<Texture2D>("Textures/Menu/Slider_S");
            this.Menu_HSVSlider_V = content.Load<Texture2D>("Textures/Menu/Slider_V");

            this.Menu_CharPrev_Frame = content.Load<Texture2D>("Textures/Menu/CharPrevFrame");

            this.Menu_CharCreation_ManaGlow = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Body/ManaGlow");
            this.Menu_CharCreation_Skin = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Body/Skin");
            this.Menu_CharCreation_Bottom = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Body/Bottom");
            this.Menu_CharCreation_Shirt = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Body/Shirt");


            this.Menu_CharCreation_Hair_1 = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Hairstyles/1");

            this.Menu_NextButton = content.Load<Texture2D>("Textures/Menu/NextButtonFrame");

            this.Menu_Stats_Minus = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Minus");
            this.Menu_Stats_Plus = content.Load<Texture2D>("Textures/Menu/CharacterCreation/Plus");

            this.Menu_Stats_ATK = content.Load<Texture2D>("Textures/Menu/CharacterCreation/ATK");
            this.Menu_Stats_DEF = content.Load<Texture2D>("Textures/Menu/CharacterCreation/DEF");
            this.Menu_Stats_VIT = content.Load<Texture2D>("Textures/Menu/CharacterCreation/VIT");
            this.Menu_Stats_DEX = content.Load<Texture2D>("Textures/Menu/CharacterCreation/DEX");
            this.Menu_Stats_INT = content.Load<Texture2D>("Textures/Menu/CharacterCreation/INT");
            this.Menu_Stats_WIS = content.Load<Texture2D>("Textures/Menu/CharacterCreation/WIS");
            this.Menu_Stats_VIR = content.Load<Texture2D>("Textures/Menu/CharacterCreation/VIR");
            this.Menu_Stats_EVL = content.Load<Texture2D>("Textures/Menu/CharacterCreation/EVL");
            this.Menu_Stats_LUK = content.Load<Texture2D>("Textures/Menu/CharacterCreation/LUK");

            this.Menu_Stats_HoverTextBackground = content.Load<Texture2D>("Textures/Menu/HoverBoxTextBackground");

            this.Menu_Continue_Frame = content.Load<Texture2D>("Textures/Menu/ContinueMenu/ContinueFrame");
            this.Menu_Continue_SaveBar = content.Load<Texture2D>("Textures/Menu/ContinueMenu/SaveBar");
            this.Menu_Continue_FuncButton_Play = content.Load<Texture2D>("Textures/Menu/ContinueMenu/FuncButton_Play");
            this.Menu_Continue_FuncButton_Delete = content.Load<Texture2D>("Textures/Menu/ContinueMenu/FuncButton_Delete");

            this.Menu_LoadingAnim = content.Load<Texture2D>("Textures/Menu/Hourglass_Anim");


            this.Icons_Frame = content.Load<Texture2D>("Textures/Icons/Frame");

            this.Icons_GenderIcons_Female = content.Load<Texture2D>("Textures/Icons/Gender/Female");
            this.Icons_GenderIcons_Male = content.Load<Texture2D>("Textures/Icons/Gender/Male");


            this.Tilesets_test1 = content.Load<Texture2D>("Textures/World/Tilesets/test1");

            this.TileAnims_Water = content.Load<Texture2D>("Textures/World/Tilesets/Anims/water_anim");


            this.FrameAnim_test1 = content.Load<Texture2D>("Textures/World/Entities/animTest1");

            this.Player_testAnim1 = content.Load<Texture2D>("Textures/World/Entities/Player/testPlayer");
            this.Player_Affinity_ManaPulse = content.Load<SoundEffect>("Sounds/Wave1");

            this.NPC_VillagerAnim = content.Load<Texture2D>("Textures/World/Entities/NPCs/Villager_anim");

            this.Enemy_Slime = content.Load<Texture2D>("Textures/World/Entities/Enemies/Slime");


            this.World_Interaction_TalkWindowAnim = content.Load<Texture2D>("Textures/World/FX/TalkWindow_anim");
            this.World_Interaction_TalkWindowOpen = content.Load<Texture2D>("Textures/World/FX/TalkWindow");

            this.Minimap_Border = content.Load<Texture2D>("Textures/World/Minimap/Border");
            this.Minimap_Border_Outline = content.Load<Texture2D>("Textures/World/Minimap/Border_outline");
            this.Minimap_Fill = content.Load<Texture2D>("Textures/World/Minimap/Fill");
            this.Minimap_Outline = content.Load<Texture2D>("Textures/World/Minimap/Outline");
            this.Minimap_Icon_Player = content.Load<Texture2D>("Textures/World/Minimap/Icon_Player_Small");
            this.Minimap_Icon_Enemy = content.Load<Texture2D>("Textures/World/Minimap/Icon_Enemy");

            this.Minimap_MaskTex = content.Load<Texture2D>("Textures/World/Minimap/MaskTex");
            this.Minimap_MapTex = content.Load<Texture2D>("Textures/World/Minimap/MapTex");
            this.Minimap_MaskBorder = content.Load<Texture2D>("Textures/World/Minimap/MaskBorder");



            this.Inventory_Square_Empty = content.Load<Texture2D>("Textures/Inventory/Square_Blank");
            this.Inventory_Square_Full = content.Load<Texture2D>("Textures/Inventory/Square_Item");

            this.Inventory_ArrowAnim_Right = content.Load<Texture2D>("Textures/Inventory/InventoryArrowAnim_Right");
            this.Inventory_ArrowAnim_Left = content.Load<Texture2D>("Textures/Inventory/InventoryArrowAnim_Left");

            this.Inventory_ExpBar_Frame = content.Load<Texture2D>("Textures/Inventory/ExpBar_Empty");
            this.Inventory_ExpBar_Fill = content.Load<Texture2D>("Textures/Inventory/ExpBar_Full");
            this.Inventory_ExpBar_Indexers = content.Load<Texture2D>("Textures/Inventory/ExpBar_Indexers");


            this.Battle_CountDownFont = content.Load<SpriteFont>("Fonts/BattleCountdownFont");
            this.Battle_HealthFont = content.Load<SpriteFont>("Fonts/BattleHealthFont");

            this.Battle_FadeRect = content.Load<Texture2D>("Textures/Battle/FadeScreen");

            this.Battle_BackgroundTexList.Add(content.Load<Texture2D>("Textures/Battle/Stage/Backgrounds/Stars"));
            this.Battle_FloorTexList.Add(content.Load<Texture2D>("Textures/Battle/Stage/Floors/Test1"));

            this.Battle_UI_HealthBarEmpty = content.Load<Texture2D>("Textures/Battle/UI/HealthBar_Base");
            this.Battle_UI_HealthBarFull = content.Load<Texture2D>("Textures/Battle/UI/HealthBar_Full");

            this.Battle_UI_OptionWheel_HighlighterSpriteSheet = content.Load<Texture2D>("Textures/Battle/UI/WheelOption_Highlight");

            this.Battle_UI_OptionWheel_Fight = content.Load<Texture2D>("Textures/Battle/UI/WheelOption_Fight");
            this.Battle_UI_OptionWheel_Item = content.Load<Texture2D>("Textures/Battle/UI/WheelOption_Item");
            this.Battle_UI_OptionWheel_Pass = content.Load<Texture2D>("Textures/Battle/UI/WheelOption_Pass");
            this.Battle_UI_OptionWheel_Flee = content.Load<Texture2D>("Textures/Battle/UI/WheelOption_Flee");

            this.Battle_UI_SelectorArrowSpriteSheet = content.Load<Texture2D>("Textures/Battle/UI/EnemySelector");


            Move.LoadMoveDictionary(); //This has to be done to make the id/name dictionaries

            //Loading all of the player animations for all attacks
            MovePrimaryAnims.Add(new FrameAnim(this.Player_testAnim1, 8, 32, 32, 25, true, true)); //0: Move.PRIMARY_SHOOT
            //

            //Loading the projectile, aura, etc animation sheets for moves
            FrameAnim Battle_Move_Anim_Flare = new FrameAnim(content.Load<Texture2D>("Textures/Battle/Moves/Flare"), 4, 32, 32, 12, true, true);
            this.MovePrimaryAnimsPerMove.Add(Move.ID_DICT["Flare"], new List<int>() { Move.PRIMARY_SHOOT });
            this.MoveSecondaryAnims.Add(Move.ID_DICT["Flare"], new List<FrameAnim>() { Battle_Move_Anim_Flare });
            //

            //Loading all move icons --- This can be a for loop like with the directory file count and stuff but ill do it later
            for(int i = 0; i < Move.ID_DICT.Keys.Count(); i++) {
                this.MoveIconTexDict.Add(i, content.Load<Texture2D>("Textures/Battle/Moves/Icons/" + Move.NAME_DICT[i]));
                
            }
            //

            

            this.Battle_FighterShadow = content.Load<Texture2D>("Textures/Battle/FighterShadow");


            this.MoveDataStreamDirPath = this.contentPath + "/BattleData/MoveData";
            List<String> moveFilePathList = Directory.GetFiles(MoveDataStreamDirPath).ToList();
            for(int i = 0; i < moveFilePathList.Count(); i++) {
                //System.Diagnostics.Debug.WriteLine(moveFilePathList[i]);
                //MoveDataStreamList.Add(content.Load<FileStream>("BattleData/MoveData/" + moveFilePathList[i])); //This needs to be split to only include the name of the file, not the folder path or file extension
            }

            //EnemyData.Create(); //This makes new files for the enemy animation data; this should be commented out in the final version, it just exists toeract  generate the binary files
            EnemyData.Construct(); //This reads the already-existing enemy animation data

            LoadBattleAssets(content, "Textures/Battle/Fighters");

            MoveLoader.LoadContent(this);


            this.Debug_BoundsRect = content.Load<Texture2D>("Textures/World/Debug/boundsRect");

        }


        public void LoadBattleAssets(ContentManager content, String folderPath) { //need to make files for each thing below
            for(int i = 0; i <= Enemy.MAX_ID; i++) {
                //Idle Anims
                this.Battle_IdleEnemySpriteSheets.Add(content.Load<Texture2D>(folderPath + "/" + i + "_idleSheet"));
                this.Battle_IdleEnemyAnimData.Add(new List<int>() { EnemyData.frameCounts_Idle[i], EnemyData.frameTimes_Idle[i]});
                //
                //Fleeing Anims
                this.Battle_FleeEnemySpriteSheets.Add(content.Load<Texture2D>(folderPath + "/" + i + "_fleeSheet"));
                this.Battle_FleeEnemyAnimData.Add(new List<int>() { EnemyData.frameCounts_Flee[i], EnemyData.frameTimes_Flee[i] });
                //
                //Fighting Anims
                this.Battle_FightEnemySpriteSheets.Add(new List<Texture2D>());
                this.Battle_FightEnemyAnimData.Add(new List<List<int>>());
                for(int j = 0; j < EnemyData.attackCountPerEnemy[i]; j++) {
                    this.Battle_FightEnemySpriteSheets[i].Add(content.Load<Texture2D>(folderPath + "/" + i + "_fightSheet" + i));
                    this.Battle_FightEnemyAnimData[i].Add(new List<int>() { EnemyData.frameCounts_Fight[i][j], EnemyData.frameTimes_Fight[i][j] });
                }
                //
            }
        }

        public Texture2D GetBasicBattleSpriteSheet(int enemyID, String spriteSheetType) {
            Texture2D returnTex = null;
            switch (spriteSheetType) {
                case "idle":
                    returnTex = this.Battle_IdleEnemySpriteSheets[enemyID];
                    break;
                case "flee":
                    returnTex = this.Battle_IdleEnemySpriteSheets[enemyID];
                    break;
            }
            return returnTex;
        }

        public List<int> GetBasicBattleAnimData(int enemyID, String spriteSheetType) { //0 = # of Frames, 1 = MS per Frame
            List<int> returnData = new List<int>();
            switch (spriteSheetType) {
                case "idle":
                    returnData = (this.Battle_IdleEnemyAnimData[enemyID]);
                    break;
                case "flee":
                    returnData = (this.Battle_FleeEnemyAnimData[enemyID]);
                    break;
            }
            return returnData;
        }


        public List<Texture2D> GetFightBattleSpriteSheets(int enemyID) {
            return this.Battle_FightEnemySpriteSheets[enemyID];
        }

        public List<List<int>> GetFightBattleAnimData(int enemyID) {
            return this.Battle_FightEnemyAnimData[enemyID];
        }

    }
}
