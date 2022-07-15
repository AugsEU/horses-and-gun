using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class MainGameScreen : Screen
    {
        Texture2D mBackground;
        Texture2D[] mDiceTextures;

        DiceQueue mDiceQueue;

        public MainGameScreen(ContentManager content, GraphicsDeviceManager graphics) : base(content, graphics)
        {
            mDiceQueue = new DiceQueue();
            mDiceTextures = new Texture2D[6];
        }

        public override void LoadContent(ContentManager content)
        {
            mBackground = content.Load<Texture2D>("main_bg");

            mDiceTextures[0] = content.Load<Texture2D>("Dice1");
            mDiceTextures[1] = content.Load<Texture2D>("Dice2");
            mDiceTextures[2] = content.Load<Texture2D>("Dice3");
            mDiceTextures[3] = content.Load<Texture2D>("Dice4");
            mDiceTextures[4] = content.Load<Texture2D>("Dice5");
            mDiceTextures[5] = content.Load<Texture2D>("Dice6");
        }

        public override void OnActivate()
        {

        }

        public override void OnDeactivate()
        {

        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            SpriteFont pixelFont = FontManager.I.GetFont("Pixica-24");

            Vector2 centre = new Vector2(mScreenTarget.Width / 2, mScreenTarget.Height / 2);

            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(Color.SaddleBrown);

            info.spriteBatch.Begin(SpriteSortMode.Immediate,
                                    BlendState.AlphaBlend,
                                    SamplerState.PointClamp,
                                    DepthStencilState.None,
                                    RasterizerState.CullNone);

            info.spriteBatch.Draw(mBackground, Vector2.Zero, Color.White);

            DrawDice(info);


            
            info.spriteBatch.End();

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
            if(InputManager.I.KeyPressed(Keys.A))
            {
                mDiceQueue.PopDice();
            }
        }

        private void DrawDice(DrawInfo info)
        {
            Vector2 startPoint = new Vector2(30.0f, 30.0f);
            Vector2 spacing =new Vector2(70.0f,0.0f);

            for(int i = 0; i < mDiceQueue.GetDiceNum(); i++)
            {
                Texture2D texture = GetDiceTexture(mDiceQueue.PeekDice(i));

                info.spriteBatch.Draw(texture, startPoint, Color.White);

                startPoint += spacing;
            }
        }

        Texture2D GetDiceTexture(Dice die)
        {
            return GetDiceTexture(die.Value);
        }

        Texture2D GetDiceTexture(int value)
        {
            return mDiceTextures[value - 1];
        }

    }
}
