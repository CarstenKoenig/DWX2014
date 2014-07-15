using System;

namespace Monaden.CSharp.Continuation
{
	public abstract class ContM<T>
	{
		public abstract void Run(Action<T> cont);
	}


	public static class ContM
	{
		public static ContM<T> Return<T>(this T value)
		{
			return new ReturnM<T>(value);
		}

		public static ContM<tB> BindMap<tA,tCol,tB>(this ContM<tA> m, Func<tA,ContM<tCol>> f, Func<tA,tCol,tB> map)
		{
			return new BindM<tA,tCol,tB>(f, map, m);
		}

		public static ContM<tB> Bind<tA, tB>(this ContM<tA> m, Func<tA, ContM<tB>> f)
		{
			return new BindM<tA,tB,tB>(f, (_, x) => x, m);
		}

		public static ContM<tR> SelectMany<tS, tR>(this ContM<tS> source, Func<tS, ContM<tR>> selector)
		{
			return source.Bind(selector);
		}

		public static ContM<tR> SelectMany<tS, tCol, tR>(this ContM<tS> source, Func<tS, ContM<tCol>> collectionSelector, Func<tS, tCol, tR> resultSelector)
		{
			return source.BindMap(collectionSelector, resultSelector);

		}

		public static ContM<TResult> Select<TSource, TResult>(this ContM<TSource> source, Func<TSource, TResult> selector)
		{
			return new MapM<TSource, TResult>(selector, source);
		}

		public static ContM<Unit> Delay(TimeSpan span)
		{
			return new DelayM(span);
		}
	}

}
