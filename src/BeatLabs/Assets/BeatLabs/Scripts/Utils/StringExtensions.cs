using System;

namespace BeatLabs.Utils
{
  public static class StringExtensions
  {
    public static bool IsEnum<TEnum>(this string s, TEnum enumValue)
    {
      if (string.IsNullOrEmpty(s))
      {
        return false;
      }

      return s.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase);
    }
  }
}
