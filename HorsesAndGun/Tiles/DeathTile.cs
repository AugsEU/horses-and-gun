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
        int mMoveAmount;

        public DeathTile(ContentManager content, int _amount) : base(content, )
        {
            mMoveAmount = _amount;
        }
        public override void ApplyEffect(Horse horse, TrackManager trackManager)
        {
            SoundManager.I.PlaySFX(SoundManager.SFXType.TileActivate, 0.75f);
            horse.Kill();
        }
    }
}
