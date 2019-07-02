# Keeping your ViewModel clean

## The problem

Although [IRequestNavigation](Sources/Xamarin.Forms.AutoNavigationPage/EventHandling/IRequestNavigation.cs) is not Xamarin specific,
it still leaks the dependency of Xamarin.Forms.AutoNavigationPage into the ViewModel

## The solution

Through the use of the INavigation extension 'ExecuteAction', it's possible to implement the functionality through a custom event.

### Set up a custom event
```csharp
public delegate Task NavRequestEventHandler(object sender, NavRequestEventArgs e);
```

```csharp
public class NavRequestEventArgs : EventArgs
{
    public string Data { get; }
    public NavRequestEventArgs(string data)
    {
        Data = data;
    }
}
```

```csharp
public interface IStateChanger
{
    event NavRequestEventHandler StateChange;
}
```
### Implement it on the ViewModel
```csharp
public class SubViewModel : IStateChanger
{
    public event NavRequestEventHandler StateChange;

    public ICommand Back
    {
        get
        {
            return new Command(() => StateChange?.Invoke(this, new NavRequestEventArgs("back")));
        }
    }
}
```

### Execute on the View
```csharp
public partial class SubPage : ContentPage
{
    public SubPage()
    {
        InitializeComponent();
        BindingContext = new SubViewModel();
        (BindingContext as IStateChanger).StateChange += (sender, e) => Navigation.ExecuteAction(e.Data);
    }
}
```

## Roadmap

Future versions (C#8) will probably implement an easier way at the
AutoNavigationPage configuration level.
