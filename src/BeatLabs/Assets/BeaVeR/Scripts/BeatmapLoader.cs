using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeaVeR.Models.BeatSaber;
using Newtonsoft.Json;
using SonicBloom.Koreo;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace BeaVeR
{
  public class BeatmapLoader : MonoBehaviour
  {
    public static BeatmapLoader Instance { get; private set; }

    private void Awake()
    {
      Instance = this;
    }

    public async Task<Koreography> LoadBeatmap(string beatmapHash, BeatmapCharacteristicName beatmapCharacteristicName, Difficulty beatmapDifficulty)
    {
      BeatmapDownloader.Instance.GetBeatmapPaths(beatmapHash, out _, out string beatmapDirPath);

      Koreography koreo = await CreateKoreoFromBeatmap(beatmapDirPath, beatmapCharacteristicName, beatmapDifficulty);

      return koreo;
    }

    private static async Task<Koreography> CreateKoreoFromBeatmap(string beatmapDirPath, BeatmapCharacteristicName beatmapCharacteristicName, Difficulty difficulty)
    {
      string infoFilePath = Path.Combine(beatmapDirPath, "info.dat");
      string infoFileContents = File.ReadAllText(infoFilePath);

      JsonSerializerSettings jsonSettings = CreateJsonSerializerSettings();

      InfoFile infoFile = JsonConvert.DeserializeObject<InfoFile>(infoFileContents, jsonSettings);

      if (infoFile == null)
      {
        throw new Exception($"Couldn't parse info file. File path: '{infoFilePath}'.");
      }

      DifficultyBeatmap difficultyBeatmap =
        infoFile._difficultyBeatmapSets
          ?.FirstOrDefault(x => x._beatmapCharacteristicName == beatmapCharacteristicName)
          ?._difficultyBeatmaps
          ?.FirstOrDefault(x => x._difficulty == difficulty);

      if (difficultyBeatmap == null)
      {
        throw new Exception($"Couldn't find '{beatmapCharacteristicName}' beatmap with difficulty '{difficulty}'.");
      }

      string difficultyFilePath = Path.Combine(beatmapDirPath, $"{difficultyBeatmap._beatmapFilename}");
      string difficultyFileContents = File.ReadAllText(difficultyFilePath);

      DifficultyFile difficultyFile = JsonConvert.DeserializeObject<DifficultyFile>(difficultyFileContents, jsonSettings);

      if (difficultyFile == null)
      {
        throw new Exception($"Couldn't parse difficulty file. File path: '{difficultyFilePath}'.");
      }

      Debug.Log($"Notes count: {difficultyFile._notes.Count}");

      Koreography koreo = ScriptableObject.CreateInstance<Koreography>();

      koreo.name = "Koreo";

      string songFilePath = Path.Combine(beatmapDirPath, infoFile._songFilename);
      Uri songFileUri = new Uri(songFilePath);
      AudioClip songAudioClip;

      using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(songFileUri, AudioType.OGGVORBIS))
      {
        await webRequest.SendWebRequest();

        songAudioClip = DownloadHandlerAudioClip.GetContent(webRequest);
      }

      koreo.SourceClip = songAudioClip;

      var koreoTracks = new Dictionary<Tuple<LineIndex, LineLayer>, KoreographyTrack>();

      Debug.Log("Proecssing notes.");

      TempoSectionDef theOnlyTempoSection = koreo.GetTempoSectionAtIndex(0);

      theOnlyTempoSection.SamplesPerBeat = koreo.SampleRate / infoFile._beatsPerMinute * 60.0;

      foreach (Note note in difficultyFile._notes)
      {
        if (note._type != NoteType.LeftNote && note._type != NoteType.RightNote)
        {
          continue;
        }

        Tuple<LineIndex, LineLayer> koreoTrackIndex = Tuple.Create(note._lineIndex, note._lineLayer);
        KoreographyTrack koreoTrack;

        if (!koreoTracks.TryGetValue(koreoTrackIndex, out koreoTrack))
        {
          koreoTrack = ScriptableObject.CreateInstance<KoreographyTrack>();
          koreoTrack.name = $"KoreoTrack-{koreoTrackIndex.Item1}-{koreoTrackIndex.Item2}";
          koreoTrack.EventID = $"Block-{koreoTrackIndex.Item1}-{koreoTrackIndex.Item2}";

          koreoTracks.Add(koreoTrackIndex, koreoTrack);

          koreo.AddTrack(koreoTrack);
        }

        var koreoEvent = new KoreographyEvent();

        koreoEvent.StartSample = koreoEvent.EndSample = (int)((note._time / infoFile._beatsPerMinute * 60.0) * koreo.SampleRate);

        string noteJson = JsonConvert.SerializeObject(note);

        koreoEvent.Payload = new TextPayload { TextVal = noteJson, };

        koreoTrack.AddEvent(koreoEvent);
      }

      return koreo;
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
