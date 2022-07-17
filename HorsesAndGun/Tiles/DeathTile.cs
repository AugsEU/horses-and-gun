using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun.Tiles
{
    internal class DeathTile : BasicTile
    {
        public DeathTile(ContentManager content) : base(content, "Tiles/tile_death")
        {

        }
        public override void ApplyEffect(Horse horse, TrackManager trackManager)
        {
            horse.Kill();
        }
    }
}
