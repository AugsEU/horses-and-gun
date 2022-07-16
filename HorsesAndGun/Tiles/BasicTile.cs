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

        public BasicTile(ContentManager content, string texName = "Tiles/basic_tile")
        {
            mTexture = content.Load<Texture2D>(texName);
        }

        public override Texture2D Draw(DrawInfo drawInfo)
        {
            return mTexture;
        }

        public override void ApplyEffect(Horse horse, TrackManager trackManager)
        {

        }
    }
}
