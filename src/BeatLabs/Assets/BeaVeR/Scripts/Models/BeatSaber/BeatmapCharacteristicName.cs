using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BeaVeR.Models.BeatSaber
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum BeatmapCharacteristicName
  {
    Standard,
    NoArrows,
    OneSaber,
    [EnumMember(Value = "360Degree")]
    A360Degree,
    [EnumMember(Value = "90Degree")]
    A90Degree,

    // SongCore Mod Extensions
    Lightshow,
    Lawless,
  }
}
