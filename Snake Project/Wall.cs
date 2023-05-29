using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Project
{
    class Wall : Circle
    {
        public List<Circle> wall = new List<Circle>();
        public List<Wall> walls = new List<Wall>();
        Random rand = new Random();      
        public int wallNum = 1;
        public Wall()
        {

        }        

        public void CreateWall(int maxWidth , int maxHeight , int xSnake , int ySnake , int score)
        {
            int SnakeLength = score + 11;
            int x = rand.Next(2, maxWidth);
            int y = rand.Next(2, maxHeight);
            for (int i=0;i<wallNum;i++)
            {
                do
                {
                    x = rand.Next(2, maxWidth);
                    y = rand.Next(2, maxHeight);
                } while (!(x >= xSnake) && !(x < xSnake + SnakeLength) && !(y >= ySnake) && !(y < ySnake + SnakeLength));
                //Chacking that the wall creation wont spwan on the snake

                Circle first_brick = new Circle();
                first_brick.X = x;
                first_brick.Y = y;
                wall.Add(first_brick);
                int j = 1;
                for (int k = 1; k < 7; k++, j++)
                {
                    Circle brick = new Circle { X = first_brick.X + j, Y = first_brick.Y };
                    wall.Add(brick);
                }
            }          
                     
        }

        public void ClearWall()
        {
            wallNum = 1;
            wall.Clear();
        }

        
    }
}
