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
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Food food = new Food();
        private Wall w = new Wall();


        public int maxWidth;
        public int maxHeight;
        public int score;
        public int highscore;
        
    
        Random rand = new Random();

        bool goLeft, goRight, goUp, goDown;
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
            caption.Text = "I scored " + score + " and my highscore is " + highscore + " on the classic snake game";
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
            //Save the file

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
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            //Settings.direction = e.Keycode();
            if (goLeft)
            {
                Settings.direction = "left";
            }
            if (goRight)
            {
                Settings.direction = "right";
            }
            if (goDown)
            {
                Settings.direction = "down";
            }
            if (goUp)
            {
                Settings.direction = "up";
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
                    //If the snake head and the food in the same x,y we call the EatFood function

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }
                    //If the snake head and body are in the same x,y we call the GameOver function
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
            
            Brush snakeColor;
            Brush wallColor;
            //color the snake head and the body

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColor = Brushes.Black;
                }
                else
                {
                    snakeColor = Brushes.DarkGreen;
                }
                //Coloring the head and the body of the snake

                canvas.FillEllipse(snakeColor, new Rectangle
                    (Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height

                    ));
                //Fills the color inside the head or the body circle 
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                (food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width, Settings.Height
                ));
            //Fills the color inside the food cricle

            wallColor = Brushes.Purple;
            for (int i = 0; i < w.wall.Count; i++)
            {

                canvas.FillEllipse(wallColor, new Rectangle
                   (w.wall[i].X * Settings.Width,
                   w.wall[i].Y * Settings.Height,
                   Settings.Width, Settings.Height

                   ));

            }
            //Fills the color of the wall
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

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;
            //Marking the edge of the canvas 

            Snake.Clear();
            //Clears the snake list
            w.wall.Clear();
            //Clears the wll list 

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

            //w.CreateWall(maxWidth,maxHeight);

            food.CreateFood(maxWidth, maxHeight);
           
            //Creating the first food circle

            gameTimer.Start();
            //Starting the timer
        }

        private void EatFood()
        {
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
            
            if (score % 3 == 0)
            {
                if (w.Active == true)
                {
                    w.wall.Clear();
                    w.CreateWall(maxWidth, maxHeight);
                }
                else
                {
                    w.CreateWall(maxWidth, maxHeight);
                }

            }
            if (w.Active == true)
            {
                food.CreateFood(maxWidth, maxHeight,w);
            }
            food.CreateFood(maxWidth, maxHeight);
            

            //Creates new food to eat
        }

        private void GameOver()
        {
            gameTimer.Stop();
            //Stops the timer
            startButton.Enabled = true;
            snapButton.Enabled = true;
            //Enable back the buttons

            if (score > highscore)
            {
                highscore = score;
                txtHighscore.Text = "High Score: " + Environment.NewLine + highscore;
                txtHighscore.ForeColor = Color.Maroon;
                txtHighscore.TextAlign = ContentAlignment.MiddleCenter;
            }
            //Update the high score if needed
        }
    }
  


}
