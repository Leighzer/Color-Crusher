using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            this.scoreLabel.Text = "0";
            this.KeyPreview = true;

            k1 = 's';
            k2 = 'd';
            k3 = 'f';

            keyToColor.Add(k1, Color.Red);
            keyToColor.Add(k2, Color.Blue);
            keyToColor.Add(k3, Color.Green);

            currentColor = keyToColor.Values.ToList()[random.Next(3)];
            setColor();
            preTimeLabel.Text = "Press 'a' to start game";
            inputHandler = inactiveGameInputHandler;
            backgroundPlayer = new SoundPlayer(WindowsFormsApplication1.Properties.Resources.Color_Crush_Background);


        }
        
        private SoundPlayer backgroundPlayer = new SoundPlayer();
        private int score = 0;
        private double time;
        private Color currentColor;
        private Dictionary<char, Color> keyToColor = new Dictionary<char, Color>();
        private char k1, k2, k3;
        Random random = new Random();
        public delegate void inputHandleFunction(char c);
        public inputHandleFunction inputHandler;

        static double defaultTime = 60d;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char charPressed = e.KeyChar;

            inputHandler(charPressed);
        }

        private void loopBackgroundMusic()
        {
            backgroundPlayer.PlayLooping();
        }

        private void stopBackgroundMusic()
        {
            backgroundPlayer.Stop();
        }

        private void setColor()
        {
            this.BackColor = currentColor;//set the form back color to our internally tracked color
        }

        private void displayScore()
        {
            this.scoreLabel.Text = score.ToString();//set score text label to our internally stored int value
        }
        
        private void subtractScore()
        {
            score -= 3;
            if (score < 0)
            {
                score = 0;
            }
            displayScore();
        }

        private void resetScore()
        {
            score = 0;
            displayScore();
        }

        private void displayTime()
        {
            this.timeLabel.Text = time.ToString();//set time label to amount of current amount of time left in round
        }

        private void resetTime()
        {
            time = defaultTime;
            displayTime();
        }
        
        private void startGame()
        {
            loopBackgroundMusic();
            timer.Enabled = true;
            inputHandler = activeGameInputHandler;
            preTimeLabel.Text = "Time:";
            resetTime();
            timer.Start();
        }

        private void endGame()
        {
            stopBackgroundMusic();
            ScoreWindow scoreWindow = new ScoreWindow(this, score);
            scoreWindow.ShowDialog();
            inputHandler = inactiveGameInputHandler;
            preTimeLabel.Text = "Press 'a' to start game";
            timeLabel.Text = "";
            resetScore();
        }

        private void resetGame()
        {
            stopBackgroundMusic();
            timer.Stop();
            timer.Enabled = false;
            inputHandler = inactiveGameInputHandler;
            preTimeLabel.Text = "Press 'a' to start game";
            timeLabel.Text = "";
            resetScore();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
            if (time <= 0)
            {
                timer.Stop();
                timer.Enabled = false;
                endGame();
                inputHandler = null;
                inputHandler = inactiveGameInputHandler;
                return;
            }
            this.time -= 1d;
            displayTime();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetGame();
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreditWindow creditWindow = new CreditWindow();
            creditWindow.ShowDialog();
        }

        private void inactiveGameInputHandler(char c)
        {

            if (c == 'a')
            {
                startGame();
            }
            else
            {
                //
            }

        }

        private void activeGameInputHandler(char c)
        {
            if (keyToColor.ContainsKey(c))//check if the pressed key is associated with a color
            {
                Color colorPressed = keyToColor[c];
                if (colorPressed.Equals(currentColor))//check if the color matches current background color
                {
                    //success
                    //increment score
                    score++;
                    displayScore();


                    //generate new color, that isn't the same as we just pushed, randomly
                    List<Color> keyToColorValues = keyToColor.Values.ToList();
                    bool haveNewColor = false;
                    while (!haveNewColor)
                    {
                        int randomIndex = random.Next(keyToColorValues.Count);
                        if (!keyToColorValues[randomIndex].Equals(currentColor))//check if random color is not the same as color currently being displayed
                        {
                            currentColor = keyToColorValues[randomIndex];
                            setColor();
                            haveNewColor = true;//successfully set a new different color
                        }
                        else
                        {
                            keyToColorValues.Remove(keyToColorValues[randomIndex]);//randomly picked our current color... remove it from the random selection list.
                        }


                    }


                }
                else
                {
                    //don't change color
                    subtractScore();//subtract score, wrong key pressed
                }
            }
            else
            {
                // pressing a key we don't care about, don't do anything
            }
        }



    }
}
