using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal abstract class TrackTile
    {
        public abstract Texture2D Draw(DrawInfo drawInfo);

        public virtual void Update(GameTime gameTime) { }
    }
}
