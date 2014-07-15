using System.Collections.Generic;
using System.Linq;

namespace Monaden.CSharp.Lists
{
    public static class Beispiel
    {
        public static IEnumerable<IEnumerable<T>> Permuationen<T>(IEnumerable<T> items)
        {
            if (!items.Any()) return new[] { new T[] { } };

            return
                from h in items
                from t in Permuationen(items.Where(x => !Equals(x, h)))
                select h.Vor(t);
        }

        static IEnumerable<T> Vor<T>(this T value, IEnumerable<T> rest)
        {
            yield return value;
            foreach (var v in rest)
                yield return v;
        }
    }
}
