using Peppermint.DataBinding;
using UnityEngine;

namespace BeaVeR.UI
{
  public class InGameUI : BindableMonoBehaviour
  {
    private bool _isVisible;

    private ICommand _goBackCommand;

    private void Start()
    {
      _goBackCommand = new DelegateCommand(OnGoBackCommand);

      BindingManager.Instance.AddSource(this, nameof(InGameUI));
    }

    private void OnDestroy()
    {
      BindingManager.Instance.RemoveSource(this);
    }

    private void OnGoBackCommand()
    {
      Debug.Log("GO BACK");
    }

    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty(ref _isVisible, value, nameof(IsVisible));
    }

    public ICommand GoBackCommand
    {
      get => _goBackCommand;
      set => SetProperty(ref _goBackCommand, value, nameof(GoBackCommand));
    }
  }
}
