using Peppermint.DataBinding;

namespace BeaVeR.UI
{
  public static class UIUtils
  {
    public static DelegateCommand EmptyDelegateCommand =
      new DelegateCommand(() => { }, () => false);
  }
}
