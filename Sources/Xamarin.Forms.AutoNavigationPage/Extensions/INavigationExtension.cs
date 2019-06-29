using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    public static class INavigationExtension
    {
        public static async Task<INavigationActionInfo> ExecuteAction(this INavigation self, string actionName)
        {
            if (NavigationActionHandler.NavigationActions.TryGetValue(actionName, out var actionList))
            {
                NavigationActionInfo info = new NavigationActionInfo(self);

                foreach (var action in actionList)
                {
                    info = await action(info);
                }

                return info;

            }

            throw new ArgumentException("The action\"" + actionName + "\" is not registered.", nameof(actionName));

        }

        //todo: implement an exception for both the public and the internal method when the root page is requested to be popped.
        /// <summary>
        /// Pops multiple pages off the NavigationStack. Returns an ordered IReadOnlyCollection of the pages that were popped.
        /// </summary>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<Page>> PopMultipleAsync(this INavigation self, int pageAmount)
        {
            var removedPages = new LinkedList<Page>();
            INavigation currentNavigation = self;
            INavigation nextNavigation = null;

            for (int i = 0; i < pageAmount; i++)
            {
                // get the next INavigation if there is one.
                var stack = currentNavigation.NavigationStack;
                if (stack.Count < 2) nextNavigation = null;
                else nextNavigation = stack[stack.Count - 2].Navigation;

                // animate only the last popped page.
                Page removedPage = await currentNavigation.PopAsync(animated: (i == pageAmount - 1));
                removedPages.AddLast(removedPage);
                // if there is no next page to get the INavigation from, break
                if (nextNavigation == null) break;
                // set the navigation to the next navigation, because a page that has been popped cant access the stack.
                // also, if we dont use the INavigation from the root page, the navigation can go through the
                // intended Xamarin.Forms navigation pipeline.
                currentNavigation = nextNavigation;
            }
            return removedPages as IReadOnlyCollection<Page>;
        }

        /// <summary>
        /// Pops multiple pages off the NavigationStack. Takes the NavActionInfo and modifies it.
        /// Has to be rewritten for internal use, because the public method does not return the current INavigation,
        /// which the internal method has to return in the NavActionInfo.
        /// </summary>
        /// <returns></returns>
        internal static async Task<NavigationActionInfo> PopMultipleAsync(this INavigation self, int pageAmount, NavigationActionInfo info)
        {
            var removedPages = new LinkedList<Page>();
            INavigation currentNavigation = self;
            INavigation nextNavigation = null;

            for (int i = 0; i < pageAmount; i++)
            {
                // get the next INavigation if there is one.
                var stack = currentNavigation.NavigationStack;
                if (stack.Count < 1) nextNavigation = null;
                else nextNavigation = stack[stack.Count - 2].Navigation;

                // animate only the last popped page.
                Page removedPage = await currentNavigation.PopAsync(animated: (i == pageAmount - 1));
                removedPages.AddLast(removedPage);
                // if there is no next page to get the INavigation from, break
                if (nextNavigation == null) break;
                // set the navigation to the next navigation, because a page that has been popped cant access the stack.
                // also, if we dont use the INavigation from the root page, the navigation can go through the
                // intended Xamarin.Forms navigation pipeline.
                currentNavigation = nextNavigation;
            }

            info.CurrentNavigation = currentNavigation;
            foreach (var page in removedPages)
            {
                info.AddPoppedPage(page);
            }

            return info;
        }

        /// <summary>
        /// Pops all pages except the root page from the navigation stack.
        /// </summary>
        internal static async Task<NavigationActionInfo> PopToRootAsync(this INavigation self, NavigationActionInfo info)
        {
            int amountToPop = self.NavigationStack.Count -1;
            info = await self.PopMultipleAsync(amountToPop, info);
            return info;
        }

        /// <summary>
        /// Pops pages, till the current page is of the given type, or till it hits the root page.
        /// </summary>
        internal static async Task<NavigationActionInfo> PopUntoAsync<PageTypeToPopUnto>(this INavigation self, NavigationActionInfo info) where PageTypeToPopUnto : Page
        {
            Type untoType = typeof(PageTypeToPopUnto);
            int untoPageIndex;
            for (untoPageIndex = self.NavigationStack.Count - 1; untoPageIndex > 0; untoPageIndex--)
            {
                Page page = self.NavigationStack[untoPageIndex];
                if (untoType.IsAssignableFrom(page.GetType())) break;
            }
            return await self.PopMultipleAsync((self.NavigationStack.Count - 1) - untoPageIndex,info);
        }

        /// <summary>
        /// Pops pages, till the current page is of the given type, or till it hits the root page.
        /// </summary>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<Page>> PopUntoAsync<PageTypeToPopUnto>(this INavigation self) where PageTypeToPopUnto : Page
        {
            Type untoType = typeof(PageTypeToPopUnto);
            int untoPageIndex;
            for (untoPageIndex = self.NavigationStack.Count - 1; untoPageIndex > 0; untoPageIndex--)
            {
                Page page = self.NavigationStack[untoPageIndex];
                if (untoType.IsAssignableFrom(page.GetType())) break;
            }
            return await self.PopMultipleAsync((self.NavigationStack.Count - 1) - untoPageIndex);
        }

    }
}
