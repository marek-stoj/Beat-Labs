using System;
using System.Collections.Generic;
using System.Globalization;
using BeatLabsPlaygrounds._00_Common.BeatSaber.Models;
using Newtonsoft.Json;
using SonicBloom.Koreo;
using UnityEditor;
using UnityEngine;

public class BeatmapToKoreo : ScriptableObject
{
  private const int _SampleRate = 44100;

  [MenuItem("Window/Beat Labs/Create Koreo from Beatmap")]
  public static void CreateKoreo()
  {
    string infoFileContents = ((TextAsset)Resources.Load("Beat-Saber-Songs/Blinding-Lights/info", typeof(TextAsset))).text;
    string difficultyFileContents = ((TextAsset)Resources.Load("Beat-Saber-Songs/Blinding-Lights/ExpertStandard")).text;

    JsonSerializerSettings jsonSettings = CreateJsonSerializerSettings();
    InfoFile infoFile = JsonConvert.DeserializeObject<InfoFile>(infoFileContents, jsonSettings);
    DifficultyFile difficultyFile = JsonConvert.DeserializeObject<DifficultyFile>(difficultyFileContents, jsonSettings);

    Debug.Log($"Name: {infoFile._songName}");
    Debug.Log($"Notes: {difficultyFile._notes.Count}");

    Koreography koreo = ScriptableObject.CreateInstance<Koreography>();

    koreo.name = "Koreo";
    koreo.SourceClip = ((AudioClip)Resources.Load("Beat-Saber-Songs/Blinding-Lights/song", typeof(AudioClip)));

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

    Debug.Log("Saving koreo track assets.");

    foreach (KoreographyTrack koreoTrack in koreoTracks.Values)
    {
      AssetDatabase.CreateAsset(koreoTrack, $"Assets/BeatLabsPlaygrounds/_07_KoreoFromBeatmap/Koreos/{koreoTrack.name}.asset");
      AssetDatabase.SaveAssets();
    }

    Debug.Log("Saving koreo asset.");

    AssetDatabase.CreateAsset(koreo, $"Assets/BeatLabsPlaygrounds/_07_KoreoFromBeatmap/Koreos/{koreo.name}.asset");
    AssetDatabase.SaveAssets();
  }

  private static JsonSerializerSettings CreateJsonSerializerSettings()
  {
    var settings = new JsonSerializerSettings();

    settings.Culture = CultureInfo.InvariantCulture;
    settings.FloatParseHandling = FloatParseHandling.Double;

    return settings;
  }
}
