using UnityEngine;

namespace BeatLabs.Utils
{
  public class DisableInPlayMode : MonoBehaviour
  {
    [Tooltip("If specified, that object will be disabled; otherwise this object will be disabled.")]
    public GameObject optionalTarget;

    private void Start()
    {
      if (!Application.isPlaying)
      {
        return;
      }

      GameObject gameObjectToDisable =
        optionalTarget ? optionalTarget : gameObject;

      gameObjectToDisable.SetActive(false);
    }
  }
}
