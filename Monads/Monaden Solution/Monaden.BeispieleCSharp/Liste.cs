using System;
using System.Collections.Generic;
using System.Linq;

namespace Monaden.BeispieleCSharp
{
    public static class Liste
    {

        public struct Outfit
        {
            public string Hemd { get; set; }
            public string Hose { get; set; }
            public string Schuh { get; set; }
        }

        public static IEnumerable<Outfit> Outfits()
        {
            return
                from schuhe in Schuhe
                from hose in Hosen
                from hemd in Hemden
                select new Outfit {Schuh = schuhe, Hose = hose, Hemd = hemd};
        }

        public static IEnumerable<string> Hemden
        {
            get { return new[] { "Polo", "Hawai", "Smoking" }; }
        }

        public static IEnumerable<string> Hosen
        {
            get { return new[] {"Jeans", "Chino", "Cord"}; }
        }

        public static IEnumerable<string> Schuhe
        {
            get { return new[] { "Galoschen", "Sneakers", "Jogginschuhe" }; }
        }
    }
}