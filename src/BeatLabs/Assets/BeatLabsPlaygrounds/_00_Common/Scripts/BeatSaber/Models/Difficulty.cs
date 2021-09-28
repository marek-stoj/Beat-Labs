using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BeatLabsPlaygrounds._00_Common.BeatSaber.Models
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
