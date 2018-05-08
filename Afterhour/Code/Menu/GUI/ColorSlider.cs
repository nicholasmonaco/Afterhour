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

namespace Afterhour.Code.Menu {
    public class ColorSlider {

        private Texture2D PointerTex;
        private Texture2D BaseTex;

        private Texture2D Htex;
        private Texture2D Stex;
        private Texture2D Vtex;


        private int HValue = 0;
        private double SValue = 1f;
        private double VValue = 1f;

        private String title;


        private Color curBaseColor = Color.White;
        public Color curColor { get; set; }


        private bool HStuck = false;
        private bool SStuck = false;
        private bool VStuck = false;

        private Vector2 pos;

        private Rectangle HpointerBounds;
        private Rectangle SpointerBounds;
        private Rectangle VpointerBounds;



        public ColorSlider(Vector2 pos, String title) {
            this.pos = pos;
            this.title = title;
        }


        public void LoadContent(Resources res) {
            this.PointerTex = res.Menu_HSVSlider_Pointer;
            this.BaseTex = res.Menu_HSVSlider_Base;
            this.Htex = res.Menu_HSVSlider_H;
            this.Stex = res.Menu_HSVSlider_S;
            this.Vtex = res.Menu_HSVSlider_V;

            this.HpointerBounds = new Rectangle((int)this.pos.X, (int)this.pos.Y - 5, this.PointerTex.Width, this.PointerTex.Height);
            this.SpointerBounds = new Rectangle((int)this.pos.X, (int)this.pos.Y + 10, this.PointerTex.Width, this.PointerTex.Height);
            this.VpointerBounds = new Rectangle((int)this.pos.X + 99, (int)this.pos.Y + 25, this.PointerTex.Width, this.PointerTex.Height);

            HValue = (int)Math.Round((360.0 / 99.0) * (HpointerBounds.X + 4 - (this.pos.X + 4)));
            SValue = (1.0 / 99.0) * (SpointerBounds.X + 4 - (this.pos.X + 4));
            VValue = (1.0 / 99.0) * (VpointerBounds.X + 4 - (this.pos.X + 4));
            curBaseColor = ColorFromHSV(HValue, SValue, VValue);
        }

        public void Update(InputHandler input) {
            if (input.mouseState.LeftButton == ButtonState.Pressed) {
                if (this.HpointerBounds.Contains(input.mouseState.Position)) {
                    HStuck = true;
                    SStuck = false;
                    VStuck = false;
                }
                if (this.SpointerBounds.Contains(input.mouseState.Position)) {
                    SStuck = true;
                    HStuck = false;
                    VStuck = false;
                }
                if (this.VpointerBounds.Contains(input.mouseState.Position)) {
                    VStuck = true;
                    HStuck = false;
                    SStuck = false;
                }


                if (HStuck) {
                    HpointerBounds.X = MathHelper.Clamp(input.mouseState.Position.X - 4, (int)this.pos.X, (int)(this.pos.X + this.BaseTex.Width)-1);
                    HValue = (int)Math.Round((360.0 / 99.0) * (HpointerBounds.X + 4 - (this.pos.X + 4)));
                    curBaseColor = ColorFromHSV(HValue, SValue, VValue);
                }
                if (SStuck) {
                    SpointerBounds.X = MathHelper.Clamp(input.mouseState.Position.X - 4, (int)this.pos.X, (int)(this.pos.X + this.BaseTex.Width)-1);
                    SValue = (1.0 / 99.0) * (SpointerBounds.X + 4 - (this.pos.X + 4));
                    curBaseColor = ColorFromHSV(HValue, SValue, VValue);
                }
                if (VStuck) {
                    VpointerBounds.X = MathHelper.Clamp(input.mouseState.Position.X - 4, (int)this.pos.X, (int)(this.pos.X + this.BaseTex.Width)-1);
                    VValue = (1.0 / 99.0) * (VpointerBounds.X + 4 - (this.pos.X + 4));
                    curBaseColor = ColorFromHSV(HValue, SValue, VValue);
                }
            }else {
                HStuck = false;
                SStuck = false;
                VStuck = false;
            }

            curColor = curBaseColor;
        }


        public void Draw(SpriteBatch sb, SpriteFont font) {
            sb.Draw(this.Htex, new Rectangle((int)this.pos.X + 4, (int)this.pos.Y, this.Htex.Width, this.Htex.Height), Color.White);
            sb.Draw(this.BaseTex, new Rectangle((int)this.pos.X + 4, (int)this.pos.Y + 15, this.BaseTex.Width, this.BaseTex.Height), ColorFromHSV(HValue, 1f, 1f));
            sb.Draw(this.BaseTex, new Rectangle((int)this.pos.X + 4, (int)this.pos.Y + 30, this.BaseTex.Width, this.BaseTex.Height), ColorFromHSV(HValue, SValue, 1f));

            sb.Draw(this.Stex, new Rectangle((int)this.pos.X + 4, (int)this.pos.Y+15, this.Stex.Width, this.Stex.Height), Color.White);
            sb.Draw(this.Vtex, new Rectangle((int)this.pos.X + 4, (int)this.pos.Y+30, this.Vtex.Width, this.Vtex.Height), Color.White);

            sb.Draw(this.PointerTex, HpointerBounds, Color.White);
            sb.Draw(this.PointerTex, SpointerBounds, Color.White);
            sb.Draw(this.PointerTex, VpointerBounds, Color.White);

            sb.DrawString(font, this.title, new Vector2((int)(this.pos.X + 4 + this.BaseTex.Width/2 - (font.MeasureString(this.title).X/2)), (int) (this.pos.Y - 25)), Color.White);
        }





        public Color ColorFromHSV(double hue, double saturation, double value) {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Color(v, t, p, 255);
            else if (hi == 1)
                return new Color(q, v, p, 255);
            else if (hi == 2)
                return new Color(p, v, t, 255);
            else if (hi == 3)
                return new Color(p, q, v, 255);
            else if (hi == 4)
                return new Color(t, p, v, 255);
            else
                return new Color(v, p, q, 255);
        }

    }
}
