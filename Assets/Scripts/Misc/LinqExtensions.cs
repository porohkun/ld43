using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LinqExtensions
{
    public static int IndexOfWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (predicate == null)
            return 0;
        int i = 0;
        foreach (var item in source)
            if (predicate.Invoke(item))
                return i;
            else
                i++;
        return -1;
    }

    public static IEnumerable<T> Enumerate<T>(this T item)
    {
        yield return item;
    }

}
