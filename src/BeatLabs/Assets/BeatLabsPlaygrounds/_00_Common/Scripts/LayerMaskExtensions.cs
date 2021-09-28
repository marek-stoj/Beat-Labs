using UnityEngine;

namespace Assets.BeatLabsPlaygrounds._00_Common.Scripts
{
  public static class LayerMaskExtensions
  {
    public static bool ContainsLayer(this LayerMask layerMask, int layer)
    {
      return (layerMask & (1 << layer)) != 0;
    }
  }
}
