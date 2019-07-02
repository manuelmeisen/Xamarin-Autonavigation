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

```csharp
public INavActionBuilder Pop();
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L14-L18)

```csharp
public INavActionBuilder PopMultiple(int amount);
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L20-L25)

```csharp
INavActionBuilder PopUnto<TypeToPopUnto>();
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L27-L32)

```csharp
INavActionBuilder PopToRoot();
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L34-L38)

```csharp
INavActionBuilder Push<TypeOfPageToPush>()
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L40-L45)

```csharp
INavActionBuilder PushDelegated<TypeOfPageToPush>(Func<TypeOfPageToPush> pageCreator);
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L47-L53)

```csharp
INavActionBuilder BufferPush<TypeOfPageToBuffer>()
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L55-L62)

```csharp
INavActionBuilder BufferPushDelegated<TypeOfPageToBuffer>(Func<TypeOfPageToBuffer> pageCreator)
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L64-L70)

```csharp
INavActionBuilder PushBufferedPages();
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L72-L76)

```csharp
INavActionBuilder CustomAction(Func<INavigationActionInfo, Task> customAction);
```
[Documentation](https://github.com/manuelmeisen/Xamarin-Autonavigation/blob/e230c2f2025c89ffa1765b8aa9f95f62a80acc28/Sources/Xamarin.Forms.AutoNavigationPage/NavigationActionBuilder/INavActionBuilder.cs#L78-L84)

## Chaining methods

It's possible to register a NavigationAction that calls more than one of the above methods.
```csharp
navigationPage.registerAction("chainedAction")
    .PopMultiple(2)
    .Push<ExamplePage>
    .CustomAction(async (info) => Console.WriteLine($"Popped {info.PoppedPages.Count} pages"));
```
