using System.Collections.Generic;

namespace BeatLabsPlaygrounds._00_Common.BeatSaber.Models
{
  public class Obstacle
  {
    public double _time;

    public LineIndex _lineIndex;

    public ObstacleType _type;

    public double _duration;

    public int _width;

    public Dictionary<string, object> _customData;
  }
}
