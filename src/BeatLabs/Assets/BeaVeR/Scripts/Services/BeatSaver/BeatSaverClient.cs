using System;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace BeaVeR.Services.BeatSaver
{
  public class BeatSaverClient
  {
    public async Task<BeatSaverSearchResponse> Search(string query, SortOrder sortOrder)
    {
      string responseText;

      using (UnityWebRequest webRequest = UnityWebRequest.Get($"https://beatsaver.com/api/search/text/0?sortOrder={sortOrder}&q={Uri.EscapeDataString(query)}"))
      {
        await webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
          responseText = webRequest.downloadHandler.text;
        }
        else
        {
          throw new Exception("Error while searching.");
        }
      }

      BeatSaverSearchResponse searchResponse =
        JsonConvert.DeserializeObject<BeatSaverSearchResponse>(
          responseText,
          CreateJsonSerializerSettings()
        );

      return searchResponse;
    }

    private static JsonSerializerSettings CreateJsonSerializerSettings()
    {
      var settings = new JsonSerializerSettings();

      settings.Culture = CultureInfo.InvariantCulture;
      settings.FloatParseHandling = FloatParseHandling.Double;

      return settings;
    }
  }
}
