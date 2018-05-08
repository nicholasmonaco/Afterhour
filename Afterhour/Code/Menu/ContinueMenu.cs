using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.States;
using Afterhour.Code.Menu.GUI;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.FileHandling;

namespace Afterhour.Code.Menu {
    public class ContinueMenu {

        private String saveDir;

        private int saveFileCount;
        private List<String> saveFilePaths = new List<String>();

        private List<String> saveNames = new List<String>();
        private List<int> saveLevels = new List<int>();


        private SpriteFont font;

        private Texture2D frameTex;
        private Rectangle frameRect;

        private Texture2D saveBarTex;

        private Texture2D playButtonTex;
        private Texture2D deleteButtonTex;
        private List<ContFuncButton> playButtons = new List<ContFuncButton>();
        private List<ContFuncButton> deleteButtons = new List<ContFuncButton>();

        private NextButton backButton;

        ///
        public ContinueMenu() {

        }


        public void LoadContent(Resources res, SpriteFont font) {
            String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split(new List<char>() { '\\' }.ToArray(), 2)[1];
            //PLACEHOLDER
            //userName = "Nick"; //put in a thing that just rips this part away from the EMP/ part
            //PLACEHOLDER
            String saveDir = "C:/Users/" + userName + "/Documents/My Games/Afterhour/";
            this.saveDir = saveDir;

            if (Directory.Exists(saveDir)) {
                this.saveFileCount = Directory.GetFiles(saveDir).Length;
                this.saveFilePaths = Directory.GetFiles(saveDir).ToList();
            } else {
                this.saveFileCount = 0;
            }

            getSaveData(saveDir);

            


            //Texture loading
            this.font = font;
            this.frameTex = res.Menu_Continue_Frame;

            this.frameRect = new Rectangle((int)(Window.resolution.X / 2 - frameTex.Width / 2), (int)(Window.resolution.Y / 2 - frameTex.Height / 2), frameTex.Width, frameTex.Height);

            this.saveBarTex = res.Menu_Continue_SaveBar;

            this.playButtonTex = res.Menu_Continue_FuncButton_Play;
            this.deleteButtonTex = res.Menu_Continue_FuncButton_Delete;

            //Post-Texture Loading

            for (int i = 0; i < saveFileCount; i++) {
                this.playButtons.Add(new ContFuncButton(ContFuncButton.PLAY, new Vector2(this.frameRect.X + 20, this.frameRect.Y + 105 + ((this.saveBarTex.Height + 5) * i))));
                this.deleteButtons.Add(new ContFuncButton(ContFuncButton.DELETE, new Vector2(this.frameRect.X + 40, this.frameRect.Y + 105 + ((this.saveBarTex.Height + 5) * i))));

                this.playButtons[i].LoadContent(playButtonTex);
                this.deleteButtons[i].LoadContent(deleteButtonTex);
            }

            backButton = new NextButton(new Vector2(15, 525), "Back");
            backButton.LoadContent(res);
        }

        public void Update(InputHandler input, StateHandler sh) {
            for (int i = 0; i < saveFileCount; i++) {
                this.playButtons[i].Update(input);
                this.deleteButtons[i].Update(input);

                if (deleteButtons[i].clicked) {
                    this.deleteButtons[i].clicked = false;
                    File.Delete(this.saveDir + saveNames[i] + ".ah");
                    this.Reload();
                }

                if (playButtons[i].clicked) {
                    this.playButtons[i].clicked = false;
                    sh.setCurState(State.GAME);
                    ((GameState)sh.curState).UpdateSaveData(PlayerSaver.LoadPlayerSaveFile(saveNames[i]));
                }
            }

            backButton.Update(input);

            if (backButton.clicked) {
                backButton.clicked = false;
                ((MenuState)sh.curState).curScreen = "main";
            }
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(this.frameTex, this.frameRect, Color.White);
            sb.DrawString(this.font, "Continue Save", new Vector2((int)(Window.resolution.X/2 - font.MeasureString("Continue Save").X/2), 30), Color.White);

            for(int i = 0; i < saveFileCount; i++) {
                sb.Draw(this.saveBarTex, new Rectangle(this.frameRect.X + 10, this.frameRect.Y + 10 + ((this.saveBarTex.Height + 5)*i), this.saveBarTex.Width, this.saveBarTex.Height), Color.White);
                sb.DrawString(this.font, saveNames[i] + "\nLevel " + saveLevels[i], new Vector2((int)(this.frameRect.X + 20), (int)(this.frameRect.Y + 20 + ((this.saveBarTex.Height + 5) * i))), Color.White);

                this.playButtons[i].Draw(sb);
                this.deleteButtons[i].Draw(sb);
            }

            backButton.Draw(sb);
        }




        private void getSaveData(String dir) {
            for(int i = 0; i < this.saveFileCount; i++) {
                this.saveNames.Add(saveFilePaths[i].Split(new String[] { "My Games/Afterhour/" }, StringSplitOptions.None)[1].Split(new char[] { '.' })[0]);
                
                SaveData tempData = PlayerSaver.LoadPlayerSaveFile(saveNames[i]);
                this.saveLevels.Add(tempData.worldData.curLevel);

                //System.Diagnostics.Debug.WriteLine("Name " + (i+1) + ": " + saveNames[i] + "  Level: " + saveLevels[i]);
            }
        }


        private void Reload() {
            if (Directory.Exists(this.saveDir)) {
                this.saveFileCount = Directory.GetFiles(saveDir).Length;
                this.saveFilePaths = Directory.GetFiles(saveDir).ToList();
            } else {
                this.saveFileCount = 0;
            }

            this.saveNames.Clear();
            this.saveLevels.Clear();

            getSaveData(saveDir);

            for (int i = 0; i < saveFileCount; i++) {
                this.playButtons.Add(new ContFuncButton(ContFuncButton.PLAY, new Vector2(this.frameRect.X + 20, this.frameRect.Y + 105 + ((this.saveBarTex.Height + 5) * i))));
                this.deleteButtons.Add(new ContFuncButton(ContFuncButton.DELETE, new Vector2(this.frameRect.X + 40, this.frameRect.Y + 105 + ((this.saveBarTex.Height + 5) * i))));

                this.playButtons[i].LoadContent(this.playButtonTex);
                this.deleteButtons[i].LoadContent(this.deleteButtonTex);
            }
        }



    }
}
