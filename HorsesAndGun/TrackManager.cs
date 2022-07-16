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

        Vector2 mDynamicOffset = new Vector2(0.0f, 0.0f);

        const double START_FALL_TIME = 3000.0;

        TrackTile[,] mTiles;
        ContentManager mContentManager;

        double mFallTime = 3000.0;
        MonoTimer mFallingTimer;

        double mShiftTime = 3000.0;
        MonoTimer mShiftTimer;

        public TrackManager(ContentManager _ContentManager)
        {
            mTiles = new TrackTile[NUM_TRACKS, NUM_TILES_PER_TRACK];
            mContentManager = _ContentManager;

            mFallingTimer = new MonoTimer();
            mShiftTimer = new MonoTimer();
        }

        public void Init()
        {
            mFallTime = START_FALL_TIME;
            mFallingTimer.FullReset();
            mShiftTimer.FullReset();

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
            if(mFallingTimer.IsPlaying())
            {
                if (mFallingTimer.GetElapsedMs() > mFallTime)
                {
                    BeginShift(gameTime);
                }
            }
            else
            {
                mFallingTimer.Start();
            }

            if (mShiftTimer.IsPlaying())
            {
                if(mShiftTimer.GetElapsedMs() > mShiftTime)
                {
                    EndShift(gameTime);
                }
                else
                {
                    UpdateShift(gameTime);
                }
            }

            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    mTiles[track, tile].Update(gameTime);
                }
            }
        }

        private void BeginShift(GameTime gameTime)
        {
            mFallingTimer.FullReset();
            mShiftTimer.FullReset();
            mShiftTimer.Start();

            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK-1; tile++)
                {
                    mTiles[track, tile] = mTiles[track, tile+1];
                }

                mTiles[track, NUM_TILES_PER_TRACK - 1] = CreateNextTrackTile();
            }

            mDynamicOffset = TILE_OFFSET;
        }

        private void UpdateShift(GameTime gameTime)
        {
            mDynamicOffset = Util.LerpVec(TILE_OFFSET, Vector2.Zero, (float)(mShiftTimer.GetElapsedMs() / mShiftTime));
        }

        private void EndShift(GameTime gameTime)
        {
            mDynamicOffset = Vector2.Zero;

            mFallingTimer.FullReset();
            mShiftTimer.FullReset();
            mFallingTimer.Start();
        }

        private TrackTile CreateNextTrackTile()
        {
            return new BasicTile(mContentManager);
        }


        public void Draw(DrawInfo info)
        {
            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    Texture2D tileTex = mTiles[track, tile].Draw(info);

                    Vector2 position = mDynamicOffset + TRACK_ORIGIN + PADDING + TRACK_PADDING + track * TRACK_OFFSET + tile * TILE_OFFSET;

                    info.spriteBatch.Draw(tileTex, position, Color.White);
                }
            }
        }

    }
}
