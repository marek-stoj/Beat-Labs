using UnityEngine;

namespace BeaVeR
{
  public class BlockTarget : MonoBehaviour
  {
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawSphere(transform.position, 0.05f);
    }
  }
}
