using System;
using System.Collections;
using Peppermint.DataBinding;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeaVeR
{
  public class SongClock : BindableMonoBehaviour
  {
#pragma warning disable 649

    [SerializeField]
    private StageController stageController;

#pragma warning restore 649

    private string _timeText;

    private void Awake()
    {
      if (stageController == null)
      {
        throw new Exception("Stage Controller has to be assigned.");
      }
    }

    private void Start()
    {
      BindingManager.Instance.AddSource(this, nameof(SongClock));
    }

    private void OnEnable()
    {
      StartCoroutine(UpdateClock());
    }

    private void OnDestroy()
    {
      BindingManager.Instance.RemoveSource(this);
    }

    private IEnumerator UpdateClock()
    {
      while (true)
      {
        yield return new WaitForSeconds(0.1f);

        try
        {
          Koreography koreo = stageController.koreo;

          if (koreo == null)
          {
            continue;
          }

          int totalTimeInSeconds = koreo.SourceClip.samples / koreo.SampleRate;
          int totalMinutes = totalTimeInSeconds / 60;
          int totalSeconds = totalTimeInSeconds % 60;

          int timeInSeconds = koreo.GetLatestSampleTime() / koreo.SampleRate;
          int minutes = timeInSeconds / 60;
          int seconds = timeInSeconds % 60;

          TimeText = $"{minutes:D2}:{seconds:D2} | {totalMinutes:D2}:{totalSeconds:D2}";
        }
        catch (Exception exc)
        {
          Debug.LogError("Unhandled exception.\n" + exc);

          yield break;
        }
      }
    }

    public string TimeText
    {
      get => _timeText;
      set => SetProperty(ref _timeText, value, nameof(TimeText));
    }
  }
}
