using System;
using BeatLabs.Utils;
using UnityEngine;

namespace BeatLabsPlaygrounds._08_SeatBaber
{
  public class Saber : MonoBehaviour
  {
    public float Pitch = 55.0f;

    public event Action<Saber, Block> collidedWithBlock;
    
    private SaberBlade _saberBlade;
    
    private Vector3 _previousPosition;

    private void Start()
    {
      _saberBlade = gameObject.GetComponentInChildrenSafe<SaberBlade>();
      _saberBlade.collidedWithBlock += OnSaberBlade_collidedWithBlock;

      _previousPosition = transform.position;

      UpdateSaberRotation();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
      UpdateSaberRotation();
    }
#endif

    private void Update()
    {
      _previousPosition = transform.position;
    }

    private void OnSaberBlade_collidedWithBlock(SaberBlade saberBlade, Block block)
    {
      collidedWithBlock?.Invoke(this, block);
    }

    private void UpdateSaberRotation()
    {
      Vector3 eulerAngles = transform.localRotation.eulerAngles;

      transform.localRotation =
        Quaternion.Euler(
          new Vector3(
            Pitch,
            eulerAngles.y,
            eulerAngles.z));
    }

    // TODO: 2021-06-28 - Immortal - naaah
    public Vector3 velocity => transform.position - _previousPosition;
  }
}
