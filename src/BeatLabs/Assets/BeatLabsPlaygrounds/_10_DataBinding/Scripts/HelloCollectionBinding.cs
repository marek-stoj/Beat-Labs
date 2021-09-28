using Peppermint.DataBinding;

public class HelloCollectionBinding : BindableMonoBehaviour
{
  public class Item : BindableObject
  {
    private string _name;
    private ICommand _removeCommand;

    public string Name
    {
      get => _name;
      set => SetProperty(ref _name, value, nameof(Name));
    }

    public ICommand RemoveCommand
    {
      get => _removeCommand;
      set => SetProperty(ref _removeCommand, value, nameof(RemoveCommand));
    }
  }

  public int initCount = 8;

  private ObservableList<Item> _items;

  private ICommand _clearCommand;
  private ICommand _createCommand;

  private int _nextIndex;

  private void Start()
  {
    _items = new ObservableList<Item>();

    for (int i = 0; i < initCount; i++)
    {
      AddNewItem();
    }

    _clearCommand = new DelegateCommand(() => _items.Clear());
    _createCommand = new DelegateCommand(AddNewItem);

    BindingManager.Instance.AddSource(this, nameof(HelloCollectionBinding));
  }

  private void OnDestroy()
  {
    BindingManager.Instance.RemoveSource(this);
  }

  public void RemoveItem(Item item)
  {
    _items.Remove(item);
  }

  private void AddNewItem()
  {
    var newItem = new Item();

    newItem.Name = $"Item {_nextIndex++:D02}";
    newItem.RemoveCommand = new DelegateCommand(() => RemoveItem(newItem));

    _items.Add(newItem);
  }

  public ObservableList<Item> Items
  {
    get => _items;
    set => SetProperty(ref _items, value, nameof(Items));
  }

  public ICommand ClearCommand
  {
    get { return _clearCommand; }
    set { SetProperty(ref _clearCommand, value, nameof(ClearCommand)); }
  }

  public ICommand CreateCommand
  {
    get { return _createCommand; }
    set { SetProperty(ref _createCommand, value, nameof(CreateCommand)); }
  }
}
