using System;
using System.Threading.Tasks;
using BeatLabs.Utils;
using BeaVeR;
using BeaVeR.UI;
using Peppermint.DataBinding;
using SonicBloom.Koreo;
using UnityEngine;

public class MainUI : BindableMonoBehaviour
{
  public enum GameState
  {
    SongSelection,
    SongLoading,
    SongPlaying,
  }

  public static MainUI Instance { get; private set; }

#pragma warning disable 649

  [SerializeField]
  private BusyPanel busyPanel;

  [SerializeField]
  private SearchUI searchUI;

  [SerializeField]
  private InGameUI inGameUI;

  [SerializeField]
  private StageController stageController;

#pragma warning restore 649

  private GameState _gameState;
  private bool _isBusy;

  private void Awake()
  {
    Instance = this;
  }

  private void Start()
  {
    searchUI.SearchingStarted += SearchUI_OnSearchingStarted;
    searchUI.SearchingFinished += SearchUI_OnSearchingFinished;
    searchUI.DownloadingStarted += SearchUI_OnDownloadingStarted;
    searchUI.DownloadingFinished += SearchUI_OnDownloadingFinished;
    searchUI.PlayLevelRequested += SearchUI_OnPlayLevelRequested;

    BindingManager.Instance.AddSource(this, nameof(MainUI));

    TransitionGameStateTo(GameState.SongSelection);
  }
  
  private void OnDestroy()
  {
    searchUI.DownloadingStarted -= SearchUI_OnDownloadingStarted;
    searchUI.DownloadingFinished -= SearchUI_OnDownloadingFinished;
    searchUI.PlayLevelRequested -= SearchUI_OnPlayLevelRequested;

    BindingManager.Instance.RemoveSource(this);
  }

  public void OnGoBackButton_Click()
  {
    if (_gameState == GameState.SongSelection)
    {
      this.GetComponentInChildrenSafe<NewGoBackPanel>().OnClick_GoBackButton();
    }
    else if (_gameState == GameState.SongPlaying)
    {
      stageController.StopLevel();

      TransitionGameStateTo(GameState.SongSelection);
    }
  }

  private void SearchUI_OnDownloadingStarted(object sender)
  {
    busyPanel.text.text = "Downloading ...";
    IsBusy = true;
  }

  private void SearchUI_OnDownloadingFinished(object sender)
  {
    busyPanel.text.text = "...";
    IsBusy = false;
  }

  private void SearchUI_OnSearchingStarted(object sender)
  {
    busyPanel.text.text = "Searching ...";
    IsBusy = true;
  }

  private void SearchUI_OnSearchingFinished(object sender)
  {
    busyPanel.text.text = "...";
    IsBusy = false;
  }

  private async Task SearchUI_OnPlayLevelRequested(object sender, PlayLevelRequestedEventArgs playLevelRequestedEventArgs)
  {
    Koreography koreo =
      await BeatmapLoader.Instance.LoadBeatmap(
        playLevelRequestedEventArgs.BeatmapHash,
        playLevelRequestedEventArgs.BeatmapCharacteristicName,
        playLevelRequestedEventArgs.BeatmapDifficulty);

    TransitionGameStateTo(GameState.SongPlaying);
    
    stageController.StartLevel(koreo);
  }

  private void TransitionGameStateTo(GameState newGameState)
  {
    switch (newGameState)
    {
      case GameState.SongSelection:
        searchUI.IsVisible = true;
        inGameUI.IsVisible = false;
        break;

      case GameState.SongLoading:
        searchUI.IsVisible = false;
        break;

      case GameState.SongPlaying:
        inGameUI.IsVisible = true;
        break;

      default:
        throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
    }

    _gameState = newGameState;
  }

  public bool IsBusy
  {
    get => _isBusy;
    set => SetProperty(ref _isBusy, value, nameof(IsBusy));
  }
}
