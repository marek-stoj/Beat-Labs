using System.Collections.Generic;

namespace BeaVeR.Models.BeatSaber{
  public class DifficultyBeatmap
  {
    public Difficulty _difficulty;

    public DifficultyRank _difficultyRank;

    public string _beatmapFilename;

    public double _noteJumpMovementSpeed;

    public double _noteJumpStartBeatOffset;

    public Dictionary<string, object> _customData;
  }
}
