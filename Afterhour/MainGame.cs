using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Afterhour.Code.Handling;
using Afterhour.Code.Handling.AssetHandling;
using Afterhour.Code.Handling.GFXHandling;
using Afterhour.Code.States;
using Afterhour.Code.Game.Scenes.Overworld.Entities;

namespace Afterhour {

    public class MainGame : Game { 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static bool exited = false;

        public Resources resources;
        public StateHandler stateHandler;
        public GFXHandler gfxHandler;

        private List<String> titleExtentions = new List<String>();


        public MainGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            titleExtentions.Add("Hidden Swords!");
            titleExtentions.Add("Unlimited Power!");
            titleExtentions.Add("Hackerman!");
            titleExtentions.Add("Fruit Baskets!");
            titleExtentions.Add("Save the Princess!");
            titleExtentions.Add("Look Under the Truck!");
            titleExtentions.Add("Magic!");
            titleExtentions.Add("Alt + F4!");

            Random rand = new Random();
            int randIndex = rand.Next(titleExtentions.Count);

            //Afterhour Constructing

            Window.Title = "Afterhour: " + titleExtentions[randIndex];
            Code.Handling.Window.resolution = new Vector2(800, 600); //eventually load this from an options file
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            //
        }


        protected override void Initialize() { //I might honestly not use this at all
            //At this point, LoadContent has not yet been called.
            base.Initialize();
            //At this point, LoadContent has been called.
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Afterhour LoadContent
            this.resources = new Resources(Content);
            this.stateHandler = new StateHandler(this.resources, State.MENU);
            this.gfxHandler = new GFXHandler();


            //
        }

        protected override void UnloadContent() {
            //Afterhour UnloadContent
            //
        }

        protected override void Update(GameTime gameTime) {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) { //Exits the game
                Exit();
            }*/

            //Afterhour Updating
            this.stateHandler.curState.Update(this.stateHandler, gameTime);
            this.gfxHandler.graphicsDeviceManager = this.graphics;


            if(exited == true) {
                Exit();
            }
            //
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            //Afterhour Drawing
            //spriteBatch.Begin(SpriteSortMode.Immediate); //Original
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            this.stateHandler.curState.Draw(spriteBatch, gfxHandler, gameTime);

            spriteBatch.End();
            //
            base.Draw(gameTime);
        }



        public static void exitGame() {
            exited = true;
        }
    }
}
