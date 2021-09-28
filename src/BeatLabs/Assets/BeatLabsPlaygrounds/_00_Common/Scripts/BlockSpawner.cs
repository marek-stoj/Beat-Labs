using System;
using System.Threading.Tasks;
using BeatLabs.Utils;
using BeatLabsPlaygrounds._01_XRRig;
using UnityEngine;

namespace BeatLabsPlaygrounds._00_Common
{
  public class BlockSpawner : MonoBehaviour
  {
    public GameObject BlockPrefab;
    public GameObject BlocksParent;
    public float FrequencyInSeconds = 1.0f;
    public float TimeToLiveInSeconds = 10.0f;
    public Material BlockMaterial;

    private float _lastSpawnTime;

    private void Awake()
    {
      // TODO: 2021-05-09 - Immortal - isn't there an Asset for things like these?
      if (BlockPrefab == null)
      {
        Debug.LogError("BlockPrefab has to be set.");
      }

      if (BlocksParent == null)
      {
        Debug.LogError("BlocksParent has to be set.");
      }

      enabled = false;
      _ = StartDelayed(4.0f);
    }

    private async Task StartDelayed(float delayInSeconds)
    {
      await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));

      _lastSpawnTime = Time.time;

      SpawnBlock();

      MusicController.Instance.StartPlaying();
      Metronome.Instance.StartPlaying();

      enabled = true;
    }

    private void Update()
    {
      if (Time.time - _lastSpawnTime >= FrequencyInSeconds)
      {
        SpawnBlock();
      }
    }

    private void SpawnBlock()
    {
      GameObject blockGameObject = Instantiate(BlockPrefab);

      Block block = blockGameObject.GetComponentSafe<Block>();

      block.Initialize(BlockMaterial, TimeToLiveInSeconds);

      blockGameObject.transform.parent = BlocksParent.transform;
      blockGameObject.transform.position = gameObject.transform.position;
      blockGameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);

      _lastSpawnTime = Time.time;
    }
  }
}
