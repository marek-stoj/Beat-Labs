using Peppermint.DataBinding;

namespace BeatLabsPlaygrounds._10_DataBinding
{
  public class MessageViewModel : BindableMonoBehaviour
  {
    private string message;

    private void Start()
    {
      Message = "Hello, DataBinding!";

      BindingManager.Instance.AddSource(this, nameof(MessageViewModel));
    }

    private void OnDestroy()
    {
      BindingManager.Instance.RemoveSource(this);
    }

    public string Message
    {
      get => message;
      set => SetProperty(ref message, value, nameof(Message));
    }
  }
}
