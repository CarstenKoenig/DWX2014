using System;

namespace Monaden.BeispieleCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Continuation.HelloAfter5.Run(_ => { });

            State.PrintSquares();

            Console.WriteLine("Verteilung von Summe zweier Würfel: {0}", Probability.SummeZweierWürfel);
            Console.WriteLine("WS mindestens zwei 6-er bei 4 Würfel: {0:0.0}%", Probability.WsMindestensZweiSechserBeiVierWürfeln() * 100.0);


            Console.WriteLine("Return zum Beenden");
            Console.ReadLine();
        }
    }
}
