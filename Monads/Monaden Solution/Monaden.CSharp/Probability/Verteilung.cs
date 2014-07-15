using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monaden.CSharp.Probability
{
	public delegate bool Ereignis<T>(T wert);

	public class Verteilung<T>
	{
		private readonly Dictionary<T,double> _verteilung;

		internal Verteilung(IEnumerable<Tuple<T, double>> verteilung)
		{
			_verteilung = new Dictionary<T, double>();
			foreach (var t in verteilung)
			{
				var p = _verteilung.ContainsKey(t.Item1) ? _verteilung[t.Item1] : 0.0;
				_verteilung[t.Item1] = p + t.Item2;
			}

			var sum = _verteilung.Sum(kvp => kvp.Value);
			if (Math.Abs(1.0 - sum) > 0.01) throw new Exception("invalid Verteilung");
		}

		internal Verteilung(params Tuple<T, double>[] verteilung)
			: this ((IEnumerable<Tuple<T, double>>)verteilung)
		{
		}

		private IEnumerable<Tuple<tR, double>> Join<tR>(Func<T, Verteilung<tR>> f)
		{
			return JoinMap(f, (_,x) => x);
		}

		private IEnumerable<Tuple<tR, double>> JoinMap<tCol, tR>(Func<T, Verteilung<tCol>> f, Func<T,tCol,tR> map)
		{
			foreach (var kvp in _verteilung)
				foreach (var kvp2 in f(kvp.Key)._verteilung)
					yield return Tuple.Create(map(kvp.Key, kvp2.Key), kvp.Value * kvp2.Value);
		}

		internal Verteilung<tR> Bind<tR>(Func<T, Verteilung<tR>> f)
		{
			return new Verteilung<tR>(this.Join(f));
		}

		public Verteilung<tR> BindMap<tCol, tR>(Func<T, Verteilung<tCol>> f, Func<T, tCol, tR> map)
		{
			return new Verteilung<tR>(this.JoinMap(f, map));
		}


		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach (var t in _verteilung.OrderByDescending(t => t.Value))
				builder.AppendFormat("\n{0}: {1:0.00}%", t.Key, t.Value * 100.0);
			if (builder.Length == 0)
				return "--Unmöglich--";

			return builder.ToString();
		}

		internal Double WsEreignis(Ereignis<T> ereignis)
		{
			return
				_verteilung.Where(kvp => ereignis(kvp.Key))
						   .Sum(kvp => kvp.Value);
		}
	}

	public static class Verteilung
	{
		public static Verteilung<T> Sicher<T>(T v)
		{
			return new Verteilung<T>(Tuple.Create(v, 1.0));
		}

		public static Verteilung<T> Unmöglich<T>()
		{
			return new Verteilung<T>();
		}

		public static Verteilung<T> Gleichverteilt<T>(params T[] werte)
		{
			return Gleichverteilt((IEnumerable<T>)werte);
		}

		public static Verteilung<T> Gleichverteilt<T>(IEnumerable<T> werte)
		{
			var werteA = werte as T[] ?? werte.ToArray();
			var anzahl = werteA.Length;
			if (anzahl == 0) return Unmöglich<T>();

			var p = 1.0 / anzahl;
			return new Verteilung<T>(werteA.Select(w => Tuple.Create(w, p)));
		}

		public static Double WsEreignis<T>(this Verteilung<T> verteilung, Ereignis<T> ereignis)
		{
			return verteilung.WsEreignis(ereignis);
		}

		public static Verteilung<tR> SelectMany<tS, tR>(this Verteilung<tS> source, Func<tS, Verteilung<tR>> selector)
		{
			return source.Bind(selector);
		}

		public static Verteilung<tR> SelectMany<tS, tCol, tR>(this Verteilung<tS> source, Func<tS, Verteilung<tCol>> collectionSelector, Func<tS, tCol, tR> resultSelector)
		{
			return source.BindMap(collectionSelector, resultSelector);
			
		}

		static T[] AddTo<T>(T[] arr, T w)
		{
			var res = new T[arr.Length + 1];
			Array.Copy(arr, 0, res, 1, arr.Length);
			res[0] = w;
			return res;
		}

		public static Verteilung<T[]> TakeN<T>(this Verteilung<T> vert, int n)
		{
			if (n <= 0) return Sicher(new T[] { });

			return
			from w in vert
			from rest in vert.TakeN(n - 1)
			select AddTo(rest, w);
		}

	}
}
