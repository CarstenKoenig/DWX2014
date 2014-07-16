using System;

namespace Monaden.CSharp.Continuation
{
    class ReturnM<T> : ContM<T>
    {
        private readonly T _withValue;

        internal ReturnM(T withValue)
        {
            _withValue = withValue;
        }

        public override void Run(Action<T> cont)
        {
            cont(_withValue);
        }
    }
}