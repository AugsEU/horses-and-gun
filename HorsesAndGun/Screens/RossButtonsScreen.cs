using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class RossButtonsScreen : Screen
    {
        private Game1 game;
        private List<Button> mButtonList;

        private void PlayButtonClick(object sender, EventArgs e)
        {
            ScreenManager.I.ActivateScreen(ScreenType.MainGame);
        }

        private void OptionsButtonClick(object sender, EventArgs e)
        {
            ScreenManager.I.ActivateScreen(ScreenType.Options);
        }

        private void QuitButtonClick(object sender, EventArgs e)
        {
            game.Exit();
        }

        public RossButtonsScreen(ContentManager content, GraphicsDeviceManager graphics) : base(content, graphics)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            Button playButton = new Button(content.Load<Texture2D>("temp_button"), content.Load<SpriteFont>("Fonts/Pixica"))
            {
                mPosition = new Vector2(170, 80),
                mText = "Play"
            };

            Button optionsButton = new Button(content.Load<Texture2D>("temp_button"), content.Load<SpriteFont>("Fonts/Pixica"))
            {
                mPosition = new Vector2(170, 140),
                mText = "Options"
            };

            Button quitButton = new Button(content.Load<Texture2D>("temp_button"), content.Load<SpriteFont>("Fonts/Pixica"))
            {
                mPosition = new Vector2(170, 200),
                mText = "Quit"
            };


            playButton.mOnMouseClick += PlayButtonClick;
            optionsButton.mOnMouseClick += OptionsButtonClick;
            quitButton.mOnMouseClick += QuitButtonClick;


            mButtonList = new List<Button>()
            {
                playButton,
                optionsButton,
                quitButton
            };
        }

        public override void OnActivate()
        {
        }

        public override void OnDeactivate()
        {
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

            foreach (Button button in mButtonList)
            {
                button.Draw(info);
            }

            info.spriteBatch.End();

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button button in mButtonList)
            {
                button.Update();
            }
        }

    }
}
