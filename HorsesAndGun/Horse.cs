using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

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
        int mTileIndex;
        int mTrackIndex;

        int mReservedTile;
        int mReservedTrack;
        Animator mIdleAnim;
        Queue<HorseOrder> mOrderQueue;
        HorseOrder mCurrentOrder;

        Vector2 mDestinationPosition;
        Vector2 mDeltaDraw;

        MonoTimer mMoveTimer;
        double mMoveTotal;

        public Horse(Vector2 _pos, int _tileIndex, int _trackIndex) : base(_pos)
        {
            mIdleAnim = new Animator(Animator.PlayType.Loop);
            mOrderQueue = new Queue<HorseOrder>();

            mTileIndex = _tileIndex;
            mTrackIndex = _trackIndex;

            mDestinationPosition = Vector2.Zero;
            mDeltaDraw = Vector2.Zero;

            mMoveTimer = new MonoTimer();
            mMoveTotal = 0.0;

            ClearOrder();
        }

        public override void LoadContent(ContentManager content)
        {
            mTexture = content.Load<Texture2D>("Horse/horse-stand1");

            mIdleAnim.LoadFrame(content, "Horse/horse-stand1", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand2", 0.5f);
            mIdleAnim.LoadFrame(content, "Horse/horse-stand3", 0.5f);
            mIdleAnim.Play();
        }

        public override Rect2f ColliderBounds()
        {
            Vector2 pos = mDeltaDraw + mPosition;

            return new Rect2f(pos, pos + new Vector2(mTexture.Width, mTexture.Height));
        }

        public override void Draw(DrawInfo info)
        {
            Texture2D texture = mIdleAnim.GetCurrentTexture();

            Vector2 pos = mDeltaDraw + mPosition;

            if(mCurrentOrder.type == HorseOrderType.none)
            {
                mDeltaDraw = Vector2.Zero;
            }

            info.spriteBatch.Draw(texture, pos, Color.White);
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
            mOrderQueue.Enqueue(order);
        }

        public HorseOrder PopTopOrder()
        {
            return mOrderQueue.Dequeue();
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
                return order.moveAmount * 600.0;
            }
            else
            {
                return order.moveAmount * 200.0;
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
