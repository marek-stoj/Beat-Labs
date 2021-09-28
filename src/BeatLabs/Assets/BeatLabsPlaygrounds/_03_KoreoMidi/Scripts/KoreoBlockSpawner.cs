using BeatLabs.Utils;
using BeatLabsPlaygrounds._00_Common;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeatLabsPlaygrounds._03_KoreoMidi
{
  public class KoreoBlockSpawner : MonoBehaviour
  {
    private static readonly System.Random _rand = new System.Random();

    public GameObject BlockPrefab;
    public GameObject BlocksParent;
    public Material BlockMaterial;
    public float TimeToLiveInSeconds = 10.0f;

    [EventID]
    public string KoreoEventID;

    private void Start()
    {
      Koreographer.Instance.RegisterForEventsWithTime(KoreoEventID, OnKoreoEvent);
    }

    private void OnKoreoEvent(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
      int? intPayload = null;
      string payloadStr = null;

      if (koreoEvent.HasTextPayload())
      {
        payloadStr = koreoEvent.GetTextValue();
      }
      else if (koreoEvent.HasIntPayload())
      {
        payloadStr = koreoEvent.GetIntValue().ToString();
      }

      Debug.Log($"==> Koreo Event at Sample Time: {sampleTime}. Sample Delta: {sampleDelta}. Delta Slice: {deltaSlice.deltaOffset + ", " + deltaSlice.deltaLength}. Payload: {payloadStr}.");

      if (koreoEvent.HasIntPayload())
      {
        intPayload = koreoEvent.GetIntValue();
      }

      SpawnBlock(intPayload);
    }

    private void SpawnBlock(int? intPayload)
    {
      GameObject blockGameObject = Instantiate(BlockPrefab);

      Block block = blockGameObject.GetComponentSafe<Block>();

      block.Initialize(BlockMaterial, TimeToLiveInSeconds);

      blockGameObject.transform.parent = BlocksParent.transform;
      blockGameObject.transform.position = gameObject.transform.position;
      blockGameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);

      if (intPayload.HasValue)
      {
        blockGameObject.transform.Translate(_rand.Next() % 3 - 1, intPayload.Value / 10.0f - 4.0f, 0.0f);
      }
    }
  }
}
