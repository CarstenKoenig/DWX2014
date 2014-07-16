using System;

namespace Monaden.CSharp.State
{
    public class StateM<S,T>
    {
        readonly Func<S, Tuple<T, S>> _runState;

        internal StateM(Func<S,Tuple<T,S>> runState)
        {
            _runState = runState;
        }

        public Tuple<T,S> Run(S state)
        {
            return _runState(state);
        }

        public T Eval(S state)
        {
            return Run(state).Item1;
        }

        public S Process(S state)
        {
            return Run(state).Item2;
        }

        internal StateM<S,R> BindMap<C,R>(Func<T,StateM<S,C>> f, Func<T,C,R> map)
        {
            return new StateM<S, R>(s =>
            {
                var t1 = _runState(s);
                var m2 = f(t1.Item1);
                var t2 = m2.Run(t1.Item2);
                return Tuple.Create(map(t1.Item1, t2.Item1), t2.Item2);
            });
        }

        internal StateM<S, R> Bind<R>(Func<T, StateM<S, R>> f)
        {
            return BindMap(f, (_, x) => x);
        }

        internal StateM<S,R> Map<R>(Func<T, R> map)
        {
            return new StateM<S,R>(s => {
                var t = Run(s);
                return Tuple.Create(map(t.Item1), t.Item2);
            });
        }

    }

    public static class StateM
    {
        public static StateM<S, T> Return<S, T>(this T value)
        {
            return new StateM<S, T>(s => Tuple.Create(value, s));
        }

        public static StateM<S,R> Select<S,T,R>(this StateM<S,T> source, Func<T,R> map)
        {
            return source.Map(map);
        }

        public static StateM<S,B> SelectMany<S, A, B>(this StateM<S, A> source, Func<A, StateM<S, B>> selector)
        {
            return source.Bind(selector);
        }

        public static StateM<S,C> SelectMany<S, A, B, C>(this StateM<S, A> source, Func<A, StateM<S, B>> collectionSelector, Func<A, B, C> resultSelector)
        {
            return source.BindMap(collectionSelector, resultSelector);

        }

        public static StateM<S, S> Get<S>()
        {
            return new StateM<S, S>(s => Tuple.Create(s, s));
        }

        public static StateM<S, Unit> Set<S>(this S state)
        {
            return new StateM<S, Unit>(_ => Tuple.Create(Unit.V, state));
        }

        public static Func<A, StateM<S, C>> Comp<S, A, B, C>(this Func<A, StateM<S, B>> f, Func<B, StateM<S, C>> g)
        {
            return a => f(a).Bind(g);
        }

    }
}
