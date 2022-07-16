using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class MovingDie : Entity
    {
        const float SCALE = 0.2f;
        const float ROT_SPEED = 0.75f;

        float mRotation;
        Vector2 mSpeed;

        public MovingDie(Vector2 _pos, Vector2 _speed, Texture2D _dice_tex) : base(_pos)
        {
            mTexture = _dice_tex;
            mRotation = 0.0f;
            mSpeed = _speed;
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override Rect2f ColliderBounds()
        {
            return new Rect2f(mPosition, mPosition + new Vector2(mTexture.Width * SCALE, mTexture.Height * SCALE));
        }

        public override void Draw(DrawInfo info)
        {
            info.spriteBatch.Draw(mTexture, mPosition, null, Color.White, mRotation, new Vector2(mTexture.Width / 2.0f, mTexture.Height / 2.0f), new Vector2(SCALE, SCALE), SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime)
        {
            mPosition += mSpeed * Util.GetDeltaT(gameTime);
            mRotation += ROT_SPEED * Util.GetDeltaT(gameTime);
        }

        public override bool DeleteMe()
        {
            return mPosition.X > Screen.SCREEN_WIDTH + 10.0f;
        }
    }
}
