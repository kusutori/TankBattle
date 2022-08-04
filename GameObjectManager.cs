using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 坦克大战
{
    internal class GameObjectManager
    {
        private static List<NotMovething> wallList = new List<NotMovething>();
        private static List<NotMovething> steelList = new List<NotMovething>();
        private static NotMovething boss;
        private static MyTank myTank;

        private static List<EnemyTank> tankList = new List<EnemyTank>();
        private static List<Bullet> bulletList = new List<Bullet>();
        private static List<Explosion> expList=new List<Explosion>();

        private static int enemyBornSpeed = 120;
        private static int enemyBornCount = 60;
        private static Point[] points = new Point[3];//存放三个出生点

        public static void Start()
        {
            points[0].X = 0;
            points[0].Y = 0;
            points[1].X = 7*30;
            points[1].Y = 0;
            points[2].X = 14 * 30;
            points[2].Y = 0;
        }

        public static void Update()
        {
            foreach (NotMovething nm in wallList)//红砖墙的更新
            {
                nm.Update();
            }
            foreach (NotMovething nm in steelList)//钢墙的更新
            {
                nm.Update();
            }
            foreach(EnemyTank tank in tankList)//坦克的更新
            {
                tank.Update();
            }
            CheckAndDestroyBullet();
            /*foreach (Bullet bullet in bulletList)//子弹的更新
            {
                bullet.Update();
            }*/
            for (int i=0;i<bulletList.Count;i++)//子弹的更新
            {
                bulletList[i].Update();
            }
            foreach(Explosion exp in expList)//爆炸效果的更新
            {
                exp.Update();
            }
            CheckAndDestroyExplosion();
            boss.Update();//boss的更新

            myTank.Update();//自己坦克的更新

            EnemyBorn();//控制敌人生成

        }

        private static void CheckAndDestroyBullet()//通过检查判断子弹是否需要销毁
        {
            List<Bullet> needToDestroy = new List<Bullet>();
            foreach(Bullet bullet in bulletList)//待销毁
            {
                if(bullet.IsDestroy==true)
                {
                    needToDestroy.Add(bullet);
                }
            }
            foreach (Bullet bullet in needToDestroy)//实行销毁
            {
                bulletList.Remove(bullet);  
            }
        }

        private static void CheckAndDestroyExplosion()
        {
            List<Explosion> needToDestroy = new List<Explosion>();
            foreach(Explosion exp in expList)//待销毁
            {
                if(exp.IsNeedDestroy==true)
                {
                    needToDestroy.Add(exp);
                }
            }
            foreach(Explosion exp in needToDestroy)//实行销毁
            {
                expList.Remove(exp);
            }
        }

        public static void CreateExplosion(int x,int y)//创建爆炸效果
        {
            Explosion exp = new Explosion(x, y);
            expList.Add(exp);
        }

        public static void CreateBullet(int x, int y, Tag tag, Direction dir)//创建子弹
        {
            Bullet bullet = new Bullet(x, y, 5, dir, tag);          
                bulletList.Add(bullet);            
        }

        public static void DestroyWall(NotMovething wall)//销毁墙体
        {
            wallList.Remove(wall);
        }

        public static void DestroyTank(EnemyTank tank)//销毁敌方坦克
        {
            tankList.Remove(tank);
        }

        private static void EnemyBorn()//控制敌人生成
        {
            enemyBornCount++;
            if (enemyBornCount < enemyBornSpeed)
                return;

            SoundManager.PlayAdd();//敌人生成时播放音乐

            //生成0~2的随机数，用来选择位置
            Random rd = new Random();
            int index= rd.Next(0, 3);
            Point position = points[index];
            int enemyType = rd.Next(1, 6);
            switch(enemyType)//随机生成五种坦克
            {
                case 1:
                    CreateEnemyTank1(position.X, position.Y);
                    break;
                case 2:
                    CreateEnemyTank2(position.X, position.Y);
                    break;
                case 3:
                    CreateEnemyTank3(position.X, position.Y);
                    break;
                case 4:
                    CreateEnemyTank4(position.X, position.Y);
                    break;
                case 5:
                    CreateEnemyTank5(position.X, position.Y);
                    break;
            }

            enemyBornCount = 0;
        }
        #region 生成五种敌方坦克
        private static void CreateEnemyTank1(int x,int y)
        {
            EnemyTank tank = new EnemyTank(x, y, 2, Resources.GrayDown, Resources.GrayUp, Resources.GrayLeft, Resources.GrayRight);
            tankList.Add(tank);
        }
        
        private static void CreateEnemyTank2(int x,int y)
        {
            EnemyTank tank = new EnemyTank(x, y, 2, Resources.GreenDown, Resources.GreenUp, Resources.GreenLeft, Resources.GreenRight);
            tankList.Add(tank);
        }
        
        private static void CreateEnemyTank3(int x,int y)
        {
            EnemyTank tank=new EnemyTank(x,y,4,Resources.QuickDown,Resources.QuickUp,Resources.QuickLeft, Resources.QuickRight);
            tankList.Add(tank);
        }
        
        private static void CreateEnemyTank4(int x,int y)
        {
            EnemyTank tank = new EnemyTank(x,y,1,Resources.SlowDown,Resources.SlowUp, Resources.SlowLeft, Resources.SlowRight);
            tankList.Add(tank);
        }
        
        private static void CreateEnemyTank5(int x,int y)
        {
            EnemyTank tank=new EnemyTank(x,y,2,Resources.YellowDown,Resources.YellowUp, Resources.YellowLeft, Resources.YellowRight);
            tankList.Add(tank);
        }
        #endregion

        public static NotMovething IsCollidedWall(Rectangle rt)//判断矩形是否和墙发生碰撞
        {
            foreach(NotMovething wall in wallList)
            {
                if(wall.GetRectangle().IntersectsWith(rt))
                {
                    return wall;
                }
            }
            return null;
        }
        
        public static NotMovething IsCollidedSteel(Rectangle rt)//判断矩形是否和墙发生碰撞
        {
            foreach(NotMovething wall in steelList)
            {
                if(wall.GetRectangle().IntersectsWith(rt))
                {
                    return wall;
                }
            }
            return null;
        }

        public static bool IsCollidedBoss(Rectangle rt)//与Boss发生碰撞
        {
            return boss.GetRectangle().IntersectsWith(rt);
        }

        public static MyTank IsCollidedMyTank(Rectangle rt)
        {
            if(myTank.GetRectangle().IntersectsWith(rt))
                return myTank; ;
            return null;
        }

        public static EnemyTank IsCollidedEnemyTank(Rectangle rt)
        {
            foreach(EnemyTank tank in tankList)
            {
                if(tank.GetRectangle().IntersectsWith(rt))
                {
                    return tank;
                }
            }
            return null;
        }

        //public static void DrawMap()//绘制地图
        //{
        //    foreach (NotMovething nm in wallList)
        //    {
        //        nm.DrawSelf();
        //    }
        //    foreach (NotMovething nm in steelList)
        //    {
        //        nm.DrawSelf();
        //    }
        //    boss.DrawSelf();
        //}

        //public static void DrawMyTank()//绘制坦克
        //{
        //    myTank.DrawSelf();
        //}

        public static void CreateMyTank()//创建坦克
        {
            int x = 5 * 30;
            int y = 14 * 30;
            myTank = new MyTank(x, y, 2);
        }

        public static void CreateMap()//创建地图
        {
            //绘制红砖
            //上半部分
            CreateWall(1, 1, 5, Resources.wall, wallList);
            CreateWall(3, 1, 5, Resources.wall, wallList);
            CreateWall(5, 1, 4, Resources.wall, wallList);
            CreateWall(7, 1, 3, Resources.wall, wallList);
            CreateWall(9, 1, 4, Resources.wall, wallList);
            CreateWall(11, 1, 5, Resources.wall, wallList);
            CreateWall(13, 1, 5, Resources.wall, wallList);
            //中间部分
            CreateWall(2, 7, 1, Resources.wall, wallList);
            CreateWall(3, 7, 1, Resources.wall, wallList);
            CreateWall(4, 7, 1, Resources.wall, wallList);
            CreateWall(6, 7, 1, Resources.wall, wallList);
            CreateWall(7, 6, 2, Resources.wall, wallList);
            CreateWall(8, 7, 1, Resources.wall, wallList);
            CreateWall(10, 7, 1, Resources.wall, wallList);
            CreateWall(11, 7, 1, Resources.wall, wallList);
            CreateWall(12, 7, 1, Resources.wall, wallList);
            //下半部分
            CreateWall(1, 9, 5, Resources.wall, wallList);
            CreateWall(3, 9, 5, Resources.wall, wallList);
            CreateWall(5, 9, 3, Resources.wall, wallList);
            CreateWall(6, 10, 1, Resources.wall, wallList);
            CreateWall(7, 10, 2, Resources.wall, wallList);
            CreateWall(8, 10, 1, Resources.wall, wallList);
            CreateWall(9, 9, 3, Resources.wall, wallList);
            CreateWall(11, 9, 5, Resources.wall, wallList);
            CreateWall(13, 9, 5, Resources.wall, wallList);
            //boss部分
            CreateWall(6, 13, 2, Resources.wall, wallList);
            CreateWall(7, 13, 1, Resources.wall, wallList);
            CreateWall(8, 13, 2, Resources.wall, wallList);
            //绘制钢墙
            CreateWall(7, 5, 1, Resources.steel, steelList);
            CreateWall(0, 7, 1, Resources.steel, steelList);
            CreateWall(14, 7, 1, Resources.steel, steelList);
            //绘制boss
            CreateBoss(7, 14, Resources.Boss);
        }
        
        //绘制boss
        private static void CreateBoss(int x,int y,Image img)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;
            boss=new NotMovething(xPosition,yPosition,img);

        }

        
        //绘制一列墙体
        private static void CreateWall(int x,int y,int count,Image img,List<NotMovething> wallList)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;
            for(int i=yPosition;i<yPosition+count*30;i+=15)
            {
                NotMovething wall1 = new NotMovething(xPosition, i, img);
                NotMovething wall2 = new NotMovething(xPosition+15, i, img);
                wallList.Add(wall1);
                wallList.Add(wall2);

            }
        }

        public static void KeyDown(KeyEventArgs args)
        {
            myTank.KeyDown(args);
        }

        public static void KeyUp(KeyEventArgs args)
        {
            myTank.KeyUp(args);
        }
    }
}
