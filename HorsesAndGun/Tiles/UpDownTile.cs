﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun.Tiles
{
    internal class UpDownTile : BasicTile
    {
        bool mGoUp;

        public UpDownTile(ContentManager content, bool _up) : base(content, GetPlusTex(_up))
        {
            mGoUp = _up;
        }

        static string GetPlusTex(bool up)
        {
            return up ? "Tiles/tile_go_up" : "Tiles/tile_go_down";

            throw new NotImplementedException();
        }

        public override void ApplyEffect(Horse horse, TrackManager trackManager)
        {
            SoundManager.I.PlaySFX(SoundManager.SFXType.TileActivate, 0.75f);
            horse.QueueOrderFront(HorseOrderType.moveTrack, mGoUp ? -1 : 1);
        }
    }
}
