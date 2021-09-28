using System.Collections.Generic;
using System.Linq;
using BeatLabs.Utils;
using BeaVeR.Models.BeatSaber;
using BeaVeR.Services.BeatSaver;
using Peppermint.DataBinding;
using Version = BeaVeR.Services.BeatSaver.Version;

namespace BeaVeR.UI
{
  public class SongSearchResult : BindableObject
  {
    public static SongSearchResult Empty =
      new SongSearchResult
      {
        SongName = "No Song Selected",
        SongAuthorAndMapAuthorNames = "",
      };

    private string _songName;
    private string _songAuthorAndMapAuthorNames;
    private string _songScore;
    private string _difficulty1Name;
    private string _difficulty2Name;
    private string _difficulty3Name;
    private string _difficulty4Name;
    private string _difficulty5Name;

    private ICommand _selectSongCommand;
    private ICommand _selectDifficulty1Command;
    private ICommand _selectDifficulty2Command;
    private ICommand _selectDifficulty3Command;
    private ICommand _selectDifficulty4Command;
    private ICommand _selectDifficulty5Command;
    private ICommand _playSongCommand;

    public static bool TryCreateFromBeatSaverDoc(Doc doc, out SongSearchResult songSearchResult)
    {
      if (doc == null
          || doc.Metadata == null
          || doc.Versions == null
          || doc.Versions.Count == 0
          || doc.Versions[0].Diffs == null
          || doc.Versions[0].Diffs.Count(d => d.Characteristic.IsEnum(BeatmapCharacteristicName.Standard)) == 0)
      {
        songSearchResult = null;

        return false;
      }

      const int difficultyButtonsCount = 5;
      Version firstVersion = doc.Versions[0];

      List<Diff> standardDiffs =
        firstVersion.Diffs
          .Where(d => d.Characteristic.IsEnum(BeatmapCharacteristicName.Standard))
          .ToList();

      var difficultyNames = new string[difficultyButtonsCount];

      for (int i = 0; i < difficultyButtonsCount; i++)
      {
        difficultyNames[i] =
          i < standardDiffs.Count
            ? standardDiffs[i].Difficulty
            : null;
      }

      string songAuthorAndMapAuthorNames = doc.Metadata.SongAuthorName;

      if (!string.IsNullOrEmpty(doc.Metadata.LevelAuthorName))
      {
        songAuthorAndMapAuthorNames += $" [{doc.Metadata.LevelAuthorName}]";
      }

      string songScore = $"{(int)(100.0 * doc.Stats.Score)}%";

      songSearchResult =
        new SongSearchResult
        {
          BeatmapSaverDoc = doc,
          SongName = doc.Metadata.SongName + $" ({songScore})",
          SongAuthorAndMapAuthorNames = songAuthorAndMapAuthorNames,
          SongScore = songScore,
          Difficulty1Name = difficultyNames[0],
          Difficulty2Name = difficultyNames[1],
          Difficulty3Name = difficultyNames[2],
          Difficulty4Name = difficultyNames[3],
          Difficulty5Name = difficultyNames[4],
        };

      return true;
    }

    public Doc BeatmapSaverDoc { get; set; }

    public string SongName
    {
      get => _songName;
      set => SetProperty(ref _songName, value, nameof(SongName));
    }

    public string SongAuthorAndMapAuthorNames
    {
      get => _songAuthorAndMapAuthorNames;
      set => SetProperty(ref _songAuthorAndMapAuthorNames, value, nameof(SongAuthorAndMapAuthorNames));
    }

    public string SongScore
    {
      get => _songScore;
      set => SetProperty(ref _songScore, value, nameof(SongScore));
    }

    public string Difficulty1Name
    {
      get => _difficulty1Name;
      set => SetProperty(ref _difficulty1Name, value, nameof(Difficulty1Name));
    }

    public string Difficulty2Name
    {
      get => _difficulty2Name;
      set => SetProperty(ref _difficulty2Name, value, nameof(Difficulty2Name));
    }

    public string Difficulty3Name
    {
      get => _difficulty3Name;
      set => SetProperty(ref _difficulty3Name, value, nameof(Difficulty3Name));
    }

    public string Difficulty4Name
    {
      get => _difficulty4Name;
      set => SetProperty(ref _difficulty4Name, value, nameof(Difficulty4Name));
    }

    public string Difficulty5Name
    {
      get => _difficulty5Name;
      set => SetProperty(ref _difficulty5Name, value, nameof(Difficulty5Name));
    }

    public ICommand SelectSongCommand
    {
      get => _selectSongCommand ?? (_selectSongCommand = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectSongCommand, value, nameof(SelectSongCommand));
    }

    public ICommand SelectDifficulty1Command
    {
      get => _selectDifficulty1Command ?? (_selectDifficulty1Command = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectDifficulty1Command, value, nameof(SelectDifficulty1Command));
    }

    public ICommand SelectDifficulty2Command
    {
      get => _selectDifficulty2Command ?? (_selectDifficulty2Command = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectDifficulty2Command, value, nameof(SelectDifficulty2Command));
    }

    public ICommand SelectDifficulty3Command
    {
      get => _selectDifficulty3Command ?? (_selectDifficulty3Command = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectDifficulty3Command, value, nameof(SelectDifficulty3Command));
    }

    public ICommand SelectDifficulty4Command
    {
      get => _selectDifficulty4Command ?? (_selectDifficulty4Command = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectDifficulty4Command, value, nameof(SelectDifficulty4Command));
    }

    public ICommand SelectDifficulty5Command
    {
      get => _selectDifficulty5Command ?? (_selectDifficulty5Command = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _selectDifficulty5Command, value, nameof(SelectDifficulty5Command));
    }

    public ICommand PlaySongCommand
    {
      get => _playSongCommand ?? (_playSongCommand = UIUtils.EmptyDelegateCommand);
      set => SetProperty(ref _playSongCommand, value, nameof(PlaySongCommand));
    }

    public string[] AllDifficultyNames
    {
      get => new[] { Difficulty1Name, Difficulty2Name, Difficulty3Name, Difficulty4Name, Difficulty5Name, };
    }

    public ICommand[] AllSelectDifficultyCommands
    {
      get => new[] { SelectDifficulty1Command, SelectDifficulty2Command, SelectDifficulty3Command, SelectDifficulty4Command, SelectDifficulty5Command, };
    }

    public bool HasDifficulty1 => !string.IsNullOrEmpty(Difficulty1Name);

    public bool HasDifficulty2 => !string.IsNullOrEmpty(Difficulty2Name);

    public bool HasDifficulty3 => !string.IsNullOrEmpty(Difficulty3Name);

    public bool HasDifficulty4 => !string.IsNullOrEmpty(Difficulty4Name);

    public bool HasDifficulty5 => !string.IsNullOrEmpty(Difficulty5Name);
  }
}
