using PokemonClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI_Project
{
    public class PokemonProcessor
    {
        static int maxNum = 807;
        public static async Task<PokemonModel> LoadPokemon(int pokeNumber=1)
        {
            string url = "";
            if(pokeNumber > 1)
            {
                url = $"https://pokeapi.co/api/v2/pokemon/{pokeNumber}";
            }
            else
            {
                url = "https://pokeapi.co/api/v2/pokemon/1";
            }

            //Wait for response
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                //if response is successful
                if (response.IsSuccessStatusCode)
                {
                    //Returns pokemon model
                    PokemonModel pokemon = await response.Content.ReadAsAsync<PokemonModel>();
                    if(pokeNumber > maxNum)
                    {
                        pokeNumber = 1;
                    }
                    return pokemon;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        //asynchronous method to return class of type damage relations
        public static async Task<DamageRelationModel> LoadType(string type ="normal")
        {
            string url = url = $"https://pokeapi.co/api/v2/type/{type}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    //place type weakness here
                    DamageRelationModel damage = await response.Content.ReadAsAsync<DamageRelationModel>();
                    return damage;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
