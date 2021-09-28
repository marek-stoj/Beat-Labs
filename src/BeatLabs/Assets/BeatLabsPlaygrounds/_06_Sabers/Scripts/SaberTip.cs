using Assets.BeatLabsPlaygrounds._00_Common.Scripts;
using BeatLabs.Utils;
using UnityEngine;

public class SaberTip : MonoBehaviour
{
  public LayerMask hittableLayerMask;

  private void OnTriggerEnter(Collider otherCollider)
  {
    GameObject otherGameObject = otherCollider.gameObject;

    if (!hittableLayerMask.ContainsLayer(otherGameObject.layer))
    {
      return;
    }

    Debug.Log("COLLISION STARTED!");

    otherGameObject.GetComponentSafe<Renderer>()
      .material.color = Color.red;
  }

  private void OnTriggerExit(Collider otherCollider)
  {
    GameObject otherGameObject = otherCollider.gameObject;

    if (!hittableLayerMask.ContainsLayer(otherGameObject.layer))
    {
      return;
    }

    Debug.Log("COLLISION ENDED!");

    otherGameObject.GetComponentSafe<Renderer>()
      .material.color = Color.white;
  }
}
