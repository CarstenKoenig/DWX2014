using System;
using System.Linq;
using Monaden.CSharp.Probability;

namespace Monaden.BeispieleCSharp
{
    static class Probability
    {
        public static Verteilung<int> Würfel
        {
            get { return Verteilung.Gleichverteilt(1, 2, 3, 4, 5, 6); }
        }

        static bool MindestensZweiSechser(int[] augen)
        {
            return
                augen.Where(a => a == 6)
                     .Count() >= 2;
        }

        public static double WsMindestensZweiSechserBeiVierWürfeln()
        {
            var würfel = Würfel.TakeN(4);
            return würfel.WsEreignis(MindestensZweiSechser);
        }

        public static Verteilung<int> SummeZweierWürfel
        {
            get
            {
                return
                    from w1 in Würfel
                    from w2 in Würfel
                    select w1 + w2;
            }
        }

    }
}
