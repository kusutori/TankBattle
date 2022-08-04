using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 坦克大战
{
    enum Tag
    {
        MyTank,
        EnemyTank
    }

    internal class Bullet : Movething
    {
        public Tag Tag { get; set; }

        public bool IsDestroy { get; set; }

        public Bullet(int x, int y, int speed,Direction dir,Tag tag)
        {
            IsDestroy = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapDown = Resources.BulletDown;
            BitmapUp = Resources.BulletUp;
            BitmapLeft = Resources.BulletLeft;
            BitmapRight = Resources.BulletRight;
            this.Dir = dir;
            this.Tag = tag;

            this.X -= Width / 2;
            this.Y -= Height / 2;
        }

        public override void DrawSelf()
        {
            base.DrawSelf();
        }


        public override void Update()
        {
            MoveCheck();//移动检查
            Move();
            base.Update();
        }

        private void MoveCheck()//移动检查
        {
            #region 检查有没有超出窗体边界
            if (Dir == Direction.Up)
            {
                if (Y+Height/2+3 < 0)
                {
                    IsDestroy=true;
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y+Height-3 > 450)
                {
                    IsDestroy=true;
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X+Width/2-3 < 0)
                {
                    IsDestroy =true;
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Width +3> 450)
                {
                    IsDestroy=true;
                    return;
                }
            }
            #endregion

            //检查有没有和其他元素发生碰撞
            Rectangle rect = GetRectangle();

            rect.X = X + Width / 2 - 3;
            rect.Y = Y + Height / 2 - 3;
            rect.Height = 3;
            rect.Width = 3;

            int xExplosino = this.X + Width / 2;
            int yExplosino = this.Y + Height / 2;

            NotMovething wall = null;

            if ((wall= GameObjectManager.IsCollidedWall(rect)) != null)//与红砖发生碰撞
            {
                IsDestroy = true;//先销毁自身
                GameObjectManager.DestroyWall(wall);//销毁墙体
                GameObjectManager.CreateExplosion(xExplosino, yExplosino);//爆炸效果
                SoundManager.PlayBlast();
                return;
            }
            if (GameObjectManager.IsCollidedSteel(rect) != null)//与钢墙发生碰撞
            {
                IsDestroy = true;//只销毁子弹自身
                GameObjectManager.CreateExplosion(xExplosino, yExplosino);
                return;
            }
            if (GameObjectManager.IsCollidedBoss(rect))//与Boss发生碰撞
            {
                GameFramwork.ChangeToGameOver();
                SoundManager.PlayBlast();
                return;
            }

            if(Tag==Tag.MyTank)//己方子弹
            {
                EnemyTank tank = null;
                if((tank=GameObjectManager.IsCollidedEnemyTank(rect))!=null)
                {
                    IsDestroy = true;//销毁子弹
                    GameObjectManager.DestroyTank(tank);//销毁坦克
                    GameObjectManager.CreateExplosion(xExplosino, yExplosino);//创造爆炸效果
                    SoundManager.PlayBlast();
                    return;
                }
            }
            else if(Tag==Tag.EnemyTank)
            {
                MyTank mytank=null;
                if((mytank=GameObjectManager.IsCollidedMyTank(rect))!= null)
                {
                    IsDestroy=true;//销毁子弹
                    GameObjectManager.CreateExplosion(xExplosino, yExplosino);
                    SoundManager.PlayHit();
                    mytank.TakeDamage();
                }
            }
        }

        private void Move()//控制移动
        {
            switch (Dir)
            {
                case Direction.Up:
                    Y -= Speed;
                    break;
                case Direction.Down:
                    Y += Speed;
                    break;
                case Direction.Left:
                    X -= Speed;
                    break;
                case Direction.Right:
                    X += Speed;
                    break;
            }
        }
    }
}
