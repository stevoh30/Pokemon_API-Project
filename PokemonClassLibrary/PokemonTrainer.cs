using PokemonAPI_Project;
using System;
using System.Collections.Generic;
using System.Text;


namespace PokemonClassLibrary
{
    public class PokemonTrainer
    {
        public List<PokemonModel> Pokemon { get; set; }
        public int WinsStreak { get; set; }
        public bool Win { get; set; }

        public PokemonTrainer()
        {
            Pokemon = new List<PokemonModel>();
            WinsStreak = 0;
            Win = false;
        }
        public void Reset()
        {
            Pokemon.Clear();
            WinsStreak = 0;
            Win = false;
        }
    }
   
}
