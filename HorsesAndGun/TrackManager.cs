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
        const int NUM_HORSES = 3;
        const int NUM_TILES_PER_TRACK = 11;

        Vector2 TRACK_ORIGIN = new Vector2(83.0f, 28.0f);
        Vector2 TRACK_OFFSET = new Vector2(0.0f, 50.0f);
        Vector2 PADDING = new Vector2(4.0f, 0.0f);
        Vector2 TILE_OFFSET = new Vector2(50.0f, 0.0f);

        Vector2 mDynamicOffset = new Vector2(0.0f, 0.0f);

        const double START_FALL_TIME = 3000.0;

        TrackTile[,] mTiles;
        ContentManager mContentManager;

        Horse[] mHorses;

        //Timings
        double mFallTime = 3000.0;
        MonoTimer mFallingTimer;

        double mShiftTime = 3000.0;
        MonoTimer mShiftTimer;

        public TrackManager(ContentManager _ContentManager)
        {
            mTiles = new TrackTile[NUM_TRACKS, NUM_TILES_PER_TRACK];
            mContentManager = _ContentManager;

            mHorses = new Horse[NUM_HORSES];

            //Timings
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
                    mTiles[track, tile] = CreateNextTrackTile();
                }
            }

            for(int horse = 0; horse < NUM_HORSES; horse++)
            {
                mHorses[horse] = new Horse(Vector2.Zero, 6, NUM_TRACKS - horse - 1);
                EntityManager.I.RegisterEntity(mHorses[horse], mContentManager);
            }
        }

        public void Update(GameTime gameTime)
        {
            //Shift logic
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

            //Update tiles
            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    mTiles[track, tile].Update(gameTime);
                }
            }

            //Update horse positions
            SetHorsePositions(gameTime);
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

            for (int h = 0; h < NUM_HORSES; h++)
            {
                Horse horse = mHorses[h];

                horse.TileIndex--;
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

        private void SetHorsePositions(GameTime gameTime)
        {
            for (int h = 0; h < NUM_HORSES; h++)
            {
                Horse horse = mHorses[h];

                horse.position = GetTilePos(horse.TrackIndex, horse.TileIndex);
            }
        }

        private TrackTile CreateNextTrackTile()
        {
            if(RandomManager.I.GetIntInRange(1,10) >= 9)
            {
                return new PlusTile(mContentManager);
            }

            return new BasicTile(mContentManager);
        }


        public void Draw(DrawInfo info)
        {
            for (int track = 0; track < NUM_TRACKS; track++)
            {
                for (int tile = 0; tile < NUM_TILES_PER_TRACK; tile++)
                {
                    Texture2D tileTex = mTiles[track, tile].Draw(info);

                    Vector2 tilePosition = GetTilePos(track, tile);

                    info.spriteBatch.Draw(tileTex, tilePosition, Color.White);
                }
            }
        }

        private Vector2 GetTilePos(int track, int tile)
        {
            return mDynamicOffset + TRACK_ORIGIN + PADDING + track * TRACK_OFFSET + tile * TILE_OFFSET;
        }
    }
}
