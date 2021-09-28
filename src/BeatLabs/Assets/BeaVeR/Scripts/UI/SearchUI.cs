using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.BeatLabs.Scripts.Utils;
using BeatLabs.Utils;
using BeaVeR.Models.BeatSaber;
using BeaVeR.Services.BeatSaver;
using Peppermint.DataBinding;
using SpaceBear.VRUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Version = BeaVeR.Services.BeatSaver.Version;

namespace BeaVeR.UI
{
  public class SearchUI : BindableMonoBehaviour
  {
    public event Action<object> SearchingStarted;
    public event Action<object> SearchingFinished;
    public event Action<object> DownloadingStarted;
    public event Action<object> DownloadingFinished;
    public event Func<object, PlayLevelRequestedEventArgs, Task> PlayLevelRequested;

#pragma warning disable 649

    [SerializeField]
    private InputField _queryInputField;

    [SerializeField]
    private VRUIKeyboardEx _keyboard;

    [SerializeField]
    private ScrollRect _searchResultsScrollRect;

    [SerializeField]
    private GameObject[] _difficultyButtons;

    [SerializeField]
    private Button _sampleQuery1Button;

    [SerializeField]
    private Button _sampleQuery2Button;

    [SerializeField]
    private Button _sampleQuery3Button;

#pragma warning restore 649

    private bool _isVisible;
    private string _query;
    private ObservableList<SongSearchResult> _songSearchResults;
    private SongSearchResult _selectedSongSearchResult;

    private ICommand _searchCommand;
    private ICommand _clearCommand;
    private ICommand _sampleQuery1Command;
    private ICommand _sampleQuery2Command;
    private ICommand _sampleQuery3Command;

    private BeatSaverClient _beatSaverClient;

    private int _selectedDifficultyIndex;

    private void Awake()
    {
      _beatSaverClient = new BeatSaverClient();
    }

    private void Start()
    {
      _songSearchResults = new ObservableList<SongSearchResult>();
      _searchCommand = new DelegateCommand(OnSearchCommand, CanExecuteSearchCommand);
      _clearCommand = new DelegateCommand(OnClearCommand, CanExecuteSearchCommand);
      _sampleQuery1Command = new DelegateCommand(() => OnSampleQueryCommand(_sampleQuery1Button));
      _sampleQuery2Command = new DelegateCommand(() => OnSampleQueryCommand(_sampleQuery2Button));
      _sampleQuery3Command = new DelegateCommand(() => OnSampleQueryCommand(_sampleQuery3Button));

      EventSystem.current.SetSelectedGameObject(_queryInputField.gameObject);
      _keyboard.gameObject.SetActive(false);

      BindingManager.Instance.AddSource(this, nameof(SearchUI));

      // TODO: 2021-09-04 - Immortal - remove
      Query = "blinding";
      Query = "korn";
      Query = "dance monkey";
      Query = "blinding lights";
    }

    private void Update()
    {
      if (_queryInputField.isFocused)
      {
        if (!_keyboard.gameObject.activeSelf)
        {
          _keyboard.gameObject.SetActive(true);
        }
      }
    }

    private void OnDestroy()
    {
      BindingManager.Instance.RemoveSource(this);
    }

    private bool CanExecuteSearchCommand()
    {
      return !string.IsNullOrEmpty(Query);
    }

    private async void OnSearchCommand()
    {
      if (string.IsNullOrEmpty(Query))
      {
        return;
      }

      _keyboard.gameObject.SetActive(false);
      SongSearchResults.Clear();
      SelectedSongSearchResult = null;

      SearchingStarted?.Invoke(this);

      try
      {
        Debug.Log($"Searching for '{Query}'.");

        BeatSaverSearchResponse searchResponse = await _beatSaverClient.Search(Query, SortOrder.Relevance);

        Debug.Log($"Got search response. Is it non-empty? {!searchResponse.IsEmpty}");

        foreach (Doc doc in searchResponse.Docs.OrderByDescending(d => d.Stats?.Score))
        {
          if (!SongSearchResult.TryCreateFromBeatSaverDoc(doc, out SongSearchResult songSearchResult))
          {
            Debug.LogWarning($"Couldn't create a {nameof(SongSearchResult)} from Beat Saver doc. Doc Id: '{doc.Id}'.");

            continue;
          }

          songSearchResult.SelectSongCommand =
            new DelegateCommand(() => OnSelectSongSearchResultCommand(songSearchResult));

          songSearchResult.SelectDifficulty1Command =
            new DelegateCommand(() => OnSelectDifficultyCommand(songSearchResult, 0), () => songSearchResult.HasDifficulty1);

          songSearchResult.SelectDifficulty2Command =
            new DelegateCommand(() => OnSelectDifficultyCommand(songSearchResult, 1), () => songSearchResult.HasDifficulty2);

          songSearchResult.SelectDifficulty3Command =
            new DelegateCommand(() => OnSelectDifficultyCommand(songSearchResult, 2), () => songSearchResult.HasDifficulty3);

          songSearchResult.SelectDifficulty4Command =
            new DelegateCommand(() => OnSelectDifficultyCommand(songSearchResult, 3), () => songSearchResult.HasDifficulty4);

          songSearchResult.SelectDifficulty5Command =
            new DelegateCommand(() => OnSelectDifficultyCommand(songSearchResult, 4), () => songSearchResult.HasDifficulty5);

          songSearchResult.PlaySongCommand =
            new DelegateCommand(async () => await OnPlaySelectedSongCommand(songSearchResult));

          SongSearchResults.Add(songSearchResult);
        }

        _searchResultsScrollRect.verticalNormalizedPosition = 1.0f;
      }
      finally
      {
        SearchingFinished?.Invoke(this);
      }
    }

    private void OnClearCommand()
    {
      _queryInputField.text = "";
      EventSystem.current.SetSelectedGameObject(_queryInputField.gameObject);
    }

    private void OnSelectSongSearchResultCommand(SongSearchResult songSearchResult)
    {
      // select song
      SelectedSongSearchResult = songSearchResult;

      // select initial difficulty
      ICommand selectInitialDifficultyCommand;

      int? selectExpertDifficultyCommandIndex =
        songSearchResult.AllDifficultyNames
          .FirstIndexOrDefault(d => d.IsEnum(Difficulty.Expert));

      if (selectExpertDifficultyCommandIndex.HasValue)
      {
        selectInitialDifficultyCommand =
          songSearchResult.AllSelectDifficultyCommands[selectExpertDifficultyCommandIndex.Value];
      }
      else
      {
        selectInitialDifficultyCommand =
          songSearchResult.AllSelectDifficultyCommands
            .Last(c => c.CanExecute());
      }

      selectInitialDifficultyCommand.Execute();
    }

    private void OnSelectDifficultyCommand(SongSearchResult songSearchResult, int difficultyIndex)
    {
      foreach (GameObject difficultyButton in _difficultyButtons)
      {
        difficultyButton.GetComponentSafe<Image>().enabled = false;
      }

      GameObject selectedDifficultyButton = _difficultyButtons[difficultyIndex];

      selectedDifficultyButton.GetComponentSafe<Image>().enabled = true;

      _selectedDifficultyIndex = difficultyIndex;
    }

    private async Task OnPlaySelectedSongCommand(SongSearchResult songSearchResult)
    {
      Version beatmapVersion =
        songSearchResult.BeatmapSaverDoc
          .Versions
          .First();

      string beatmapUrl = beatmapVersion.DownloadURL;
      string beatmapHash = beatmapVersion.Hash;

      BeatmapCharacteristicName beatmapCharacteristicName = BeatmapCharacteristicName.Standard;

      Difficulty beatmapDifficulty =
        beatmapVersion.Diffs
          .Where(d => d.Characteristic.IsEnum(BeatmapCharacteristicName.Standard))
          .Where((d, i) => i == _selectedDifficultyIndex)
          .Select(d => (Difficulty)Enum.Parse(typeof(Difficulty), d.Difficulty, ignoreCase: true))
          .Single();

      IsVisible = false;

      DownloadingStarted?.Invoke(this);

      try
      {
        await BeatmapDownloader.Instance.DownloadAndDecompressBeatmap(
          beatmapUrl,
          beatmapHash);
      }
      catch (Exception exc)
      {
        Debug.LogError("Unhandled exception.\n" + exc);

        IsVisible = true;
      }
      finally
      {
        DownloadingFinished?.Invoke(this);
      }

      var playLevelRequestedEventArgs =
        new PlayLevelRequestedEventArgs(
          beatmapHash,
          beatmapCharacteristicName,
          beatmapDifficulty);

      PlayLevelRequested?.Invoke(this, playLevelRequestedEventArgs);
    }

    private void OnSampleQueryCommand(Selectable sender)
    {
      _queryInputField.text = sender.gameObject.GetComponentInChildrenSafe<Text>().text;
    }

    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty(ref _isVisible, value, nameof(IsVisible));
    }

    public string Query
    {
      get => _query;
      set => SetProperty(ref _query, value, nameof(Query));
    }

    public ObservableList<SongSearchResult> SongSearchResults
    {
      get => _songSearchResults;
      set => SetProperty(ref _songSearchResults, value, nameof(SongSearchResults));
    }

    public SongSearchResult SelectedSongSearchResult
    {
      get => _selectedSongSearchResult ?? (_selectedSongSearchResult = SongSearchResult.Empty);
      set => SetProperty(ref _selectedSongSearchResult, value, nameof(SelectedSongSearchResult));
    }

    public ICommand SearchCommand
    {
      get => _searchCommand;
      set => SetProperty(ref _searchCommand, value, nameof(SearchCommand));
    }

    public ICommand ClearCommand
    {
      get => _clearCommand;
      set => SetProperty(ref _clearCommand, value, nameof(ClearCommand));
    }

    public ICommand SampleQuery1Command
    {
      get => _sampleQuery1Command;
      set => SetProperty(ref _sampleQuery1Command, value, nameof(SampleQuery1Command));
    }

    public ICommand SampleQuery2Command
    {
      get => _sampleQuery2Command;
      set => SetProperty(ref _sampleQuery2Command, value, nameof(SampleQuery2Command));
    }

    public ICommand SampleQuery3Command
    {
      get => _sampleQuery3Command;
      set => SetProperty(ref _sampleQuery3Command, value, nameof(SampleQuery3Command));
    }
  }
}
