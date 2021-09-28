using System.Collections.Generic;

namespace BeaVeR.Models.BeatSaber
{
  public class Event
  {
    public double _time;

    public EventType _type;

    /// <summary>
    /// See: https://bsmg.wiki/mapping/map-format.html#events-2
    /// </summary>
    public int _value;

    public Dictionary<string, object> _customData;
  }
}
