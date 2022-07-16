using System;
using System.Collections.Generic;
using System.Text;
using HorsesAndGun.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class TrackManager
    {
        const int NUM_TRACKS = 5;
        const int NUM_TILES_PER_TRACK = 11;

        Vector2 TRACK_ORIGIN = new Vector2(83.0f, 29.0f);
        Vector2 TRACK_OFFSET = new Vector2(0.0f, 50.0f);
        Vector2 PADDING = new Vector2(4.0f, 0.0f);
        Vector2 TRACK_PADDING = new Vector2(0.0f, 19.0f);
        Vector2 TILE_OFFSET = new Vector2(50.0f, 0.0f);

        const double START_FALL_TIME = 3000.0;

        TrackTile[,] mTiles;
        ContentManager mContentManager;

        double mFallTime = 3000.0;
        MonoTimer mFallingTimer;

        public TrackManager(ContentManager _ContentManager)
        {
            mTiles = new TrackTile[NUM_TRACKS, NUM_TILES_PER_TRACK];
            mContentManager = _ContentManager;

            mFallingTimer = new MonoTimer();
        }

        public void Init()
        {
            mFallTime = START_FALL_TIME;
            mFallingTimer.Reset();

            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    mTiles[track, tile] = new BasicTile(mContentManager);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for(int track = 0; track < NUM_TRACKS; track++)
            {
                for(int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    mTiles[track, tile].Update(gameTime);
                }
            }
        }

        public void Draw(DrawInfo info)
        {
            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    Texture2D tileTex = mTiles[track, tile].Draw(info);

                    Vector2 position = TRACK_ORIGIN + PADDING + TRACK_PADDING + track * TRACK_OFFSET + tile * TILE_OFFSET;

                    info.spriteBatch.Draw(tileTex, position, Color.White);
                }
            }
        }

    }
}
