﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Project
{
    class Bonus : Food
    {
        bool flag1 = true;
        bool flag2 = true;
        Random rand = new Random();
        public void CreateBouns(int maxWidth, int maxHeight, Wall w)
        {
            while (flag1)
            {
                int x = rand.Next(2, maxWidth);
                int y = rand.Next(2, maxHeight);
                x = rand.Next(2, maxWidth);
                y = rand.Next(2, maxWidth);
                for (int i = 0; i < w.wall.Count; i++)
                {
                    if (x == w.wall[i].X && y == w.wall[i].Y)
                    {
                        flag2 = false;
                    }
                }
                if (flag2 == true)
                {
                    this.X = x;
                    this.Y = y;
                    flag1 = false;
                }
            }
        }

        public void clearBouns()
        {
            this.X = 500;
            this.Y = 500;
            flag1 = true;
            flag2 = true;
        }
        //Random rand = new Random();
        //Food f = new Food();
        //int maxW;
        //int maxH;
        //public Bonus(int maxW, int maxH) // constructor 
        //{
        //    this.maxW = maxW;
        //    this.maxH = maxH;
        //}

        //// public void createBonus() // point of bonus
        // {
        //     // f.CreateFood(maxW, maxH);
        // }

        //public void snakeAndBonus() // function when snake eats bonus
        //{

        //    //Circle body = new Circle
        //    //{
        //    //    X = snake[snake.Count - 1].X,
        //    //    Y = snake[snake.Count - 1].Y
        //    //};

        //    //snake.Add(body);



        //}

    }
}
