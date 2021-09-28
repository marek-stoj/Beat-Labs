using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BeaVeR.Models.BeatSaber
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum Difficulty
  {
    Easy,
    Normal,
    Hard,
    Expert,
    ExpertPlus,
  }
}
