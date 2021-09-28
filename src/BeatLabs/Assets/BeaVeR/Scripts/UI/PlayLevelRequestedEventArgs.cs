using BeaVeR.Models.BeatSaber;

namespace BeaVeR.UI
{
  public class PlayLevelRequestedEventArgs
  {
    public PlayLevelRequestedEventArgs(string beatmapHash, BeatmapCharacteristicName beatmapCharacteristicName, Difficulty beatmapDifficulty)
    {
      BeatmapHash = beatmapHash;
      BeatmapCharacteristicName = beatmapCharacteristicName;
      BeatmapDifficulty = beatmapDifficulty;
    }

    public string BeatmapHash { get; }

    public BeatmapCharacteristicName BeatmapCharacteristicName { get; }
      
    public Difficulty BeatmapDifficulty { get; }
  }
}
