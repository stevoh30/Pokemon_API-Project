using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonClassLibrary;
using System.Threading;

namespace PokemonAPI_Project
{

    public partial class Form1 : Form
    {
        //Saved trainer pokemon for win screen
        public static PokemonTrainer trainer1 = new PokemonTrainer();
        public static PokemonTrainer trainer2 = new PokemonTrainer();

        //Temporary lists for methods
        List<PokemonModel> BattlePokemon = new List<PokemonModel>();
        List<DamageRelationModel> PokemonDamage1 = new List<DamageRelationModel>();
        List<DamageRelationModel> PokemonDamage2 = new List<DamageRelationModel>();

        // Global variables used in various methods
        static int count = 0;
        public static int pokemonbattlecount = 0;
        static int pokemonwinner = 0;

        public Form1()
        {
            InitializeComponent();

            // Initialize Client to call to browser
            ApiHelper.InitializeClient();
        }
        private async Task LoadPokemon(int num, int trainerNum)
        {
            //Returns pokemon from API
            var pokemon = await PokemonProcessor.LoadPokemon(num);
            //Displays pokemon information
            try
            {
                if (trainerNum == 1)
                {
                    //Capitalize the first letter in the name 
                    lblPokemon1.Text = pokemon.name.First().ToString().ToUpper() + pokemon.name.Substring(1);
                    pictureBox1.Load(pokemon.sprites.back_default);
                }
                else
                {
                    lblPokemon2.Text = pokemon.name.First().ToString().ToUpper() + pokemon.name.Substring(1);
                    pictureBox2.Load(pokemon.sprites.front_default);
                }
            }
            catch
            {
                MessageBox.Show("There was an error when retrieving image");

            }
            //Adds pokemon to list
            BattlePokemon.Add(pokemon);
        }
        private async Task LoadType(string type)
        {
            try
            {
                var damageRelation = await PokemonProcessor.LoadType(type);
                PokemonDamage1.Add(damageRelation);
            }
            catch
            {
                MessageBox.Show("There was an error when loading pokemon type");
            }
        }

        private async void btnRandomize_Click(object sender, EventArgs e)
        {
            //Clears previous pokemon battle
            BattlePokemon.Clear();

            //Collects random pokemon based off Random()
            Random rand = new Random();
            int num = rand.Next(807);
            int num2 = rand.Next(807);
            await LoadPokemon(num, 1);
            await LoadPokemon(num2, 2);

            // Announcer message box to display types
            MessageBox.Show("An exciting matchup between " + BattlePokemon[0].name.First().ToString().ToUpper() + BattlePokemon[0].name.Substring(1) +
                " and " + BattlePokemon[1].name.First().ToString().ToUpper() + BattlePokemon[1].name.Substring(1) + "!" + "\n\n" +
                ReturnPokemonType(0) + " vs " + ReturnPokemonType(1) + "!", "Pokemon Announcer : ");
            btnBattle.Text = "ANALYZE";
            btnBattle.Enabled = true;
        }

        // Method displays pokemon type
        private string ReturnPokemonType(int num)
        {
            //Message box to display pokemon types
            int count = 0;
            string type = "";
            foreach (var s in BattlePokemon[num].types)
            {
                if (count == 1)
                    type += "/";
                type += s.type.name;
                count++;
            }
            return type;
        }

        // Load damage type relations into a list
        private async void btnBattle_Click(object sender, EventArgs e)
        {
            if (btnBattle.Text == "ANALYZE")
            {
                PokemonDamage1.Clear();
                PokemonDamage2.Clear();

                await LoadPokemonType(0);
                await LoadPokemonType(1);

                AnalyzeMethod();
            }
            else
            {
                trainer1.Pokemon.Add(BattlePokemon[0]);
                trainer2.Pokemon.Add(BattlePokemon[1]);

                //switch to form2 for winner
                BattleMethod();
                //Add pokemon to trainer class
                btnBattle.Enabled = false;
            }
        }
        private async Task LoadPokemonType(int num)
        {
            foreach (var t in BattlePokemon[num].types)
            {
                if (num == 0)
                    PokemonDamage1.Add(await PokemonProcessor.LoadType(t.type.name));
                else
                    PokemonDamage2.Add(await PokemonProcessor.LoadType(t.type.name));
            }
        }
        private void AnalyzeMethod()
        {
            count = 0;
            foreach (DamageRelationModel d in PokemonDamage1)
            {
                foreach (var h in d.damage_relations.double_damage_from)
                {
                    foreach (var b in BattlePokemon[1].types)
                        if (b.type.name == h.name)
                            count++;
                }
            }
            foreach (DamageRelationModel d in PokemonDamage2)
            {
                foreach (var h in d.damage_relations.double_damage_from)
                {
                    foreach (var b in BattlePokemon[0].types)
                        if (b.type.name == h.name)
                            count--;
                }
            }
            //checks count to determine pokemon advantage/disadvantage
            switch (count)
            {
                case -2:
                    MessageBox.Show("Ouch, " + BattlePokemon[1].name + " is at a major disadvantage!", "Announcer : ");
                    break;
                case -1:
                    MessageBox.Show("Uh oh, " + BattlePokemon[1].name + " is at a disadvantage!", "Announcer : ");
                    break;
                case 0:
                    MessageBox.Show("Yes! " + BattlePokemon[0].name + " and " + BattlePokemon[1].name + " are an even matchup!", "Announcer : ");
                    break;
                case 1:
                    MessageBox.Show("Uh oh, " + BattlePokemon[0].name + " is at a disadvantage!", "Announcer : ");
                    break;
                case 2:
                    MessageBox.Show("Ouch, " + BattlePokemon[0].name + " is at a major disadvantage!", "Announcer : ");
                    break;
            }
            btnBattle.Text = "BATTLE!";
        }
        private void BattleMethod()
        {
            pokemonbattlecount++;

            int health1 = 10;
            int health2 = 10;
            Random damage = new Random();

            while (health1 >= 0 && health2 >= 0)
            {
                health1 = health1 - damage.Next(10) - count;
                health2 = health2 - damage.Next(10) + count;
            }
            if (health1 > health2)
            {
                MessageBox.Show(BattlePokemon[1].name + " has been defeated!", "Announcer :");
                pictureBox2.Image = PokemonAPI_Project.Properties.Resources.red_x;
                trainer1.WinsStreak += 1;
                pokemonwinner = 1;
            }
            else if (health2 > health1)
            {
                MessageBox.Show(BattlePokemon[0].name + " has been defeated!", "Announcer :");
                pictureBox1.Image = PokemonAPI_Project.Properties.Resources.red_x;
                trainer2.WinsStreak += 1;
                pokemonwinner = 2;
            }
            else
            {
                MessageBox.Show("What an amazing fight! It looks like it is a draw folks!", "Announcer :");
                pokemonwinner = 0;
            }
            switch (pokemonbattlecount)
            {
                case 1:
                    ScoreBoardMethod(pictureBoxRed1, pictureBoxBlue1);
                    break;
                case 2:
                    ScoreBoardMethod(pictureBoxRed2, pictureBoxBlue2);
                    break;
                case 3:
                    ScoreBoardMethod(pictureBoxRed3, pictureBoxBlue3);
                    break;
            }
            //Final round, tally up points and switch to form2
            if (trainer1.WinsStreak ==2 || trainer2.WinsStreak ==2|| pokemonbattlecount ==3)
            {
                if (trainer1.WinsStreak> trainer2.WinsStreak)
                {
                    trainer1.Win = true;
                }
                else if (trainer2.WinsStreak > trainer1.WinsStreak)
                {
                    trainer2.Win = true;
                }
                else{
                    MessageBox.Show("It's a tie! The two trainers are evenly matched!");
                }

                // Reset data
                ResetVariables();
                Form_Win f2 = new Form_Win();
                f2.Show();
            }
        }
        private void ScoreBoardMethod(PictureBox p1, PictureBox p2)
        {
            if (pokemonwinner == 1)
            {
                p1.Image = PokemonAPI_Project.Properties.Resources.check1;
                p2.Image = PokemonAPI_Project.Properties.Resources.red_x;

            }
            else if (pokemonwinner == 2)
            {
                p1.Image = PokemonAPI_Project.Properties.Resources.red_x;
                p2.Image = PokemonAPI_Project.Properties.Resources.check1;
            }
            else
            {
                p1.Image = PokemonAPI_Project.Properties.Resources.red_x;
                p2.Image = PokemonAPI_Project.Properties.Resources.red_x;
            }
        }
        private void ResetVariables()
        {
            //Clear form 1 variables and objects
            BattlePokemon.Clear();
            PokemonDamage1.Clear();
            PokemonDamage2.Clear();
            pictureBoxBlue1.Image = null;
            pictureBoxBlue2.Image = null;
            pictureBoxBlue3.Image = null;
            pictureBoxRed1.Image = null;
            pictureBoxRed2.Image = null;
            pictureBoxRed3.Image = null;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            lblPokemon1.Text = null;
            lblPokemon2.Text = null;
            count = 0;
        }
    }
}


//dynamically change image loading
//add pokedex
//fix image loading issues