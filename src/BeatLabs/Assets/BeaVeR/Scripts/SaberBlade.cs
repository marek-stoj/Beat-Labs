using System;
using Assets.BeatLabsPlaygrounds._00_Common.Scripts;
using BeatLabs.Utils;
using UnityEngine;

namespace BeaVeR
{
  public class SaberBlade : MonoBehaviour
  {
    public LayerMask hittableLayerMask;

    public event Action<SaberBlade, Block> collidedWithBlock;

    private void OnTriggerEnter(Collider otherCollider)
    {
      GameObject otherGameObject = otherCollider.gameObject;

      if (!hittableLayerMask.ContainsLayer(otherGameObject.layer))
      {
        return;
      }

      Block block = otherGameObject.GetComponentSafe<Block>();

      collidedWithBlock?.Invoke(this, block);
    }
  }
}
