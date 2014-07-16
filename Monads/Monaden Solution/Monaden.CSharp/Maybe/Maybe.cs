using System;

namespace Monaden.CSharp.Maybe
{
    public class Maybe<T>
    {
        private readonly bool _isSome;
        private readonly T _value;

        internal Maybe(T value)
        {
            _isSome = true;
            _value = value;
        }

        internal Maybe()
        {
            _isSome = false;
        }

        public T Value
        {
            get
            {
                if (!_isSome) throw new InvalidOperationException();
                return _value;
            }
        }

        public bool IsSome
        {
            get { return _isSome; }
        }

        public bool IsNone
        {
            get { return !IsSome; }
        }

        internal Maybe<R> BindMap<C,R>(Func<T, Maybe<C>> f, Func<T, C, R> map)
        {
            if (IsNone) return new Maybe<R>();
            var m = f(Value);
            return m.IsNone ? new Maybe<R>() : new Maybe<R>(map(Value, m.Value));
        }

        internal Maybe<R> Bind<R>(Func<T, Maybe<R>> f)
        {
            return BindMap(f, (_, x) => x);
        }

        internal Maybe<R> Map<R>(Func<T, R> map)
        {
            return IsNone ? new Maybe<R>() : new Maybe<R>(map(Value));
        }
    }

    public static class Maybe
    {
        public static Maybe<T> Return<T>(this T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<R> Select<T, R>(this Maybe<T> source, Func<T, R> map)
        {
            return source.Map(map);
        }

        public static Maybe<B> SelectMany<A, B>(this Maybe<A> source, Func<A, Maybe<B>> selector)
        {
            return source.Bind(selector);
        }

        public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> source, Func<A, Maybe<B>> collectionSelector, Func<A, B, C> resultSelector)
        {
            return source.BindMap(collectionSelector, resultSelector);

        }

        public static Maybe<T> Some<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> None<T>()
        {
            return new Maybe<T>();
        }
    }
}
