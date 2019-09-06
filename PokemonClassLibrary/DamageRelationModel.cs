using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonClassLibrary
{
    public class DamageRelationModel
    {
        public DamageRelations damage_relations { get; set; }
    }
    public class DamageRelations
    {
        public List<DoubleDamageFrom> double_damage_from { get; set; }
        public List<HalfDamageTo> half_damage_to { get; set; }
        public List<object> no_damage_from { get; set; }
        public List<object> no_damage_to { get; set; }
    }
    public class DoubleDamageFrom
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class HalfDamageTo
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class NoDamageFrom
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class NoDamageTo
    {
        public string name { get; set; }
        public string url { get; set; }
    }
}
