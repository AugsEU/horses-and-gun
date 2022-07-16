using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal abstract class Entity
    {
        protected Vector2 mPosition;
        protected Texture2D mTexture;

        public Entity(Vector2 pos)
        {
            mPosition = pos;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public abstract void Draw(DrawInfo info);

        public abstract void LoadContent(ContentManager content);

        public abstract Rect2f ColliderBounds();

        public void ShiftPosition(Vector2 shift)
        {
            mPosition += shift;
        }
        public Vector2 position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public virtual void CollideWithEntity(Entity entity)
        {

        }

        public virtual void Kill()
        {

        }

        public virtual bool DeleteMe()
        {
            return false;
        }
    }
}
