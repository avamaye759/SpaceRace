using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Rectangle p1 = new Rectangle(200, 450, 20, 30);
        Rectangle p2 = new Rectangle(350, 450, 20, 30);
        int playerSpeed = 6;

        List<Rectangle> asteroids = new List<Rectangle>();
        List<int> astSpeed = new List<int>();
        int astWidth = 8;
        int astHeight = 3;

        bool upDown = false;
        bool downDown = false;
        bool wDown = false;
        bool sDown = false;

        int p1Score = 0;
        int p2Score = 0;

        int time = 3000;

        string gameState = "wait";

        SolidBrush astBrush = new SolidBrush(Color.White);
        SolidBrush p1Brush = new SolidBrush(Color.Lime);
        SolidBrush p2Brush = new SolidBrush(Color.DeepPink);

        Random ranGen = new Random();
        int ranValue = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public void GameSetup()
        {
            gameState = "run";

            titleLabel.Text = "";
            subtitleLabel.Text = "";
            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";
            timeLabel.Text = $"{time}";

            titleLabel.Visible = false;
            subtitleLabel.Visible = false;
            p1ScoreLabel.Visible = true;
            p2ScoreLabel.Visible = true;
            timeLabel.Visible = true;

            gameTimer.Enabled = true;

            p1Score = 0;
            p2Score = 0;
            p1.X = 170;
            p1.Y = 450;
            p2.X = 400;
            p2.Y = 450;

            asteroids.Clear();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (gameState != "run")
                    {
                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //start timer countdown
            time--;
            timeLabel.Text = $"{time}";

            //move players
            if (upDown == true && p2.Y > 0)
            {
                p2.Y -= playerSpeed;
            }

            if (downDown == true && p2.Y < this.Height - p2.Height)
            {
                p2.Y += playerSpeed;
            }

            if (wDown == true && p1.Y > 0)
            {
                p1.Y -= playerSpeed;
            }

            if (sDown == true && p1.Y < this.Height - p1.Height)
            {
                p1.Y += playerSpeed;
            }

            //move ball objects
            for (int i = 0; i < asteroids.Count; i++)
            {
                int x = asteroids[i].X + astSpeed[i];
                asteroids[i] = new Rectangle(x, asteroids[i].Y, astWidth, astHeight);
            }

            ranValue = ranGen.Next(1, 101);

            //generate new asteroid if it is time
            if (ranValue < 10)
            {
                asteroids.Add(new Rectangle(600, ranGen.Next(0, 401), astWidth, astHeight));
                astSpeed.Add(-7);
            }
            
            //check for collision between any asteroid and players
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (p1.IntersectsWith(asteroids[i]))
                {
                    asteroids.RemoveAt(i);
                    astSpeed.RemoveAt(i);
                    p1.X = 170;
                    p1.Y = 450;
                }

                if (p2.IntersectsWith(asteroids[i]))
                {
                    asteroids.RemoveAt(i);
                    astSpeed.RemoveAt(i);
                    p2.X = 400;
                    p2.Y = 450;
                }
            }

            //remove asteroid if it goes off the screen
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (asteroids[i].X == 0)
                {
                    asteroids.RemoveAt(i);
                    astSpeed.RemoveAt(i);
                }
            }

            //check if a player has reached other side
            if (p1.Y == 0)
            {
                p1Score++;
                p1ScoreLabel.Text = $"{p1Score}";
                p1.X = 170;
                p1.Y = 450;
            }

            if (p2.Y == 0)
            {
                p2Score++;
                p2ScoreLabel.Text = $"{p2Score}";
                p2.X = 400;
                p2.Y = 450;
            }

            //check to see if timer has run out; if so, announce winner
            if (time == 0)
            {
                if (p1Score > p2Score)
                {
                    gameState = "p1Win";
                }
                else if (p1Score < p2Score)
                {
                    gameState = "p2Win";
                }
                else if (p1Score == p2Score)
                {
                    gameState = "tie";
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "wait")
            {
                titleLabel.Text = "SPACE RACE";
                subtitleLabel.Text = "Press enter to start or esc to exit";

                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
                timeLabel.Visible = false;
            }
            else if (gameState == "run")
            {
                //draw players
                e.Graphics.FillRectangle(p1Brush, p1);
                e.Graphics.FillRectangle(p2Brush, p2);

                //draw asteroids
                for (int i = 0; i < asteroids.Count(); i++)
                {
                    e.Graphics.FillRectangle(astBrush, asteroids[i]);
                }
            }
            else if (gameState == "p1Win")
            {
                titleLabel.Text = "Player 1 Wins";
                subtitleLabel.Text = "Press enter to play again or esc to exit";

                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
                timeLabel.Visible = false;

                gameTimer.Enabled = false;
            }
            else if (gameState == "p2Win")
            {
                titleLabel.Text = "Player 2 Wins";
                subtitleLabel.Text = "Press enter to play again or esc to exit";

                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
                timeLabel.Visible = false;

                gameTimer.Enabled = false;
            }
            else if (gameState == "tie")
            {
                titleLabel.Text = "Tie";
                subtitleLabel.Text = "Press enter to play again or esc to exit";

                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
                timeLabel.Visible = false;

                gameTimer.Enabled = false;
            }
        }
    }
}
