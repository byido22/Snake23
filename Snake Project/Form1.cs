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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

namespace Snake_Project
{
    public partial class Form1 : Form
    {
        private List<Wall> wallList = new List<Wall>();
        private List<Circle> Snake = new List<Circle>();
        static private Bonus b = new Bonus();
        private Food food = new Food();
        private Wall w = new Wall();
        private Poison p = new Poison();
        Highscore h = new Highscore();
        Random rand = new Random();

        public int maxWidth;
        public int maxHeight;
        public int score;
        public int highscore;
        public int totalCount = 0;
        private float rotationAngle = 90f;
        bool goLeft, goRight, goUp, goDown;
        bool poisonIndex = false;

        public Form1()
        {
            InitializeComponent();

            new Settings();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.direction != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.direction != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.direction != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.direction != "up")
            {
                goDown = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapshot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored " + score + " and my highscore is " + h.Value + " on the classic snake game";
            caption.Font = new Font("Ariel", 9, FontStyle.Bold);
            caption.ForeColor = Color.DarkBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);
            //Creats label for us to add on the top part of the screenshot

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Classic Snake game Screenshot";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;
            //Save the JPG file

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
            //When we save a picture we define the width ,the height and the format of the picture and add the label we created above
            picCanvas.Controls.Remove(caption);
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            
            if (goLeft)
            {
                Settings.direction = "left";
                rotationAngle = 90f;
            }
            if (goRight)
            {
                Settings.direction = "right";
                rotationAngle = 270f;
            }
            if (goDown)
            {
                Settings.direction = "down";
                rotationAngle = 0f;
            }
            if (goUp)
            {
                Settings.direction = "up";
                rotationAngle = 180f;
            }
            //Setting the direction

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                    }
                    //Direction events

                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }
                    //Makes the snake appear on the other side of the canvas when its out of the bounds

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }
                    if (Snake[i].X == p.X && Snake[i].Y == p.Y)
                    {
                        eatPoison();
                    }
                    if (Snake[i].X == b.X && Snake[i].Y == b.Y)
                    {
                        eatBonus();
                    }
                    //If the snake head and the food in the same x,y we call the EatFood function

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }

                    //If the snake's head and body are in the same x,y call the GameOver function
                    bool check_1 = WallAndSnake(Snake[i].X, Snake[i].Y);

                    if (check_1 == true)
                    {
                        GameOver();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
                //Makes the body follow after the last circle in the list
            }
            picCanvas.Invalidate();
            //Clear the canvas and redrwen
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            //Linking the pain event to the canvas

            int enlargedWidth = Settings.Width + 3;
            int enlargedHeight = Settings.Height + 3;
            int HenlargedWidth = Settings.Width + 8;
            int HenlargedHeight = Settings.Height + 8;
            int PenlargedWidth = Settings.Width + 10;
            int PenlargedHeight = Settings.Height + 10;
            //Diffrent sizes to the images
            int offsetX;
            int offsetY;
           
            Image wallImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\fance.png");
            Image poisonImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\Poison.png");
            Image snakeHeadImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\SnakeHead.png");
            Image snakeBodyImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\SnakeBody.png");
            Image foodImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\Burger.png");
            Image bonusImage = Image.FromFile(@"C:\Users\byido\source\repos\Snake Project\Snake Project\img\Bonus.png");
                

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    offsetX = Snake[i].X * Settings.Width + (Settings.Width - HenlargedWidth) / 2;
                    offsetY = Snake[i].Y * Settings.Height + (Settings.Height - HenlargedHeight) / 2;

                    canvas.TranslateTransform(offsetX + HenlargedWidth / 2, offsetY + HenlargedHeight / 2);
                    canvas.RotateTransform(rotationAngle);
                    canvas.TranslateTransform(-HenlargedWidth / 2, -HenlargedHeight / 2);
                    canvas.DrawImage(snakeHeadImage, new Rectangle(0, 0, HenlargedWidth, HenlargedHeight));
                    canvas.ResetTransform();
                }
                else
                {
                    offsetX = Snake[i].X * Settings.Width + (Settings.Width - enlargedWidth) / 2;
                    offsetY = Snake[i].Y * Settings.Height + (Settings.Height - enlargedHeight) / 2;

                    canvas.TranslateTransform(offsetX + enlargedWidth / 2, offsetY + enlargedHeight / 2);
                    canvas.RotateTransform(rotationAngle);
                    canvas.TranslateTransform(-enlargedWidth / 2, -enlargedHeight / 2);
                    canvas.DrawImage(snakeBodyImage, new Rectangle(0, 0, enlargedWidth, enlargedHeight));
                    canvas.ResetTransform();

                }
                           
            }
            //Adds the image to the Snake list , head and body
            canvas.DrawImage(foodImage, new Rectangle
               (food.X * Settings.Width - (PenlargedWidth - Settings.Width) / 2,
               food.Y * Settings.Height - (PenlargedHeight - Settings.Height) / 2,
              PenlargedWidth, PenlargedHeight
               ));
            //Adds the image to the food 

            for (int i = 0; i < w.wall.Count; i++)
            {

                canvas.DrawImage(wallImage, new Rectangle
                   (w.wall[i].X * Settings.Width - (enlargedWidth - Settings.Width) / 2,
                   w.wall[i].Y * Settings.Height - (enlargedHeight - Settings.Height) / 2,
                   enlargedWidth, enlargedHeight

                   ));

            }
            //Adds the image to the Wall list

            canvas.DrawImage(poisonImage, new Rectangle
                (p.X * Settings.Width - (PenlargedWidth - Settings.Width) / 2,
                p.Y * Settings.Height - (PenlargedHeight - Settings.Height) / 2,
               PenlargedWidth, PenlargedHeight
                ));
            //Adds the image to the Poison class

            canvas.DrawImage(bonusImage, new Rectangle
              (b.X * Settings.Width - (PenlargedWidth - Settings.Width) / 2,
              b.Y * Settings.Height - (PenlargedHeight - Settings.Height) / 2,
             PenlargedWidth, PenlargedHeight
              ));
            //Adds the image to the Bonus class
        }

        private bool WallAndSnake(int Xpoint, int Ypoint)
        {
            for (int k = 0; k < w.wall.Count; k++)
            {
                if (Xpoint == w.wall[k].X && Ypoint == w.wall[k].Y)
                {
                    return true;
                }
            }
            return false;
        }
        //Chacking if the head hits the wall

        private void goFast()
        {
            gameTimer.Interval = 25;
        }
        //Increases the snake's speed

        private void goSlow()
        {
            gameTimer.Interval = 40;
        }
        //Lowers the snake's speed

        private void RestartGame()
        {
            h = Highscore.LoadFile("highscore.xml");
            using (StreamWriter writer = new StreamWriter("highscore.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Highscore));
                serializer.Serialize(writer, h);
                writer.Close();
            }
            //Serialize

            txtHighscore.Text = "Highscore: " + h.Value;
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;
            //Marking the edge of the canvas 

            totalCount = 0;
            //Restart the total counting

         

            if (poisonIndex == true)
            {
                poisonIndex = false;
                goSlow();
            }
            //Restarting the speed of the snake

            Snake.Clear();
            //Clears the snake list

            w.ClearWall();
            //Clears the wall list 

            food.ClearFood();
            //Clears the food

            p.clearPoison();
            //Clears the poison

            b.clearBouns();
            //Clears the bonus

            startButton.Enabled = false;
            snapButton.Enabled = false;
            //Disable the buttons to use the keyboerd

            score = 0;
            txtScore.Text = "Score: " + score;
            //Update the score         

            Circle head = new Circle { X = 10, Y = 5 };
            //Center the Snake in the canvas

            Snake.Add(head);
            //Adding the head part of the snake to the list

            for (int i = 0; i < 10; i++)
            {

                Snake.Add(new Circle());
            }
            //Adding the body part to the list

            food.CreateFood(maxWidth, maxHeight, w,p,Snake);
            //Creating the food circle

            gameTimer.Start();
            //Starting the timer
        }

        public void eatBonus()
        {
            b.clearBouns();
            score += 2;
            txtScore.Text = "score: " + score;
            //Score added 2 points 

            if (w.wallNum > 1)
            {
                w.wallNum--;
            }
            //Delete 1 wall

            w.wall.Clear();
            food.ClearFood();
            w.CreateWall(maxWidth, maxHeight, Snake[0].X, Snake[0].Y,score);
            food.CreateFood(maxWidth, maxHeight, w,p,Snake);
            //Reset the Food and the Wall position

            for (int i = 0; i < 2; i++)
            {
                Circle body = new Circle
                {
                    X = Snake[Snake.Count - 1].X,
                    Y = Snake[Snake.Count - 1].Y
                };
                //Creates new body circle

                Snake.Add(body);
            }

        }

        public void eatPoison()
        {
            p.clearPoison();
            score--;
            txtScore.Text = "Score: " + score;
            //Score update

            w.wallNum++;
            w.wall.Clear();
            w.CreateWall(maxWidth, maxHeight, Snake[0].X, Snake[0].Y,score);
            //Adding a wall

            food.ClearFood();
            food.CreateFood(maxWidth, maxHeight, w,p,Snake);
            //Reset the Food

            Snake[Snake.Count - 1].deleteCircle();
            Snake.RemoveAt(Snake.Count - 1);
            //shortens the snake

            goFast();
            //increases the speed of the snake

            poisonIndex = true;

        }

        private void EatFood()
        {
            food.ClearFood();

            totalCount++;
            //Update total count
            score++;
            txtScore.Text = "Score: " + score;
            //Score update

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            //Creates new body circle

            Snake.Add(body);
            //Adds the new circle to the end of the list

            if (poisonIndex == true)
            {
                goSlow();
                poisonIndex = false;
            }
            if (totalCount % 3 == 0)
            {
                w.wall.Clear();
                w.CreateWall(maxWidth, maxHeight, Snake[0].X, Snake[0].Y,score);
            }
            //Creates a wall

            if (totalCount % 5 == 0)
            {
                p.clearPoison();
                p.CreatePoison(maxWidth, maxHeight, w);
            }
            //Creates a poison

            if (totalCount % 10 == 0)
            {
                b.clearBouns();
                b.CreateBouns(maxWidth, maxHeight, w);
            }

            food.CreateFood(maxWidth, maxHeight, w, p, Snake);
            //Creates new food to eat
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GameOver()
        {
            gameTimer.Stop();
            //Stops the timer
            startButton.Enabled = true;
            snapButton.Enabled = true;
            //Enable back the buttons

            if (score > h.Value)
            {
                h.Value = score;
                txtHighscore.Text = "Highscore: " + h.Value;
                h.Save("highscore.xml");
            }
            //Chacking for new highscore
        }

        
    }
  


}
