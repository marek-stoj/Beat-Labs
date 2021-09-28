using System;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeatLabsPlaygrounds._05_BlockSpawning
{
  public class Block : MonoBehaviour
  {
    [Tooltip("In Units per Second.")]
    public float _speed;

    public float _targetTimeInSeconds;

    public GameObject target;
    public float spawnRadius;
    public float despawnRadius;
    public Material material;

    private Koreography _koreo;
    private Renderer _renderer;

    private void Start()
    {
      _koreo = Koreographer.Instance.GetKoreographyAtIndex(0);

      _renderer = GetComponent<Renderer>();
      _renderer.sharedMaterial = material;
    }

    private void Update()
    {
      if (target == null)
      {
        return;
      }

      int timeInSamples = _koreo.GetLatestSampleTime();
      float currentTimeInSeconds = (float)timeInSamples / _koreo.SampleRate;

      Vector3 _targetPosition = target.transform.position;
      float timeDistanceInSeconds = _targetTimeInSeconds - currentTimeInSeconds;
      float currentPositionZ = _speed * timeDistanceInSeconds + _targetPosition.z;
      float spaceDistanceInUnits = _targetPosition.z - currentPositionZ;

      bool hasPassedTheTarget = timeDistanceInSeconds < 0.0f;

      if (!hasPassedTheTarget)
      {
        bool isWithinSpawnDistance = Math.Abs(spaceDistanceInUnits) <= spawnRadius;

        if (_renderer.enabled)
        {
          if (!isWithinSpawnDistance)
          {
            _renderer.enabled = false;

            return;
          }
        }
        else
        {
          if (isWithinSpawnDistance)
          {
            _renderer.enabled = true;
          }
        }
      }
      else // has passed the target
      {
        bool isOutsideDespawnRadius = Math.Abs(spaceDistanceInUnits) > despawnRadius;

        if (isOutsideDespawnRadius)
        {
          Destroy(gameObject);

          return;
        }
      }

      transform.position = new Vector3(_targetPosition.x, _targetPosition.y, currentPositionZ);
    }
  }
}
