using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace NewShooter
{
    public partial class Form1 : Form
    {
        PictureBox[] stars;
        int backgroundspeed;
        int playerSpeed;
        Random rnd;

        PictureBox[] munitions;
        int MunitionSpeed;

        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;
        WindowsMediaPlayer explosion;

        PictureBox[] enemies;
        int enemiSpeed;

        PictureBox[] enemiesMunition;
        int enemiesMunitionSpeed;

        int score;
        int level;
        int difficulty;
        bool pause;
        bool gameIsOver;


        public Form1()
        {
            InitializeComponent();
            MoveBgTimer.Tick += MoveBgTimer_Tick;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            score = 0;
            level = 1;
            pause = false;
            gameIsOver = false;
            difficulty = 9;


            backgroundspeed = 4;
            playerSpeed = 4;
            stars = new PictureBox[20];
            rnd = new Random();
            munitions = new PictureBox[3];
            MunitionSpeed = 20;

            Image munition = Image.FromFile(@"asserts\munition.png");

            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                munitions[i].Visible = false;
                this.Controls.Add(munitions[i]);
            }


            for (int i = 0; i < stars.Length; i++)
            {

                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }
                this.Controls.Add(stars[i]);
            }

            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;
            explosion.settings.volume = 6;
            gameMedia.controls.play();

            enemies = new PictureBox[10];
            enemiSpeed = 4;
            Image enemi1 = Image.FromFile(@"asserts\E1.png");
            Image enemi2 = Image.FromFile(@"asserts\E2.png");
            Image enemi3 = Image.FromFile(@"asserts\E3.png");
            Image boss1 = Image.FromFile(@"asserts\Boss1.png");
            Image boss2 = Image.FromFile(@"asserts\Boss2.png");

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemi2;
            enemies[2].Image = enemi3;
            enemies[3].Image = enemi3;
            enemies[4].Image = enemi1;
            enemies[5].Image = enemi3;
            enemies[6].Image = enemi2;
            enemies[7].Image = enemi3;
            enemies[8].Image = enemi2;
            enemies[9].Image = boss2;

            enemiesMunition = new PictureBox[10];
            enemiesMunitionSpeed = 4;

            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i]=new PictureBox();
                enemiesMunition[i].Size = new Size(2, 25);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemiesMunition[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemiesMunition[i]);
            }

        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                // Move each star downward
                stars[i].Top += backgroundspeed;

                // If a star goes below the bottom of the screen, reset it to the top
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;

                }
            }
            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed - 2;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;

                }
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 400)
            {
                Player.Left += playerSpeed; 
            }

        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed; // Fixed: Changed -= to +=
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if (!gameIsOver)
                {
                    if (pause)
                    {
                        StartTimers();
                        label.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label.Location = new Point(this.Width / 2 - 120, 150);
                        label.Text = "PAUSED";
                        label.Visible = true;
                        gameMedia.controls.play();
                        StopTimers();
                        pause= true;
                    }
                }
            }
        }

        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            shootgMedia.controls.play();
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= MunitionSpeed;

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }

        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiSpeed);
        }

        private void MoveEnemies(PictureBox[] array,int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        private void Collision()
        {
            for (int i=0;i<enemies.Length;i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds)
                    || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) 
                    || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds)
                    )
                {
                    explosion.controls.play();

                    score += 1;
                    scorelbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString();
                    if (score%3 == 0)
                    {
                        level += 1;
                        levellbl.Text = (level<10)? "0" + level.ToString() : level.ToString();

                        if (enemiSpeed<=10 && enemiesMunitionSpeed<=10 && difficulty>=0)
                        {
                            difficulty--;
                            enemiSpeed++;
                            enemiesMunitionSpeed++;
                        }

                        if (level == 10)
                        {
                            GameOver("NICE DONE");
                        }
                    }

                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void GameOver(String str)
        {
            label.Text = str;
            label.Location = new Point(120, 120);
            label.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;

            gameMedia.controls.stop();
            StopTimers();
        }

        private void StopTimers()
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemiesMunitionTimer.Stop();
        }

        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
            EnemiesMunitionTimer.Start();
        }

        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0;i<(enemiesMunition.Length-difficulty);i++)
            {
                if (enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMunitionSpeed;

                    CollisionWithEnemiesMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemiesMunition[i].Location=new Point(enemies[x].Location.X + 20, enemies[x].Location.Y+30);
                }
            }
        }

        private void CollisionWithEnemiesMunition()
        {
            for (int i=0;i<enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible=false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        
    }
}
