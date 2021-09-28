using System.Collections.Generic;

namespace BeaVeR.Models.BeatSaber
{
  /// <remarks>
  /// Based on https://bsmg.wiki/mapping/map-format.html and https://github.com/lolPants/beatmap-schemas/blob/master/schemas/difficulty.schema.json.
  /// </remarks>
  public class DifficultyFile
  {
    public string _version;

    public List<Note> _notes;

    public List<Obstacle> _obstacles;

    public List<Event> _events;

    public List<Waypoint> _waypoints;

    public List<SpecialEventsKeywordFilter> _specialEventsKeywordFilters;

    public Dictionary<string, object> _customData;
  }
}
