using System;

namespace Monaden.CSharp.Continuation
{
    class MapM<tA, tB> : ContM<tB>
    {
        private readonly Func<tA, tB> _map;
        private readonly ContM<tA> _m;

        internal MapM(Func<tA, tB> map, ContM<tA> m)
        {
            _map = map;
            _m = m;
        }

        public override void Run(Action<tB> cont)
        {
            _m.Run(a => cont(_map(a)));
        }
    }
}