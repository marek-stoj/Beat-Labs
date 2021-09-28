using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

/// <summary>
/// See BeatmapToKoreo class (Editor). />
/// </summary>
public class Main : MonoBehaviour
{
  private const string _EventID = "Track1";

  public AudioClip sourceClip;
  public SimpleMusicPlayer musicPlayer;

  public Koreography createdKoreo;

  private void Start()
  {
    KoreographyTrack koreoTrack = ScriptableObject.CreateInstance<KoreographyTrack>();

    koreoTrack.name = "KoreoTrack";
    koreoTrack.EventID = _EventID;

    var koreoEvent = new KoreographyEvent();

    koreoEvent.StartSample = koreoEvent.EndSample = 0;
    koreoEvent.Payload = new IntPayload() { IntVal = 666 };

    koreoTrack.AddEvent(koreoEvent);

    Koreography koreo = ScriptableObject.CreateInstance<Koreography>();

    koreo.name = "Koreo";
    koreo.SourceClip = sourceClip;

    koreo.AddTrack(koreoTrack);

    createdKoreo = koreo;

    Koreographer.Instance.RegisterForEventsWithTime(_EventID, OnKoreoEvent);

    musicPlayer.LoadSong(createdKoreo, 0, false);
    musicPlayer.Play();
  }

  private void OnKoreoEvent(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
  {
    object payload = ((IntPayload)koreoEvent.Payload).IntVal;

    Debug.Log($"Koreo Event. Payload: {payload}");
  }
}
