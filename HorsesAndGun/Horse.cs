﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Nito.Collections;

namespace HorsesAndGun
{
    enum HorseOrderType
    {
        moveTile,
        moveTrack,
        none
    }

    struct HorseOrder
    {
        public HorseOrder(HorseOrderType _type, int _moveAmount)
        {
            moveAmount = _moveAmount;
            type = _type;
            finished = false;
        }

        public int moveAmount;
        public HorseOrderType type;
        public bool finished;
    }

    internal class Horse : Entity
    {
        //Position
        int mTileIndex;
        int mTrackIndex;
        int mReservedTile;
        int mReservedTrack;
        
        //Orders
        Deque<HorseOrder> mOrderQueue;
        HorseOrder mCurrentOrder;

        //Lerp data
        Vector2 mDestinationPosition;
        Vector2 mDeltaDraw;

        MonoTimer mMoveTimer;
        double mMoveTotal;

        //Textures
        Animator mIdleAnim;
        Animator mRunAnim;

        //State
        bool mAlive;

        public Horse(Vector2 _pos, int _tileIndex, int _trackIndex) : base(_pos)
        {
            mIdleAnim = new Animator(Animator.PlayType.Loop);
            mRunAnim = new Animator(Animator.PlayType.Loop);

            mOrderQueue = new Deque<HorseOrder>();

            mTileIndex = _tileIndex;
            mTrackIndex = _trackIndex;

            mDestinationPosition = Vector2.Zero;
            mDeltaDraw = Vector2.Zero;

            mMoveTimer = new MonoTimer();
            mMoveTotal = 0.0;

            mAlive = true;

            ClearOrder();
        }

        public override void Kill()
        {
            mAlive = false;
            base.Kill();
        }

        public bool IsAlive()
        {
            return mAlive;
        }

        public Vector2 GetEffectivePos()
        {
            return mPosition + mDeltaDraw;
        }

        public override void LoadContent(ContentManager content)
        {
            mTexture = content.Load<Texture2D>("Horse/horse-stand1");

            mIdleAnim.LoadFrame(content, "Horse/horse-stand1", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand2", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.Play();

            const float RUN_ANIM_SPEED = 0.1f;
            mRunAnim.LoadFrame(content, "Horse/horse_run_1", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_2", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_3", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_4", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_5", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_6", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_7", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_8", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_9", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_10", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_11", RUN_ANIM_SPEED);
            mRunAnim.LoadFrame(content, "Horse/horse_run_12", RUN_ANIM_SPEED);
            mRunAnim.Play();
        }

        public override Rect2f ColliderBounds()
        {
            Vector2 pos = GetEffectivePos();
            return new Rect2f(pos, pos + new Vector2(mTexture.Width, mTexture.Height));
        }

        public override void Draw(DrawInfo info)
        {
            Texture2D texture = mIdleAnim.GetCurrentTexture();

            //if(mCurrentOrder.type != HorseOrderType.none || mOrderQueue.Count > 0)
            {
                texture = mRunAnim.GetCurrentTexture();
            }

            info.spriteBatch.Draw(texture, GetEffectivePos(), Color.White);

        }

        public void ShiftHorseBack()
        {
            mTileIndex--;
            mReservedTile--;
        }

        public void SetDestPosition(Vector2 dest)
        {
            mDestinationPosition = dest;
        }

        public override void Update(GameTime gameTime)
        {
            mIdleAnim.Update(gameTime);
            mRunAnim.Update(gameTime);

            UpdateCurrentOrder(gameTime);
        }

        private void UpdateCurrentOrder(GameTime gameTime)
        {
            if(mCurrentOrder.type != HorseOrderType.none)
            {
                float lerpVal = (float)(mMoveTimer.GetElapsedMs() / mMoveTotal);

                if(lerpVal < 1.0)
                {
                    mDeltaDraw = Util.LerpVec(Vector2.Zero, mDestinationPosition - mPosition, lerpVal);
                }
                else
                {
                    mCurrentOrder.finished = true;
                }
            }
        }

        public override void CollideWithEntity(Entity entity)
        {
            if(entity is MovingDie)
            {
                MovingDie movingDie = (MovingDie)entity;

                if (movingDie.GetEnabled())
                {
                    movingDie.Kill();

                    QueueOrder(HorseOrderType.moveTile, movingDie.GetDice().Value);
                }
            }
        }

        public void QueueOrder(HorseOrderType _type, int _amount)
        {
            QueueOrder(new HorseOrder(_type, _amount));
        }

        public void QueueOrder(HorseOrder order)
        {
            mOrderQueue.AddToBack(order);
        }

        public void QueueOrderFront(HorseOrderType _type, int _amount)
        {
            mOrderQueue.AddToFront(new HorseOrder(_type, _amount));
        }

        public void QueueOrderFront(HorseOrder order)
        {
            mOrderQueue.AddToFront(order);
        }

        public HorseOrder PopTopOrder()
        {
            return mOrderQueue.RemoveFromFront();
        }

        public void ExecuteOrder(HorseOrder order, Point finalPos)
        {
            if(mCurrentOrder.type == HorseOrderType.none)
            {
                mCurrentOrder = order;
                mCurrentOrder.finished = false;

                mMoveTotal = OrderTime(mCurrentOrder);
                mMoveTimer.FullReset();
                mMoveTimer.Start();
            }
        }

        private double OrderTime(HorseOrder order)
        {
            if (order.type == HorseOrderType.moveTile)
            {
                return Math.Abs(order.moveAmount) * 600.0;
            }
            else
            {
                return Math.Abs(order.moveAmount) * 600.0;
            }
        }

        public bool ReadyToMove()
        {
            return mOrderQueue.Count != 0 && mCurrentOrder.type == HorseOrderType.none;
        }

        public bool FinishedMove()
        {
            return mCurrentOrder.type != HorseOrderType.none && mCurrentOrder.finished == true;
        }

        public void FinishOrder()
        {
            mTileIndex = mReservedTile;
            mTrackIndex = mReservedTrack;
            mDeltaDraw = Vector2.Zero;

            ClearOrder();
        }

        public void ClearOrder()
        {
            mCurrentOrder = new HorseOrder(HorseOrderType.none, 0);
            mReservedTile = mTileIndex;
            mReservedTrack = mTrackIndex;
        }

        public Point GetReservedPoint()
        {
            return new Point(mReservedTile, mReservedTrack);
        }

        public void ReserveTile(Point toReserve)
        {
            mReservedTile = toReserve.X;
            mReservedTrack = toReserve.Y;
        }

        public Point GetCurrentPoint()
        {
            return new Point(mTileIndex, mTrackIndex);
        }

        public int TrackIndex
        {
            get { return mTrackIndex; }
            set { mTrackIndex = value; }
        }

        public int TileIndex
        {
            set { mTileIndex = value; }
            get { return mTileIndex; }
        }
    }
}
