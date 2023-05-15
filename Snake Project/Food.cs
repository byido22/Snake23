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
        public Food()
        {

        }

        public void CreateFood(int maxWidth,int maxHeight)
        {
            int x = rand.Next(2, maxWidth);
            int y = rand.Next(2, maxHeight);
            this.X = x;
            this.Y = y;          
        }

        public void CreateFood(int maxWidth, int maxHeight,Wall w)
        {
            bool flag = true;
            while (flag)
            {
                int x = rand.Next(2, maxWidth);
                int y = rand.Next(2, maxHeight);
                if (x != w.X && y != w.Y)
                {
                    this.X = x;
                    this.Y = y;
                    flag = false;
                }
            }                                           
        }

    }
}
