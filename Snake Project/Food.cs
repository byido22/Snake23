using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace Snake_Project
{
    class Food : Circle
    {
        Random rand = new Random();
        bool flag1 = true;
        public Food()
        {

        }


        public void CreateFood(int maxWidth, int maxHeight,Wall w , Poison p, List<Circle> s)
        {                  
            while (flag1)
            {
                bool chack1 = false;
                bool chack2 = false;
                bool chack3 = false;
                int x = rand.Next(2, maxWidth);
                int y = rand.Next(2, maxHeight);
                chack1 = foodAndWall(x,y,w);
                chack2 = foodAndPoison(x, y, p);
                chack3 = foodAndSnake(x, y, s);
                if (chack1 == false && chack2 == false && chack3 == false )
                {
                    this.X = x;
                    this.Y = y;
                    flag1 = false;
                }
            }                                           
        }

        public bool foodAndWall(int xPoint,int yPoint, Wall w)
        {
            for (int i=0; i<w.wall.Count;i++)
            {
                if (xPoint==w.wall[i].X && yPoint==w.wall[i].Y)
                {
                    return true;
                }
            }
            return false;

        }

        public bool foodAndSnake(int xPoint, int yPoint, List<Circle> s)
        {
            for (int i = 0; i < s.Count; i++)
            {
                if (xPoint == s[i].X && yPoint == s[i].Y)
                {
                    return true;
                }
            }
            return false;

        }

        public bool foodAndPoison(int xPoint, int yPoint, Poison p)
        {
            if (xPoint == p.X && yPoint == p.Y)
            {
                return true;
            }
            return false;
        }
        public void ClearFood()
        {
            flag1 = true;
            this.X = 0;
            this.Y = 0;
        }
    }   
}
