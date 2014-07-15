using System;

namespace Monaden.CSharp.Continuation
{
    class BindM<tA,tCol,tB> : ContM<tB>
    {
        private readonly Func<tA, ContM<tCol>> _f;
        private readonly Func<tA, tCol, tB> _map;
        private readonly ContM<tA> _m;

        internal BindM(Func<tA,ContM<tCol>> f, Func<tA,tCol,tB> map, ContM<tA> m)
        {
            _map = map;
            _f = f;
            _m = m;
        }

        public override void Run(Action<tB> cont)
        {
            _m.Run(a => _f(a).Run(col => cont(_map(a, col))));
        }
    }
}