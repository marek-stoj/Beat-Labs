using System.Collections.Generic;
using BeatLabs.Utils;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeatLabsPlaygrounds._05_BlockSpawning
{
  public class SimpleBlockSpawner : MonoBehaviour
  {
    public GameObject _target;

    [Tooltip("In Units per Second.")]
    public float _blockSpeed;

    public GameObject blockArchetype;
    public GameObject blocksParent;

    public int modDivider;
    public int modResult;

    public float spawnRadius;
    public float despawnRadius;

    public Material[] materials;

    private Koreography _koreo;

    private void Start()
    {
      _koreo = Koreographer.Instance.GetKoreographyAtIndex(0);

      KoreographyTrackBase koreoTrack = _koreo.GetTrackAtIndex(0);
      List<KoreographyEvent> koreoEvents = koreoTrack.GetAllEvents();

      int eventIndex = 0;

      // NOTE: this could be optimized so that we don't create all the blocks up front
      // but rather "just in time"
      foreach (KoreographyEvent koreoEvent in koreoEvents)
      {
        if (eventIndex++ % modDivider != modResult)
        {
          continue;
        }

        GameObject blockGameObject = Instantiate(blockArchetype, blocksParent.transform);
        Block block = blockGameObject.GetComponentSafe<Block>();

        block.material = materials[modResult];
        block._speed = _blockSpeed;
        block._targetTimeInSeconds = koreoEvent.StartSample / (float)_koreo.SampleRate;
        block.target = _target;
        block.spawnRadius = spawnRadius;
        block.despawnRadius = despawnRadius;
      }
    }

    private void Update()
    {
      // do nothing
    }
  }
}
