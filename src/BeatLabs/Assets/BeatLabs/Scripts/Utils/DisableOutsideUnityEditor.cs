using UnityEngine;

namespace BeatLabs.Utils
{
  public class DisableOutsideUnityEditor : MonoBehaviour
  {
    [Tooltip("If specified, that object will be disabled; otherwise this object will be disabled.")]
    public GameObject optionalTarget;

    private void Start()
    {
#if !UNITY_EDITOR
      GameObject gameObjectToDisable =
        optionalTarget ? optionalTarget : gameObject;

      gameObjectToDisable.SetActive(false);
#endif
    }
  }
}
