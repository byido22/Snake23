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
        Random rand = new Random();
        public bool Active = false;
        public Wall()
        {

        }        

        public void CreateWall(int maxWidth , int maxHeight)
        {          
            Circle first_brick = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            
           wall.Add(first_brick);
            int j = 1;
            for (int i = 1; i < 7; i++, j++)
            {
                Circle brick = new Circle { X = first_brick.X + j, Y = first_brick.Y };
                wall.Add(brick);
            }
            this.Active = true;
        }
    }
}
