# NavigationActions

The following methods can be used to register NavigationActions for your AutoNavigationPage.
You can also look up their signature and documentation on [INavActionBuilder](Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs).
Methods are composable.

## Features

* Chaining methods
* Pushing pages
* Buffering pages
* Popping pages
* Delegating page creation
* Custom functionality

## Methods

### Pop pages
```csharp
INavActionBuilder Pop();

INavActionBuilder PopMultiple(int amount);

INavActionBuilder PopUnto<TypeToPopUnto>();

INavActionBuilder PopToRoot();
```

### Push Pages
```csharp
INavActionBuilder Push<TypeOfPageToPush>();

INavActionBuilder PushDelegated<TypeOfPageToPush>(Func<TypeOfPageToPush> pageCreator);

INavActionBuilder BufferPush<TypeOfPageToBuffer>();

INavActionBuilder BufferPushDelegated<TypeOfPageToBuffer>(Func<TypeOfPageToBuffer> pageCreator);

INavActionBuilder PushBufferedPages();
```
### Other

```csharp
INavActionBuilder CustomAction(Func<INavigationActionInfo, Task> customAction);
```

## Chaining methods

It's possible to register a NavigationAction that calls more than one of the above methods.
```csharp
navigationPage.registerAction("chainedAction")
    .PopMultiple(2)
    .Push<ExamplePage>()
    .CustomAction(async (info) => Console.WriteLine($"Popped {info.PoppedPages.Count} pages"));
```
