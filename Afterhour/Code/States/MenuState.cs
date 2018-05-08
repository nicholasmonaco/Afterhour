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
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.Menu;

namespace Afterhour.Code.States {
    public class MenuState : State {

        private SpriteFont font;
        private Texture2D mousePointerTex;

        private Texture2D titleTex;

        private KeyboardState keyboardState;
        private MouseState mouseState;

        private Texture2D backgroundTex;
        private Rectangle backgroundSourceRect;
        private Vector2 backgroundVelocity;

        private List<String> screens = new List<String>();
        public String curScreen { get; set; }

        private List<String> options = new List<String>();
        private int curOptionSelectionID;
        private List<Rectangle> optionRects = new List<Rectangle>();

        private InputHandler inputHandler;

        private WorldCreator worldCreator;
        private ContinueMenu continueMenu;


        public MenuState() {
            screens.Add("main");
            screens.Add("creation");
            screens.Add("continue");
            curScreen = screens[0];

            options.Add("New Game"); //0
            options.Add("Continue"); //1
            options.Add("Options"); //2
            options.Add("Exit Game"); //3
            curOptionSelectionID = 0;

            this.worldCreator = new WorldCreator();
            this.continueMenu = new ContinueMenu();

            this.keyboardState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();

            inputHandler = new InputHandler(this.keyboardState, this.mouseState);
        }


        public override void LoadContent(Resources res) {
            this.font = res.Menu_Font;
            this.mousePointerTex = res.Menu_MousePointer;

            this.titleTex = res.Menu_Title;

            this.backgroundTex = res.Menu_Background;
            this.backgroundSourceRect = new Rectangle(0, 0, (int)Window.resolution.X*2, (int)Window.resolution.Y*2);

            int yCoord = 300;
            foreach (String option in this.options) {
                this.optionRects.Add(new Rectangle(35, yCoord, (int)font.MeasureString(option).X, (int)font.MeasureString(option).Y));
                yCoord += 30;
            }

            worldCreator.LoadContent(res, this.font);
            continueMenu.LoadContent(res, this.font);
        }

        public override void Update(StateHandler sh, GameTime gameTime) {
            this.keyboardState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();
            this.inputHandler.UpdateVars(this.keyboardState, this.mouseState);

            UpdateBackground(gameTime);


            switch (curScreen) {
                case "main":
                    UpdateMainOptions(sh, gameTime, this.inputHandler);
                    break;
                case "creation":
                    UpdateCreationScreen(sh, gameTime, this.inputHandler);
                    break;
                case "continue":
                    UpdateContinueScreen(sh, gameTime, this.inputHandler);
                    break;
            }

            if (this.keyboardState.IsKeyDown(Keys.F)) {
                sh.setCurState(State.GAME);
            }
        }

        public override void Draw(SpriteBatch sb, GFXHandler gh, GameTime gameTime) {
            sb.Draw(this.backgroundTex, new Rectangle(0, 0, backgroundTex.Width, backgroundTex.Height), backgroundSourceRect, Color.White);

            switch (curScreen) {
                case "main":
                    DrawMainOptions(sb, gameTime);
                    break;
                case "creation":
                    DrawCreationScreen(sb, gameTime);
                    break;
                case "continue":
                    DrawContinueScreen(sb, gameTime);
                    break;
            }



            sb.Draw(this.mousePointerTex, new Rectangle(this.mouseState.Position.X, this.mouseState.Position.Y, this.mousePointerTex.Width, this.mousePointerTex.Height), Color.White);
        }


        //Below: Specific update and draw methods for parts of the Main Menu state


        private void UpdateMainOptions(StateHandler sh, GameTime gameTime, InputHandler input) {//updates the main menu option words
            //Updates which option is selected
            if (input.keyboardState.IsKeyUp(Keys.W) && input.keyboardState_old.IsKeyDown(Keys.W)) { //Highlights the next option upwards
                if (curOptionSelectionID > 0) {
                    curOptionSelectionID--;
                } else {
                    curOptionSelectionID = options.Count() - 1;
                }
            }

            if (input.keyboardState.IsKeyUp(Keys.S) && input.keyboardState_old.IsKeyDown(Keys.S)) { //Highlights the next option downwards
                if (curOptionSelectionID < options.Count() - 1) {
                    curOptionSelectionID++;
                } else {
                    curOptionSelectionID = 0;
                }
            }
            //

            //Executes command based on which option is selected
            if (input.keyboardState.IsKeyUp(Keys.Space) && input.keyboardState_old.IsKeyDown(Keys.Space) || input.mouseState.LeftButton == ButtonState.Pressed) { //Actually selects the highlighted option
                switch (curOptionSelectionID) {
                    case 0: //New Game
                        //sh.setCurState(State.GAME); //Don't do this yet, do it at the end of the creation screens
                        this.curScreen = screens[1];
                        break;
                    case 1: //Continue
                        this.curScreen = screens[2];
                        break;
                    case 2: //Options
                        sh.setCurState(State.OPTIONS);
                        break;
                    case 3: //Exit Game
                        MainGame.exitGame();
                        break;
                }
            }

            Point mousePos = input.mouseState.Position;
            for(int i=0;i<optionRects.Count();i++) {
                if (optionRects[i].Contains(mousePos)) {
                    curOptionSelectionID = i;
                }
            }
            //
        }

        private void DrawMainOptions(SpriteBatch sb, GameTime gameTime) { //draws the main menu option words
            int yCoord = 300;
            foreach (String option in this.options) {
                if (option != options[curOptionSelectionID]) {
                    sb.DrawString(this.font, option, new Vector2(35, yCoord), Color.White);
                } else {
                    sb.DrawString(this.font, option, new Vector2(35, yCoord), Color.Purple);
                }
                yCoord += 30;
            }

            sb.Draw(this.titleTex, new Rectangle(280, 50, this.titleTex.Width, this.titleTex.Height), Color.White);
        }


        private void UpdateCreationScreen(StateHandler sh, GameTime gameTime, InputHandler input) {
            worldCreator.Update(input, sh);
        }

        private void DrawCreationScreen(SpriteBatch sb, GameTime gameTime) {
            worldCreator.Draw(sb);
        }


        private void DrawContinueScreen(SpriteBatch sb, GameTime gameTime) {
            continueMenu.Draw(sb);
        }

        public void UpdateContinueScreen(StateHandler sh, GameTime gameTime, InputHandler input) {
            continueMenu.Update(input, sh);
        }



        private void UpdateBackground(GameTime gameTime) {
            if(backgroundVelocity.X == 0) {
                Random rand = new Random();
                backgroundVelocity.X = (rand.Next(4) - 2);
            }

            if (backgroundVelocity.Y == 0) {
                Random rand = new Random();
                backgroundVelocity.Y = (rand.Next(4) - 2);
            }



            if (backgroundSourceRect.X + backgroundVelocity.X >= 0 && backgroundSourceRect.X + backgroundVelocity.X <= 1000) {
                backgroundSourceRect.X += (int)(backgroundVelocity.X);
            }else {
                backgroundVelocity.X = 0;
            }
            if (backgroundSourceRect.Y + backgroundVelocity.Y >= 0 && backgroundSourceRect.Y + backgroundVelocity.Y <= 1000) {
                backgroundSourceRect.Y += (int)(backgroundVelocity.Y);
            } else {
                backgroundVelocity.Y = 0;
            }

            //backgroundVelocity.X--;
            //backgroundVelocity.Y--;
        }

    }
}
