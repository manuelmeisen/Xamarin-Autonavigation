using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    internal class NavActionBuilder : INavActionBuilder
    {
        internal readonly LinkedList<NavigationAction> _actionList 
            = new LinkedList<NavigationAction>();

        public INavActionBuilder CustomAction(Func<INavigationActionInfo,Task> customAction)
        {
            _actionList.AddLast(async (info) => {
                await customAction(info);
                return info;
            });
            return this;
        }

        public INavActionBuilder Pop()
        {
            var popFunc = new NavigationAction(async info => {
                info = await info.CurrentNavigation.PopMultipleAsync(1,info);
                return info;
            });
            _actionList.AddLast(popFunc);
            return this;
        }

        public INavActionBuilder Push<TypeOfPageToPush>() where TypeOfPageToPush : Page, new()
        {
            return PushDelegated(() => new TypeOfPageToPush());
        }

        public INavActionBuilder PushDelegated<TypeOfPageToPush>(Func<TypeOfPageToPush> pageCreator) where TypeOfPageToPush : Page
        {
            var pushFunc = new NavigationAction(
                async info =>
                {
                    var newPage = pageCreator();
                    await info.CurrentNavigation.PushAsync(newPage);
                    info.CurrentNavigation = newPage.Navigation;
                    info.AddPushedPage(newPage);
                    return info;
                }
                );
            _actionList.AddLast(pushFunc);
            return this;
        }


        public INavActionBuilder PopMultiple(int amount)
        {
            var popFunc = new NavigationAction(
                async info =>
                {
                    info = await info.CurrentNavigation.PopMultipleAsync(amount,info);
                    return info;
                }
                );
            _actionList.AddLast(popFunc);
            return this;
        }

        public INavActionBuilder PopToRoot()
        {
            var popFunc = new NavigationAction(
                async info =>
                {
                    info = await info.CurrentNavigation.PopToRootAsync(info);
                    return info;
                }
                );
            _actionList.AddLast(popFunc);
            return this;
        }

        public INavActionBuilder PopUnto<TypeToPopUnto>() where TypeToPopUnto : Page
        {
            var popFunc = new NavigationAction(
                async info =>
                {
                    info = await info.CurrentNavigation.PopUntoAsync<TypeToPopUnto>(info);
                    return info;
                }
                );
            _actionList.AddLast(popFunc);
            return this;
        }

        public INavActionBuilder BufferPush<TypeOfPageToBuffer>() where TypeOfPageToBuffer : Page, new()
        {
            return BufferPushDelegated(() => new TypeOfPageToBuffer());
        }

        public INavActionBuilder BufferPushDelegated<TypeOfPageToBuffer>(Func<TypeOfPageToBuffer> pageCreator) where TypeOfPageToBuffer : Page
        {
            var bufferFunc = new NavigationAction(
                async info =>
                {
                    var newPage = pageCreator();
                    if (info.CurrentNavigation != AutoNavigationPage.NavigationWithBufferedPages.navigation)
                    {
                        AutoNavigationPage.NavigationWithBufferedPages.navigation = info.CurrentNavigation;
                        AutoNavigationPage.NavigationWithBufferedPages.bufferedPages = new LinkedList<Page>();
                    }

                    AutoNavigationPage.NavigationWithBufferedPages.bufferedPages.AddLast(newPage);
                    return info;
                }
                );
            _actionList.AddLast(bufferFunc);
            return this;
        }

        public INavActionBuilder PushBufferedPages()
        {
            var pushFunc = new NavigationAction(
                async info =>
                {
                    if (info.InitialNavigation == AutoNavigationPage.NavigationWithBufferedPages.navigation)
                    {
                        foreach(Page p in AutoNavigationPage.NavigationWithBufferedPages.bufferedPages)
                        {
                            await info.CurrentNavigation.PushAsync(p);
                            info.CurrentNavigation = p.Navigation;

                        }
                        //delete all buffered pages that are left.
                        AutoNavigationPage.NavigationWithBufferedPages.navigation = null;
                        AutoNavigationPage.NavigationWithBufferedPages.bufferedPages = null;
                    }
                    return info;
                }
                );
            _actionList.AddLast(pushFunc);
            return this;
        }


        //todo: implement RemovePageOfType. Should remove the highest occurence of a page of the given type.
    }
}
