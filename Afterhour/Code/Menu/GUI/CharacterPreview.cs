using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Afterhour.Code.Handling.AssetHandling;

namespace Afterhour.Code.Menu {
    public class CharacterPreview {

        private Vector2 pos;

        private int gender;
        private int hairstyleID;
        private Color manaColor;
        private Color skinColor;
        private Color hairColor;
        private Color shirtColor;

        private Texture2D frameTex;
        private Texture2D manaBaseTex;
        private Texture2D skinBaseTex;
        private Texture2D bottomTex;
        private Texture2D shirtBaseTex;

        private List<Texture2D> hairstyles = new List<Texture2D>();



        public CharacterPreview(Vector2 pos) {
            this.pos = pos;
        }

        public void LoadContent(Resources res, int gender, int hairstyleID, Color manaColor, Color skinColor, Color hairColor, Color shirtColor) {
            this.frameTex = res.Menu_CharPrev_Frame;

            this.manaBaseTex = res.Menu_CharCreation_ManaGlow;
            this.skinBaseTex = res.Menu_CharCreation_Skin;
            this.bottomTex = res.Menu_CharCreation_Bottom;
            this.shirtBaseTex = res.Menu_CharCreation_Shirt;

            this.hairstyles.Add(res.Menu_CharCreation_Hair_1);

            //Variable Setting
            this.gender = gender;
            this.hairstyleID = hairstyleID;
            this.manaColor = manaColor;
            this.skinColor = skinColor;
            this.hairColor = hairColor;
            this.shirtColor = shirtColor;
        }

        public void Update(int gender, int hairstyleID, Color manaColor, Color skinColor, Color hairColor, Color shirtColor) {
            this.gender = gender;
            this.hairstyleID = hairstyleID;
            this.manaColor = manaColor;
            this.skinColor = skinColor;
            this.hairColor = hairColor;
            this.shirtColor = shirtColor;
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(frameTex, new Rectangle((int)this.pos.X, (int)this.pos.Y, frameTex.Width, frameTex.Height), Color.White);

            //Render order:
            sb.Draw(this.manaBaseTex, new Rectangle((int)this.pos.X+5, (int)this.pos.Y, this.manaBaseTex.Width, this.manaBaseTex.Height), manaColor);
            sb.Draw(this.skinBaseTex, new Rectangle((int)this.pos.X+9, (int)this.pos.Y+8, skinBaseTex.Width, skinBaseTex.Height), skinColor);
            sb.Draw(this.shirtBaseTex, new Rectangle((int)this.pos.X+9, (int)this.pos.Y + 8, shirtBaseTex.Width, shirtBaseTex.Height), shirtColor);
            sb.Draw(this.bottomTex, new Rectangle((int)this.pos.X+9, (int)this.pos.Y+8, this.bottomTex.Width, this.bottomTex.Height), Color.White);


            sb.Draw(hairstyles[hairstyleID], new Rectangle((int)this.pos.X + 9, (int)this.pos.Y + 4, 32, 32), hairColor);
        }

    }
}
