using UnityEngine;

namespace BeatLabsPlaygrounds._06_Sabers.Scripts
{
  public class Saber : MonoBehaviour
  {
    public float Pitch = 55;
    public GameObject Tip;
    public LayerMask HittableLayer;

    private void OnEnable()
    {
      Vector3 eulerAngles = transform.localRotation.eulerAngles;

      transform.localRotation =
        Quaternion.Euler(
          new Vector3(Pitch, eulerAngles.y, eulerAngles.z));
    }
  }
}
