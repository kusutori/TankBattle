using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 坦克大战
{
    internal class EnemyTank:Movething
    {
        public int ChangeDirSpeed { get; set; }
        private int ChangeDirCount = 0;
        public int AttackSpeed { get; set; }
        private int attackCount = 0;
        private Random r = new Random();

        public EnemyTank(int x, int y, int speed,Bitmap bmpDown,Bitmap bmpUp,Bitmap bmpLeft,Bitmap bmpRight)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapDown = bmpDown;
            BitmapUp = bmpUp;
            BitmapLeft = bmpLeft;
            BitmapRight = bmpRight;
            this.Dir = Direction.Down;
            AttackSpeed = 60;
            ChangeDirSpeed = 70;
        }


        public override void Update()
        {
            MoveCheck();//移动检查
            Move();
            AttackCheck();
            AutoChangeDirection();
            base.Update();
        }

        private void MoveCheck()
        {
            #region 检查有没有超出窗体边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Speed + Height > 450)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Speed + Width > 450)
                {
                    ChangeDirection();
                    return;
                }
            }
            #endregion

            //检查有没有和其他元素发生碰撞
            Rectangle rect = GetRectangle();
            switch (Dir)
            {
                case Direction.Up:
                    rect.Y -= Speed;
                    break;
                case Direction.Down:
                    rect.Y += Speed;
                    break;
                case Direction.Left:
                    rect.X -= Speed;
                    break;
                case Direction.Right:
                    rect.X += Speed;
                    break;
            }
            if (GameObjectManager.IsCollidedWall(rect) != null)//与红砖发生碰撞
            {
                ChangeDirection();
                return;
            }
            if (GameObjectManager.IsCollidedSteel(rect) != null)//与钢墙发生碰撞
            {
                ChangeDirection();
                return;
            }
            if (GameObjectManager.IsCollidedBoss(rect))//与Boss发生碰撞
            {
                ChangeDirection();
                return;
            }
        }

        private void AutoChangeDirection()
        {
            ChangeDirCount++;
            if (ChangeDirCount < ChangeDirSpeed)
                return;
            ChangeDirection();
            ChangeDirCount = 0;
        }

        private void ChangeDirection()
        {
            while (true)
            {
                Direction dir = (Direction)r.Next(0, 4);//改变方向
                if(dir==Dir)//与当前方向相符
                {
                    continue;
                }
                {
                    Dir = dir;
                    break;
                }
            }
            MoveCheck();
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

        private void AttackCheck()//攻击判断
        {
            attackCount++;
            if(attackCount<AttackSpeed)
                return;
            Attack();
            attackCount = 0;
        }

        private void Attack()//攻击
        {
            int x = this.X;
            int y = this.Y;
            switch (Dir)
            {
                case Direction.Up:
                    x = x + Width / 2;
                    break;
                case Direction.Down:
                    x = x + Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y = y + Height / 2;
                    break;
                case Direction.Right:
                    x += Width;
                    y = y + Height / 2;
                    break;
            }
            GameObjectManager.CreateBullet(x, y, Tag.EnemyTank, Dir);
        }
    }
}
