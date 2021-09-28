using System.Collections.Generic;

namespace BeaVeR.Services.BeatSaver
{
  public class BeatSaverSearchResponse
  {
    public List<Doc> Docs { get; set; } = new List<Doc>();

    public bool IsEmpty => Docs.Count == 0;
  }
}
