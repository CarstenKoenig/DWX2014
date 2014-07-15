using System;
using Monaden.CSharp.State;

namespace Monaden.BeispieleCSharp
{
    static class State
    {
        public static StateM<int, int> Incr
        {
            get
            {
                return
                    from nr in StateM.Get<int>()
                    from _ in StateM.Set(nr + 1)
                    select nr;
            }
        }

        public static StateM<int, int> NextSquare
        {
            get
            {
                return
                    from nr in Incr
                    select nr*nr;
            }
        }

        public static void PrintSquares()
        {
            var squares =
                from a in NextSquare
                from b in NextSquare
                from c in NextSquare
                from d in NextSquare
                select Tuple.Create(a,b,c,d);

            var tuples = squares.Eval(0);
            Console.WriteLine("a={0}; b={1}; c={2}; d={3}", tuples.Item1, tuples.Item2, tuples.Item3, tuples.Item4);
        }
    }
}
