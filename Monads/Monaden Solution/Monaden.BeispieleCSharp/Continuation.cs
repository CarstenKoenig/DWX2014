using Monaden.CSharp;
using System;
using Monaden.CSharp.Continuation;

namespace Monaden.BeispieleCSharp
{
    static class Continuation
    {
        public static ContM<Unit> HelloAfter5
        {
            get
            {
                var res =
                    from t1 in ContM.Delay(TimeSpan.FromSeconds(2))
                    from t2 in ContM.Delay(TimeSpan.FromSeconds(3))
                    select Unit.V;

                return res.Select(x => { Console.WriteLine("Hello"); return x; });
            }
        }
    }
}
