using System;

namespace Monaden.CSharp.Continuation
{
    class DelayM : ContM<Unit>
    {
        private TimeSpan _span;

        internal DelayM(TimeSpan span)
        {
            _span = span;
        }

        public override void Run(Action<Unit> cont)
        {
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer(
                _ =>
                {
                    timer.Dispose();
                    cont(Unit.V);
                }, null, (int)_span.TotalMilliseconds, System.Threading.Timeout.Infinite);
        }

    }
}