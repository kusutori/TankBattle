using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 坦克大战
{
    abstract internal class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        protected abstract Image GetImage();

        public virtual void DrawSelf()
        {
            Graphics g = GameFramwork.g;
            g.DrawImage(GetImage(), X, Y);
        }

        public virtual void Update()//实时更新
        {
            DrawSelf();//每次更新重绘
        }

        public Rectangle GetRectangle()
        {
            Rectangle rectangle=new Rectangle(X, Y, Width, Height);
            return rectangle;
        }
    }
}
