using System;
using BeatLabs.Utils;
using UnityEngine;

namespace BeatLabsPlaygrounds._00_Common
{
  public class Block : MonoBehaviour
  {
    public float Speed = 1.0f;
    public float TimeToLiveInSeconds = 10.0f;

    private float _timeBorn;

    private void Start()
    {
      _timeBorn = Time.time;
    }

    private void Update()
    {
      if (Time.time - _timeBorn >= TimeToLiveInSeconds)
      {
        Destroy(gameObject);
        return;
      }

      gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
    }

    public void Initialize(Material material, float timeToLiveInSeconds)
    {
      if (material == null)
      {
        throw new ArgumentNullException(nameof(material));
      }

      TimeToLiveInSeconds = timeToLiveInSeconds;

      MeshRenderer meshRenderer = gameObject.GetComponentSafe<MeshRenderer>();

      meshRenderer.sharedMaterial = material;
    }
  }
}
