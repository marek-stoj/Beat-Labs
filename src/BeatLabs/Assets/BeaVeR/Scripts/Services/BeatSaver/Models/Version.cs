using System;
using System.Collections.Generic;

namespace BeaVeR.Services.BeatSaver
{
  public class Version
  {
    public string Hash { get; set; }

    public string Key { get; set; }

    public string State { get; set; }

    public DateTime CreatedAt { get; set; }

    public int SageScore { get; set; }

    public List<Diff> Diffs { get; set; } = new List<Diff>();

    public string DownloadURL { get; set; }

    public string CoverURL { get; set; }

    public string PreviewURL { get; set; }
  }
}
