using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class EndGameScreen : Screen
    {
        public EndGameScreen(ContentManager content, GraphicsDeviceManager graphics) : base(content, graphics)
        {
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void OnActivate()
        {

        }

        public override void OnDeactivate()
        {

        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
