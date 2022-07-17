﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun.Tiles
{
    internal class PlusTile : BasicTile
    {
        int mMoveAmount;

        public PlusTile(ContentManager content, int _amount) : base(content, GetPlusTex(_amount))
        {
            mMoveAmount = _amount;
        }

        static string GetPlusTex(int amount)
        {
            switch(amount)
            {
                case 1: return "Tiles/plus_one_tile";
                case 2: return "Tiles/plus_two_tile";
                case 3: return "Tiles/plus_three_tile";
            }

            throw new NotImplementedException();
        }

        public override void ApplyEffect(Horse horse, TrackManager trackManager)
        {
            SoundManager.I.PlaySFX(SoundManager.SFXType.TileActivate, 0.75f);

            horse.QueueOrderFront(HorseOrderType.moveTile, mMoveAmount);
        }
    }
}
