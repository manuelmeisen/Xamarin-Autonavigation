using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    public interface INavActionBuilder
    {
        //todo: implement a not async custom action. Wrap the delegate, so that both async and non async lambdas are allowed
        //todo: implement modal stack

        INavActionBuilder Pop();
        INavActionBuilder Push<TypeOfPageToPush>() where TypeOfPageToPush : Page, new();
        INavActionBuilder PushDelegated<TypeOfPageToPush>(Func<TypeOfPageToPush> pageCreator) where TypeOfPageToPush : Page;
        INavActionBuilder PopMultiple(int amount);
        INavActionBuilder PopUnto<TypeToPopUnto>() where TypeToPopUnto : Page;
        INavActionBuilder PopToRoot();
        INavActionBuilder CustomAction(Func<INavigationActionInfo, Task> customAction);
        INavActionBuilder BufferPush<TypeOfPageToBuffer>() where TypeOfPageToBuffer : Page, new();
        INavActionBuilder BufferPushDelegated<TypeOfPageToBuffer>(Func<TypeOfPageToBuffer> pageCreator) where TypeOfPageToBuffer : Page;
        INavActionBuilder PushBufferedPages();

    }
}