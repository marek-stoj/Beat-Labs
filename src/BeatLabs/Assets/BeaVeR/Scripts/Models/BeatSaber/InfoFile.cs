using System.Collections.Generic;

namespace BeaVeR.Models.BeatSaber
{
  /// <remarks>
  /// Based on https://bsmg.wiki/mapping/map-format.html and https://github.com/lolPants/beatmap-schemas/blob/master/schemas/info.schema.json.
  /// </remarks>
  public class InfoFile
  {
    public string _version;

    public string _songName;

    public string _songSubname;

    public string _songAuthorName;

    public string _levelAuthorName;

    public double _beatsPerMinute;

    public double _shuffle;

    public double _shufflePeriod;

    public double _previewStartTime;

    public double _previewDuration;

    public string _songFilename;

    public string _coverImageFilename;

    public string _environmentName;

    public string _allDirectionsEnvironmentName;

    public double _songTimeOffset;

    public List<DifficultyBeatmapSet> _difficultyBeatmapSets;

    public Dictionary<string, object> _customData;
  }
}
