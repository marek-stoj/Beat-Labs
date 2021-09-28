using System;
using System.Collections;
using BeatLabs.Utils;
using BeaVeR.XR;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

namespace BeaVeR
{
  public class StageController : MonoBehaviour
  {
    public event EventHandler StartLevelRequested;
    public event EventHandler StopLevelRequested;

#pragma warning disable 649

    [SerializeField]
    private SimpleMusicPlayer simpleMusicPlayer;

#pragma warning restore 649

    public float slowMotionFactor = 1.0f;

    public Material[] blockMaterials;

    public Saber leftSaber;

    public Saber rightSaber;

    private Koreography _koreo;
    private AudioSource _audioSource;

    private void Awake()
    {
      if (simpleMusicPlayer == null)
      {
        throw new Exception("Simple Music Player has to be assigned.");
      }
    }

    private void Start()
    {
      _audioSource = this.GetComponentSafe<AudioSource>();

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

    public void StartLevel(Koreography koreography)
    {
      XRRigState.Instance.AreSabersActive = true;

      _koreo = koreography;

      simpleMusicPlayer.LoadSong(_koreo, startSampleTime: 0, autoPlay: false);

      StartLevelRequested?.Invoke(this, EventArgs.Empty);

      simpleMusicPlayer.Play();
    }

    public void StopLevel()
    {
      XRRigState.Instance.AreSabersActive = false;

      simpleMusicPlayer.Stop();
      StopLevelRequested?.Invoke(this, EventArgs.Empty);
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

    public Koreography koreo => _koreo;
  }
}
