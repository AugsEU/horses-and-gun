using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun.Tiles
{
    internal class BasicTile : TrackTile
    {
        Texture2D mTexture;

        public BasicTile(ContentManager content)
        {
            mTexture = content.Load<Texture2D>(TexName());
        }

        protected virtual string TexName()
        {
            return "Tiles/basic_tile";
        }

        public override Texture2D Draw(DrawInfo drawInfo)
        {
            return mTexture;
        }
    }
}
