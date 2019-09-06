using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokemonAPI_Project
{
    public partial class Form_Win : Form
    {
        public Form_Win()
        {
            InitializeComponent();
            InitializeForm();
        }
        private void InitializeForm()
        {
            if(Form1.trainer1.Win == true)
            {
                this.BackColor = Color.Red;
                label1.Text = "Red Team WINS!";
                pictureBox1.Load(Form1.trainer1.Pokemon[0].sprites.front_default);
                pictureBox2.Load(Form1.trainer1.Pokemon[1].sprites.front_default);
                if(Form1.pokemonbattlecount == 3)
                pictureBox3.Load(Form1.trainer1.Pokemon[2].sprites.front_default);
            }
            else if(Form1.trainer2.Win == true)
            {
                this.BackColor = Color.Blue;
                label1.Text = "Blue Team WINS!";
                pictureBox1.Load(Form1.trainer2.Pokemon[0].sprites.front_default);
                pictureBox2.Load(Form1.trainer2.Pokemon[1].sprites.front_default);
                if(Form1.pokemonbattlecount==3)
                pictureBox3.Load(Form1.trainer2.Pokemon[2].sprites.front_default);
            }
        }

        private void btnNewBattle_Click(object sender, EventArgs e)
        {
            //Closes form2 and resets form1 variables
            Form1.trainer1.Reset();
            Form1.trainer2.Reset();
            Form1.pokemonbattlecount = 0;
            this.Close();
        }
    }
}
