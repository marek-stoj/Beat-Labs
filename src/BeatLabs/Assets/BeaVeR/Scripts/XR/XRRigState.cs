using Peppermint.DataBinding;

namespace BeaVeR.XR
{
  public class XRRigState : BindableMonoBehaviour
  {
    public static XRRigState Instance { get; private set; }

    private bool _areSabersActive;

    private void Awake()
    {
      Instance = this;
    }

    private void Start()
    {
      BindingManager.Instance.AddSource(this, nameof(XRRigState));
    }

    private void OnDestroy()
    {
      try
      {
        BindingManager.Instance.RemoveSource(this);
      }
      finally
      {
        Instance = null;
      }
    }

    public bool AreSabersActive
    {
      get => _areSabersActive;
      set => SetProperty(ref _areSabersActive, value, nameof(AreSabersActive));
    }
  }
}
