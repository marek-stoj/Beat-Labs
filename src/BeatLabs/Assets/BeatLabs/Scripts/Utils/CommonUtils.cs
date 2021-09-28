using System;

namespace BeatLabs.Utils
{
  public static class CommonUtils
  {
    public static Exception CreateComponentNullException(Type componentType)
    {
      return new InvalidOperationException($"Component of type '{componentType.Name}' doesn't exist.");
    }
  }
}
