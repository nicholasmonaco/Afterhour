using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Afterhour.Code.Handling.GFXHandling {
    public class TextureMasker {

        public Texture2D backTex { get; }
        public Texture2D maskTex { get; }



        public TextureMasker(Texture2D backTex, Texture2D maskTex) {
            this.backTex = backTex;
            this.maskTex = maskTex;
        }


        public void Draw(GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, Vector2 backTexPos) {
            DepthFormat oldDepthFormat = gdm.PreferredDepthStencilFormat;

            gdm.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

            var m = Matrix.CreateOrthographicOffCenter(0,
                                                       gdm.GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                       gdm.GraphicsDevice.PresentationParameters.BackBufferHeight,
                                                       0, 0, 1);

            var a = new AlphaTestEffect(gdm.GraphicsDevice) {
                Projection = m
            };


            var stencil1 = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            var stencil2 = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            

            sb.End();

            sb.Begin(SpriteSortMode.Immediate, null, null, stencil1, null, a); //This is the mask shape
            sb.Draw(this.maskTex, pos, Color.White);
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            sb.Draw(this.maskTex, pos, Color.Black);
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, null, null, stencil2, null, a); //This is the mask filling
            sb.Draw(backTex, backTexPos, Color.White);
            sb.End();

            gdm.PreferredDepthStencilFormat = oldDepthFormat;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }


    }
}
