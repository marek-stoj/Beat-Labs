using UnityEngine;

namespace BeatLabsPlaygrounds._01_XRRig
{
  public class MusicController : MonoBehaviour
  {
    public static MusicController Instance;

    private AudioSource _audioSource;

    private void Awake()
    {
      if (Instance != null)
      {
        Destroy(Instance);
      }

      Instance = this;

      _audioSource = GetComponent<AudioSource>();
    }

    public void StartPlaying()
    {
      _audioSource.Play();
    }
  }
}
