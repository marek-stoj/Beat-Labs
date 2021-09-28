using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.BeatLabs.Scripts.Utils
{
  public static class EnumerableExtensions
  {
    public static int? FirstIndexOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
      return enumerable
        .Select((item, index) => (item, index))
        .Where(t => predicate(t.item))
        .Select(t => t.index)
        .FirstOrDefault();
    }
  }
}
