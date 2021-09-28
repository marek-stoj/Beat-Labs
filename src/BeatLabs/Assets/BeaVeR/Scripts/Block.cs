using System;
using BeatLabs.Utils;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeaVeR
{
  public class Block : MonoBehaviour
  {
    public StageController stageController;

    public float speed;

    public float spawnRadius;

    public float despawnRadius;

    public GameObject target;

    public float targetTimeInSeconds;

    public int layer;
    
    public Material material;

    public float angle;
    
    public bool cutDirectionMatters;

    private Renderer[] _renderers;

    public void Initialize(
      StageController stageController,
      float speed,
      float spawnRadius,
      float despawnRadius,
      GameObject target,
      float targetTimeInSeconds,
      int layer,
      Material material,
      float angle,
      bool cutDirectionMatters)
    {
      this.stageController = stageController;

      this.speed = speed;
      this.spawnRadius = spawnRadius;
      this.despawnRadius = despawnRadius;

      this.target = target;
      this.targetTimeInSeconds = targetTimeInSeconds;

      this.layer = layer;
      this.material = material;
      this.angle = angle;
      this.cutDirectionMatters = cutDirectionMatters;
    }

    private void Start()
    {
      gameObject.SetLayerRecursively(layer);

      _renderers = GetComponentsInChildren<Renderer>();
      _renderers[0].sharedMaterial = material;

      if (cutDirectionMatters)
      {
        this.transform.Rotate(0.0f, 0.0f, -angle);
      }
      else
      {
        GameObject cutDirectionIndicator = this.FindChildByNameRecursive("CutDirectionIndicator");

        cutDirectionIndicator.SetActive(false);
      }
    }

    private void Update()
    {
      if (target == null)
      {
        return;
      }

      Koreography koreo = stageController.koreo;

      int currentTimeInSamples = koreo.GetLatestSampleTime();
      float currentTimeInSeconds = (float)currentTimeInSamples / koreo.SampleRate;

      Vector3 targetPosition = target.transform.position;
      float timeDistanceInSeconds = targetTimeInSeconds - currentTimeInSeconds;
      float currentPositionZ = stageController.slowMotionFactor * speed * timeDistanceInSeconds + targetPosition.z;
      float spaceDistanceInUnits = targetPosition.z - currentPositionZ;

      bool hasPassedTheTarget = timeDistanceInSeconds < 0.0f;

      if (!hasPassedTheTarget)
      {
        bool isWithinSpawnDistance = Math.Abs(spaceDistanceInUnits) <= spawnRadius;

        if (_renderers[0].enabled)
        {
          if (!isWithinSpawnDistance)
          {
            foreach (Renderer tmpRenderer in _renderers)
            {
              tmpRenderer.enabled = false;
            }

            return;
          }
        }
        else
        {
          if (isWithinSpawnDistance)
          {
            foreach (Renderer tmpRenderer in _renderers)
            {
              tmpRenderer.enabled = true;
            }
          }
        }
      }
      else // has passed the target
      {
        bool isOutsideDespawnRadius = Math.Abs(spaceDistanceInUnits) > despawnRadius;

        if (isOutsideDespawnRadius)
        {
          DestroyYourself();

          return;
        }
      }

      transform.position = new Vector3(targetPosition.x, targetPosition.y, currentPositionZ);
    }

    public void DestroyYourself()
    {
      Destroy(gameObject);
    }
  }
}
