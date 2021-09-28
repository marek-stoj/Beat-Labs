using UnityEngine;

namespace BeatLabsPlaygrounds._01_XRRig
{
  public class Metronome : MonoBehaviour
  {
    public static Metronome Instance;

    public float BPM = 80;

    private AudioSource _audioSource;
    private float _lastClickTime;

    private void Awake()
    {
      if (Instance != null)
      {
        Destroy(Instance);
      }

      Instance = this;

      _audioSource = GetComponent<AudioSource>();

      enabled = false;
    }

    private void Update()
    {
      if (Time.time - _lastClickTime >= 60.0f / BPM)
      {
        _audioSource.Play();

        _lastClickTime = Time.time;
      }
    }

    public void StartPlaying()
    {
      _audioSource.Play();

      _lastClickTime = Time.time;

      enabled = true;
    }
  }
}
