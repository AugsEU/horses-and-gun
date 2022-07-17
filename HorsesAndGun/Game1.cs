using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HorsesAndGun
{
    public class Game1 : Game
    {
        public static Rectangle sRenderTargetRect = new Rectangle(0,0,1,1);
        public static Game1 self;

        private const double FRAME_RATE = 60d;
        private const int MIN_HEIGHT = Screen.SCREEN_HEIGHT;
        private const int MIN_WIDTH = Screen.SCREEN_WIDTH;
        private const float ASPECT_RATIO = 1.77778f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            self = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Fix to 60 fps.
            IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromSeconds(1d / FRAME_RATE);

            Window.ClientSizeChanged += OnResize;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Screen.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = Screen.SCREEN_HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;

            base.Initialize();
        }

        private void OnResize(object sender, EventArgs eventArgs)
        {
            if (_graphics.IsFullScreen)
            {
                return;
            }

            int newWidth = Math.Max(MIN_WIDTH, Window.ClientBounds.Width);
            int newHeight = Math.Max(MIN_HEIGHT, Window.ClientBounds.Height);

            _graphics.PreferredBackBufferWidth = newWidth;
            _graphics.PreferredBackBufferHeight = newHeight;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundManager.I.LoadContent(this.Content);
            ScreenManager.I.LoadAllScreens(this.Content, _graphics);
            FontManager.I.LoadAllFonts(this.Content);

            //

            // ROSS BUTTON SCREEN FIRST FOR TESTING //

            ScreenManager.I.ActivateScreen(ScreenType.RossButtonsScreen);

            //

            //ScreenManager.I.ActivateScreen(ScreenType.Intro);
        }

        protected override void Update(GameTime gameTime)
        {
            if(IsActive == false)
            {
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            InputManager.I.Update(gameTime);

            ScreenManager.I.Update(gameTime);
            TimeManager.I.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawInfo frameInfo;

            frameInfo.graphics = _graphics;
            frameInfo.spriteBatch = _spriteBatch;
            frameInfo.gameTime = gameTime;
            frameInfo.device = GraphicsDevice;

            //Draw active screen.
            Screen screen = ScreenManager.I.GetActiveScreen();

            if (screen != null)
            {
                RenderTarget2D screenTargetRef = screen.DrawToRenderTarget(frameInfo);

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(SpriteSortMode.Immediate,
                                    BlendState.AlphaBlend,
                                    SamplerState.PointClamp,
                                    DepthStencilState.None,
                                    RasterizerState.CullNone);
                DrawScreenPixelPerfect(frameInfo, screenTargetRef);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void DrawScreenPixelPerfect(DrawInfo info, RenderTarget2D screen)
        {
            Rectangle screenRect = info.device.PresentationParameters.Bounds;

            int multiplier = (int)MathF.Min(screenRect.Width / screen.Width, screenRect.Height / screen.Height);

            int finalWidth = screen.Width * multiplier;
            int finalHeight = screen.Height * multiplier;

            Rectangle destRect = new Rectangle((screenRect.Width - finalWidth) / 2, (screenRect.Height - finalHeight) / 2, finalWidth, finalHeight);

            sRenderTargetRect.Location = destRect.Location;
            sRenderTargetRect.Width = multiplier;
            sRenderTargetRect.Height = multiplier;

            info.spriteBatch.Draw(screen, destRect, Color.White);
        }


    }
}
