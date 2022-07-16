using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class Horse : Entity
    {
        int mTileIndex;
        int mTrackIndex;
        Animator mIdleAnim;

        public Horse(Vector2 _pos, int _tileIndex, int _trackIndex) : base(_pos)
        {
            mIdleAnim = new Animator(Animator.PlayType.Loop);

            mTileIndex = _tileIndex;
            mTrackIndex = _trackIndex;
        }

        public override void LoadContent(ContentManager content)
        {
            mTexture = content.Load<Texture2D>("Horse/horse-stand1");

            mIdleAnim.LoadFrame(content, "Horse/horse-stand1", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand2", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.Play();
        }

        public override Rect2f ColliderBounds()
        {
            return new Rect2f(mPosition, mPosition + new Vector2(mTexture.Width, mTexture.Height));
        }

        public override void Draw(DrawInfo info)
        {
            Texture2D texture = mIdleAnim.GetCurrentTexture();

            info.spriteBatch.Draw(texture, mPosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            mIdleAnim.Update(gameTime);
        }

        public int TrackIndex
        {
            get { return mTrackIndex; }
            set { mTrackIndex = value; }
        }

        public int TileIndex
        {
            set { mTileIndex = value; }
            get { return mTileIndex; }
        }
    }
}
