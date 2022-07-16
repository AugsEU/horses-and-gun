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
        const int NUM_LANES = 5;

        //Textures
        Texture2D mBackground;
        Texture2D[] mDiceTextures;

        int mGunLane;
        Texture2D mGunTexture;

        //Dice
        DiceQueue mDiceQueue;
        
        //Tiles
        TrackManager mTrackManager;


        public MainGameScreen(ContentManager content, GraphicsDeviceManager graphics) : base(content, graphics)
        {
            mDiceQueue = new DiceQueue();
            mDiceTextures = new Texture2D[6];
            mTrackManager = new TrackManager(content);
            mGunLane = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            mBackground = content.Load<Texture2D>("main_bg");
            mGunTexture = content.Load<Texture2D>("gun");

            mDiceTextures[0] = content.Load<Texture2D>("Dice1");
            mDiceTextures[1] = content.Load<Texture2D>("Dice2");
            mDiceTextures[2] = content.Load<Texture2D>("Dice3");
            mDiceTextures[3] = content.Load<Texture2D>("Dice4");
            mDiceTextures[4] = content.Load<Texture2D>("Dice5");
            mDiceTextures[5] = content.Load<Texture2D>("Dice6");
        }

        public override void OnActivate()
        {
            mDiceQueue = new DiceQueue();
            mGunLane = 0;

            mTrackManager.Init();
        }

        public override void OnDeactivate()
        {

        }

        public override void Update(GameTime gameTime)
        {
            EntityManager.I.Update(gameTime);
            mTrackManager.Update(gameTime);

            HandleInput(gameTime);

            DecideGunPosition();
        }

        private void HandleInput(GameTime gameTime)
        {
            if (InputManager.I.LClick())
            {
                FireGun(gameTime);
            }
        }

        private void FireGun(GameTime gameTime)
        {
            Dice diceToShoot = mDiceQueue.PopDice();

            Texture2D diceTex = GetDiceTexture(diceToShoot);

            Vector2 speed = new Vector2(20.0f, 0.0f);
            Vector2 startPos = new Vector2(65.0f, 180.0f);

            startPos.Y = mGunLane * 50.0f + 43.0f;

            EntityManager.I.RegisterEntity(new MovingDie(startPos, speed, diceTex), mContentManager);
        }

        public void DecideGunPosition()
        {
            Point mousePos = InputManager.I.GetMousePos();

            mGunLane = Math.Max(0, (mousePos.Y - 29) / 50);

            mGunLane = Math.Min(mGunLane, NUM_LANES - 1);
        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(Color.SaddleBrown);

            info.spriteBatch.Begin(SpriteSortMode.Immediate,
                                    BlendState.AlphaBlend,
                                    SamplerState.PointClamp,
                                    DepthStencilState.None,
                                    RasterizerState.CullNone);

            info.spriteBatch.Draw(mBackground, Vector2.Zero, Color.White);

            mTrackManager.Draw(info);
            EntityManager.I.Draw(info);


            DrawDice(info);
            DrawGun(info);
            
            info.spriteBatch.End();

            return mScreenTarget;
        }

        private void DrawGun(DrawInfo info)
        {
            Vector2 startPoint = new Vector2(0.0f, 29.0f);
            Vector2 spacing = new Vector2(0.0f, 50.0f);

            startPoint += spacing * mGunLane;

            info.spriteBatch.Draw(mGunTexture, startPoint, Color.White);
        }

        private void DrawDice(DrawInfo info)
        {
            Vector2 startPoint = new Vector2(9.0f, 287.0f);

            //Speical dice
            Texture2D texture = GetDiceTexture(mDiceQueue.PeekDice(0));
            info.spriteBatch.Draw(texture, startPoint, Color.White);


            startPoint = new Vector2(110.5f, 287.0f);
            Vector2 spacing =new Vector2(73.0f,0.0f);

            for(int i = 1; i < mDiceQueue.GetDiceNum(); i++)
            {
                texture = GetDiceTexture(mDiceQueue.PeekDice(i));

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
