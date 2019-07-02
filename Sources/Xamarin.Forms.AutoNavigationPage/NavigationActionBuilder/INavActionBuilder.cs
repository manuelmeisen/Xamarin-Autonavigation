using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    /// <summary>
    /// Registers NavigationActions to a list, accessible by the registered name.
    /// </summary>
    public interface INavActionBuilder
    {
        //todo: implement a not async custom action. Wrap the delegate, so that both async and non async lambdas are allowed
        //todo: implement modal stack

        /// <summary>
        /// Adds a NavigationAction that pops one page off the NavigationStack.
        /// </summary>
        /// <returns></returns>
        INavActionBuilder Pop();

        /// <summary>
        /// Pops multiple Pages off the NavigationStack.
        /// </summary>
        /// <param name="amount">The amount of pages to be popped.</param>
        /// <returns></returns>
        INavActionBuilder PopMultiple(int amount);

        /// <summary>
        /// Pops pages off the NavigationStack, untill the current page is of the supplied Type or the root page.
        /// </summary>
        /// <typeparam name="TypeToPopUnto"></typeparam>
        /// <returns></returns>
        INavActionBuilder PopUnto<TypeToPopUnto>() where TypeToPopUnto : Page;

        /// <summary>
        /// Pops pages off the NavigationStack, untill it hits the root page.
        /// </summary>
        /// <returns></returns>
        INavActionBuilder PopToRoot();

        /// <summary>
        /// Pushes a Page of the supplied Type to the NavigationStack.
        /// </summary>
        /// <typeparam name="TypeOfPageToPush"></typeparam>
        /// <returns></returns>
        INavActionBuilder Push<TypeOfPageToPush>() where TypeOfPageToPush : Page, new();

        /// <summary>
        /// Pushes a Page, supplied through a creator function, to the NavigationStack.
        /// </summary>
        /// <typeparam name="TypeOfPageToPush"></typeparam>
        /// <param name="pageCreator"></param>
        /// <returns></returns>
        INavActionBuilder PushDelegated<TypeOfPageToPush>(Func<TypeOfPageToPush> pageCreator) where TypeOfPageToPush : Page;

        /// <summary>
        /// Buffers a page of the supplied type to be pushed at a later point.
        /// Don't push or pop pages off the NavigationStack while there are buffered pages.
        /// Buffer pages to supply arguments through messaging before pushing the page to the NavigationStack.
        /// </summary>
        /// <typeparam name="TypeOfPageToBuffer"></typeparam>
        /// <returns></returns>
        INavActionBuilder BufferPush<TypeOfPageToBuffer>() where TypeOfPageToBuffer : Page, new();

        /// <summary>
        /// Buffers a page like BufferPush, but the page is supplied through a creator function.
        /// </summary>
        /// <typeparam name="TypeOfPageToBuffer"></typeparam>
        /// <param name="pageCreator"></param>
        /// <returns></returns>
        INavActionBuilder BufferPushDelegated<TypeOfPageToBuffer>(Func<TypeOfPageToBuffer> pageCreator) where TypeOfPageToBuffer : Page;

        /// <summary>
        /// Pushes all buffered pages to the NavigationStack.
        /// </summary>
        /// <returns></returns>
        INavActionBuilder PushBufferedPages();

        /// <summary>
        /// Registers a custom Func. Returns a task, so should be implemented as async.
        /// Takes INavigationActionInfo, which exposes the popped/pushed pages so far in this NavigationAction.
        /// </summary>
        /// <param name="customAction"></param>
        /// <returns></returns>
        INavActionBuilder CustomAction(Func<INavigationActionInfo, Task> customAction);
    }
}