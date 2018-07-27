using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ScoreWindow : Form
    {
        public ScoreWindow(Form1 form, int score)
        {
            InitializeComponent();
            this.Location = form.Location;
            scoreLabel.Text = score.ToString() + "!";
        }

        private void ScoreWindow_Load(object sender, EventArgs e)
        {
           
        }
    }
}
