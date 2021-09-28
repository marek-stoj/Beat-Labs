using System.Collections;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeatLabsPlaygrounds._08_SeatBaber
{
  public class StageController : MonoBehaviour
  {
    public float slowMotionFactor = 1.0f;

    public Material[] blockMaterials;

    public Saber leftSaber;

    public Saber rightSaber;

    private AudioSource _audioSource;

    private void Start()
    {
      _audioSource = GetComponent<AudioSource>();

      leftSaber.collidedWithBlock += OnSaber_collidedWithBlock;
      rightSaber.collidedWithBlock += OnSaber_collidedWithBlock;

      SetSlowMotionFactor(slowMotionFactor);

      // TODO: 2021-06-27 - Immortal - remove
      StartCoroutine(ResetSlowMotionFactor());
    }

    private void Update()
    {
#if UNITY_EDITOR
      SetSlowMotionFactor(slowMotionFactor);
#endif
    }

    private void OnSaber_collidedWithBlock(Saber saber, Block block)
    {
      // TODO: 2021-06-28 - Immortal - we should take into account the velocity of blade/tip
      // let's check if the direction is right
      // credits: Valem; https://youtu.be/gh4k0Q1Pl7E?t=530
      float saberToBlockAngle = Vector3.Angle(saber.velocity, block.transform.up);

      if (saberToBlockAngle > 130)
      {
        block.DestroyYourself();
      }
    }

    // TODO: 2021-06-27 - Immortal - remove
    private IEnumerator ResetSlowMotionFactor()
    {
      yield return new WaitForSeconds(10.0f / slowMotionFactor);

      SetSlowMotionFactor(1.0f);
    }

    // TODO: 2021-06-27 - Immortal - remove
    private void SetSlowMotionFactor(float factor)
    {
      slowMotionFactor = factor;
      _audioSource.pitch = slowMotionFactor;
    }

    public Koreography koreo => Koreographer.Instance.GetKoreographyAtIndex(0);
  }
}
