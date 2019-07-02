# Pushing a new page with arguments

Sometimes you'll want to pass arguments to a page you push.
Instead of creating the page inside the view or viewmodel, buffer the page 
and use messaging to notify the buffered page, before finally pushing it to the NavigationStack.

For simplicity, the following example uses [PubSub](https://github.com/upta/pubsub) as messaging framework.

## Register actions
```csharp
var autoNavigation = new AutoNavigationPage(new MainPage());

    autoNavigation.RegisterAction("PrepareSub")
        .BufferPush<SubPage>();

    autoNavigation.RegisterAction("Open")
        .PushBufferedPages();
```
## Buffer/Publish/Push

```csharp
public class MainViewModel : IRequestNavigation
{
    public event RequestNavigationEventHandler RequestNavigation;

    public ICommand Continue
    {
        get
        {
            return new Command(async() =>{
                await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs("PrepareSub"));
                this.Publish<string>("A message");
                await RequestNavigation?.Invoke(this, new RequestNavigationEventArgs("Open"));
            });
        }
    }
}
```
## Subscribe in the buffered page

```csharp
public class SubViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _labelText;
    public string LabelText 
    {
        get
        {
            return _labelText;
        }
        set
        {
            _labelText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }

    public SubViewModel()
    {
        this.Subscribe<string>((message) => LabelText = message);
    }
}
```