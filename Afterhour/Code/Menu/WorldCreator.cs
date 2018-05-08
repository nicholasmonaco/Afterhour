using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.States;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.FileHandling;

namespace Afterhour.Code.Menu {
    public class WorldCreator {

        private List<String> steps = new List<String>();
        private String curStep;

        private Texture2D creationFrameTex;

        //Charatcer creation variables
        private TypeBox nameBox;
        private OptionGrid genderBoxes;
        private ListClicker hairstyleList;
        private ColorSlider manaColorSlider;
        private ColorSlider skinColorSlider;
        private ColorSlider hairColorSlider;
        private ColorSlider shirtColorSlider;


        private CharacterPreview charPrev;

        private NextButton nextButton1;
        private NextButton backButton1;

        private SpriteFont font;
        //

        //Stat distribution variables
        private int statCount = 8;
        private List<ClickBox> plusBoxes = new List<ClickBox>();
        private List<ClickBox> minusBoxes = new List<ClickBox>();
        private int statXOffset = 235;

        private List<int> statValues = new List<int>();
        private List<HoverBox> statIcons = new List<HoverBox>();

        private int remainingPoints = 25;

        private Texture2D hoverTex;

        private NextButton nextButton2;
        private NextButton backButton2;
        //


        public WorldCreator() {
            this.steps.Add("character"); //basic character customization; stardew valley basically
            this.steps.Add("stats"); //10 of each base stat at start, 25 points to distribute freely among them

            this.curStep = steps[0];


            nameBox = new TypeBox();
            genderBoxes = new OptionGrid(new Vector2(504, 200), 1, 2);
            hairstyleList = new ListClicker(new Vector2(460, 290), "Hairstyle ", 20);
            manaColorSlider = new ColorSlider(new Vector2(180, 170), "Mana Color");
            skinColorSlider = new ColorSlider(new Vector2(180, 245), "Skin Color");
            hairColorSlider = new ColorSlider(new Vector2(180, 320), "Hair Color");
            shirtColorSlider = new ColorSlider(new Vector2(180, 395), "Shirt Color");


            charPrev = new CharacterPreview(new Vector2(380, 200));

            nextButton1 = new NextButton(new Vector2(690, 525), "Next");
            backButton1 = new NextButton(new Vector2(15, 525), "Back");

            nextButton2 = new NextButton(new Vector2(690, 525), "Next");
            backButton2 = new NextButton(new Vector2(15, 525), "Back");

        }


        public void LoadContent(Resources res, SpriteFont font) {
            this.creationFrameTex = res.Menu_CreationFrame;

            nameBox.LoadContent(res);


            List<Texture2D> genderIcons = new List<Texture2D>() {res.Icons_GenderIcons_Female, res.Icons_GenderIcons_Male};
            genderBoxes.LoadContent(genderIcons, res);

            hairstyleList.LoadContent(res);

            manaColorSlider.LoadContent(res);
            skinColorSlider.LoadContent(res);
            hairColorSlider.LoadContent(res);
            shirtColorSlider.LoadContent(res);

            charPrev.LoadContent(res, this.genderBoxes.curSelectedID, hairstyleList.curElementID, manaColorSlider.curColor, skinColorSlider.curColor, hairColorSlider.curColor, shirtColorSlider.curColor);

            nextButton1.LoadContent(res);
            backButton1.LoadContent(res);


            //Stats
            for (int i = 0; i < statCount; i++) {
                plusBoxes.Add(new ClickBox(new Vector2((creationFrameTex.Width - 40) / statCount * i + (Window.resolution.X - creationFrameTex.Width)/2 + 40, statXOffset)));
                minusBoxes.Add(new ClickBox(new Vector2((creationFrameTex.Width - 40) / statCount * i + (Window.resolution.X - creationFrameTex.Width)/2 + 40, statXOffset + 90)));

                plusBoxes[i].LoadContent(res.Menu_Stats_Plus);
                minusBoxes[i].LoadContent(res.Menu_Stats_Minus);

                statIcons.Add(new HoverBox(new Vector2((int)(((creationFrameTex.Width - 40) / statCount) * i + ((Window.resolution.X - creationFrameTex.Width) / 2) + 30), statXOffset + 35)));
            }

            this.hoverTex = res.Menu_Stats_HoverTextBackground;

            statIcons[0].LoadContent(res.Menu_Stats_ATK, "ATK: Attack, the stat \ndetailing raw, physical \npower.");
            statIcons[1].LoadContent(res.Menu_Stats_DEF, "DEF: Defense, the stat \ndetailing defense from both \nphysical and mana-based \nattacks.");
            statIcons[2].LoadContent(res.Menu_Stats_VIT, "VIT: Vitality, the stat detailing \nhealth and connection to the \nphysical self.");
            statIcons[3].LoadContent(res.Menu_Stats_DEX, "DEX: Dexterity, the stat \ndetailing speed and \nevasiveness.");
            statIcons[4].LoadContent(res.Menu_Stats_INT, "INT: Intelligence, the stat \ndetailing general knowledge \nand connection to the \nmental self.");
            statIcons[5].LoadContent(res.Menu_Stats_WIS, "WIS: Wisdom, the stat \ndetailing the connection \nbetween the self and outside \nworld.");
            statIcons[6].LoadContent(res.Menu_Stats_VIR, "VIR: Virtue, the stat detailing \nthe connection to \nsanctioned affinites.");
            statIcons[7].LoadContent(res.Menu_Stats_EVL, "EVL: Evil, the stat detailing \nthe connection to forbidden \naffinities.");

            for(int i = 0; i < statCount; i++) {
                statValues.Add(10);
            }


            nextButton2.LoadContent(res);
            backButton2.LoadContent(res);

            //

            this.font = font;
        }

        public void Update(InputHandler input, StateHandler sh) {
            switch (curStep) {
                case "character":
                    UpdateCharacterCreation(input, sh);
                    break;
                case "stats":
                    UpdateCharStats(input, sh);
                    break;
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.creationFrameTex, new Rectangle((int)Window.resolution.X / 2 - creationFrameTex.Width / 2, (int)Window.resolution.Y / 2 - creationFrameTex.Height / 2, creationFrameTex.Width, creationFrameTex.Height), Color.White);

            switch (curStep) {
                case "character":
                    DrawCharacterCreation(sb);
                    break;
                case "stats":
                    DrawCharStats(sb);
                    break;
            }
        }


        //Below: Specific update and draw methods for each step of the World Creation Process


        private void UpdateCharacterCreation(InputHandler input, StateHandler sh) {
            nameBox.Update(input);
            genderBoxes.Update(input);
            hairstyleList.Update(input);
            manaColorSlider.Update(input);
            skinColorSlider.Update(input);
            hairColorSlider.Update(input);
            shirtColorSlider.Update(input);

            charPrev.Update(genderBoxes.curSelectedID, hairstyleList.curElementID, manaColorSlider.curColor, skinColorSlider.curColor, hairColorSlider.curColor, shirtColorSlider.curColor);

            nextButton1.Update(input);
            backButton1.Update(input);

            if (backButton1.clicked) {
                backButton1.clicked = false;
                ((MenuState)sh.curState).curScreen = "main";
            }

            if (nextButton1.clicked) {
                nextButton1.clicked = false;
                this.curStep = "stats";
            }
        }

        private void UpdateCharStats(InputHandler input, StateHandler sh) {
            nextButton2.Update(input);
            backButton2.Update(input);

            for (int i = 0; i < statCount; i++) {
                plusBoxes[i].Update(input);
                minusBoxes[i].Update(input);

                if (plusBoxes[i].clicked) {
                    plusBoxes[i].clicked = false;
                    if (remainingPoints > 0) {
                        statValues[i]++;
                        remainingPoints--;
                    }
                } else if (minusBoxes[i].clicked) {
                    minusBoxes[i].clicked = false;
                    if(statValues[i] > 10) {
                        statValues[i]--;
                        remainingPoints++;
                    }
                }


                statIcons[i].Update(input);
            }

            if (backButton2.clicked) {
                backButton2.clicked = false;
                this.curStep = "character";
            }

            if (nextButton2.clicked) {
                nextButton2.clicked = false;
                if(remainingPoints == 0) {
                    PlayerSaver.SaveNewGame(new CharData(this.nameBox.text, this.genderBoxes.curSelectedID, this.hairstyleList.curElementID,
                                                     this.manaColorSlider.curColor, this.skinColorSlider.curColor, this.hairColorSlider.curColor, this.shirtColorSlider.curColor),
                                                     this.statValues);

                    sh.setCurState(State.GAME);
                    ((GameState)sh.curState).UpdateSaveData(PlayerSaver.LoadPlayerSaveFile(this.nameBox.text));
                }
            }

        }


        private void DrawCharacterCreation(SpriteBatch sb) {
            Vector2 centPos = new Vector2(Window.resolution.X / 2 - font.MeasureString("Character Creation").X / 2 - 1, ((Window.resolution.Y / 2) - creationFrameTex.Height/2 ) + 10);
            sb.DrawString(font, "Character Creation", new Vector2((int)centPos.X, (int)centPos.Y), Color.White);

            sb.DrawString(font, "Name", new Vector2(Window.resolution.X / 2 - font.MeasureString("Name").X/2 - 4, Window.resolution.Y / 2 + 120), Color.White);
            nameBox.Draw(sb, new Vector2(Window.resolution.X/2 - nameBox.bounds.Width/2, Window.resolution.Y / 2 + 145), font);

            genderBoxes.Draw(sb);

            hairstyleList.Draw(sb);

            manaColorSlider.Draw(sb, font);
            skinColorSlider.Draw(sb, font);
            hairColorSlider.Draw(sb, font);
            shirtColorSlider.Draw(sb, font);

            charPrev.Draw(sb);

            nextButton1.Draw(sb);
            backButton1.Draw(sb);
        }


        private void DrawCharStats(SpriteBatch sb) {
            Vector2 centPos = new Vector2(Window.resolution.X / 2 - font.MeasureString("Stats").X / 2 - 1, ((Window.resolution.Y / 2) - creationFrameTex.Height / 2) + 10);
            sb.DrawString(font, "Stats", centPos.ToPoint().ToVector2(), Color.White);

            centPos = new Vector2(Window.resolution.X / 2 - font.MeasureString("Remaining Points: " + remainingPoints.ToString()).X / 2 - 1, ((Window.resolution.Y / 2) - creationFrameTex.Height / 2) + 10);
            sb.DrawString(font, "Remaining Points: " + remainingPoints.ToString(), new Vector2((int)centPos.X, (int)centPos.Y + 50), Color.MediumVioletRed);


            for (int i = 0; i < statCount; i++) {
                plusBoxes[i].Draw(sb);
                minusBoxes[i].Draw(sb);

                statIcons[i].Draw(sb, this.font);
                sb.DrawString(this.font, statValues[i].ToString(), new Vector2((int)(((creationFrameTex.Width - 40) / statCount) * i + ((Window.resolution.X - creationFrameTex.Width) / 2) + 41), statXOffset - 28), Color.White);


                

            }

            foreach(HoverBox statIcon in statIcons) {
                if (statIcon.hovering) {
                    sb.Draw(hoverTex, new Rectangle(statIcon.bounds.X, statIcon.bounds.Y, hoverTex.Width, hoverTex.Height), Color.White);
                    sb.DrawString(font, statIcon.text, new Vector2((int)statIcon.bounds.X + 8, (int)statIcon.bounds.Y + 8), Color.White);
                }
            }


            nextButton2.Draw(sb);
            backButton2.Draw(sb);

        }



    }
}
