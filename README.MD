# Xamarin-Autonavigation

Register NavigationActions and automatically dispose any IDisposable Page/BindingContext with AutoNavigationPage.
Extends the NavigationPage and is completely compatible with it's standard functionality.

## Getting started

### Install

** Xamarin.Forms (.NET Standard 2.0) **
[![Nuget](https://img.shields.io/nuget/v/Xamarin.Forms-AutoNavigationPage.svg)](https://www.nuget.org/packages/Xamarin.Forms-AutoNavigationPage/)

### Quick setup

The quick and dirty way. For a clean MVVM version, look here. (coming soon)

#### 1) Create the AutoNavigationPage
```csharp
public App()
    {
        InitializeComponent();
        var autoNavigation = new AutoNavigationPage(new MainPage());
        MainPage = autoNavigation;
    }
```
###### References:
* [AutoNavigationPage](Sources/Xamarin.Forms.AutoNavigationPage/AutoNavigationPage.cs)
* [App](https://docs.microsoft.com/de-de/dotnet/api/xamarin.forms.application?view=xamarin-forms)
#### 2) Register NavigationActions
```csharp
public App()
    {
        InitializeComponent();
        var autoNavigation = new AutoNavigationPage(new Page());
        MainPage = autoNavigation;

        autoNavigation.RegisterAction("back")
            .PopMultiple(2);

        autoNavigation.RegisterAction("backToSelect")
            .PopUnto<SelectionPage>();
    }
```
###### References:
* [AutoNavigationPage](Sources/Xamarin.Forms.AutoNavigationPage/AutoNavigationPage.cs)
* [App](https://docs.microsoft.com/de-de/dotnet/api/xamarin.forms.application?view=xamarin-forms)
#### 2) Use them in your Page and DataContext
```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
```

```csharp
public class MainViewModel : IRequestNavigation, INotifyPropertyChanged, IDisposable
{
    public event RequestNavigationEventHandler RequestNavigation;
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand GoBack
    {
        get
        {
            return new Command(async() =>{
                await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs("back"));
            });
        }
    }

    public void Dispose()
    {
        PropertyChanged = null;
    }
}
```
###### References:
* [IRequestNavigation](Sources/Xamarin.Forms.AutoNavigationPage/EventHandling/IRequestNavigation.cs)
* [RequestNavigationEventArgs](Sources/Xamarin.Forms.AutoNavigationPage/EventHandling/RequestNavigationEventArgs.cs)
* [IDisposable](https://docs.microsoft.com/de-de/dotnet/api/system.idisposable?view=netframework-4.8)
* [ICommand](https://docs.microsoft.com/de-de/xamarin/xamarin-forms/app-fundamentals/data-binding/commanding)

If the MainPage is popped off the navigation stack, the MainViewModel will be disposed.
The Page itself doesn't implement IDisposable, but Pages that do will also be disposed.
If the ICommand GoBack is executed, 2 pages will be popped off the navigation stack. 

## Drawbacks
* Does not support multiple AutoNavigationPages
* So far, no extension functionality for the ModalStack (but planned). You can still use the standard functionality.

## How to
* Clean MVVM implementation (reference coming soon)
* Available NavigationActions (reference coming soon)
* Push pages with arguments (reference coming soon)
* Buffer pages before pushing (reference coming soon)
* Register NavigationActions through Container (reference coming soon)
