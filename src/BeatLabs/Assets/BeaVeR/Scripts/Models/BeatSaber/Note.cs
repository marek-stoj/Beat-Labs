using System.Collections.Generic;

namespace BeaVeR.Models.BeatSaber
{
  public class Note
  {
    public double _time;

    public LineIndex _lineIndex;

    public LineLayer _lineLayer;

    public NoteType _type;

    public CutDirection _cutDirection;

    public Dictionary<string, object> _customData;
  }
}
