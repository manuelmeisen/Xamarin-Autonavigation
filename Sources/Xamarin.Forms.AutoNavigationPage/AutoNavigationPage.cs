using System;
using System.Collections.Generic;

namespace Xamarin.Forms.AutoNavigationPage
{
    /// <summary>
    /// Starts the automatic disposal of pages that are popped off the navigation stack.
    /// Additionaly, allows for navigation actions to be registered.
    /// </summary>
    public class AutoNavigationPage : NavigationPage
    {
        internal static (INavigation navigation, LinkedList<Page> bufferedPages) NavigationWithBufferedPages;

        public AutoNavigationPage(bool autoPageDisposal = true, bool standardSubscribtion = true)
        {
            if (autoPageDisposal) StartAutoPageDisposal();
            if (standardSubscribtion) StartAutoSubscribing();
        }

        public AutoNavigationPage(Page page, bool autoPageDisposal = true,bool standardSubscribtion = true) : base(page)
        {
            if (autoPageDisposal) StartAutoPageDisposal();
            if (standardSubscribtion) StartAutoSubscribing();
            if (page.BindingContext is IRequestNavigation requester)
            {
                requester.RequestNavigation += (requestSender, requestArgs) => page.Navigation.ExecuteAction(requestArgs.Request);
            }
        }

        private void StartAutoSubscribing()
        {
            Pushed += (sender, e) =>
            {
                if (e.Page.BindingContext is IRequestNavigation requester)
                {
                    requester.RequestNavigation += async (requestSender, requestArgs) => await  e.Page.Navigation.ExecuteAction(requestArgs.Request);
                }
            };
        }

        public INavActionBuilder RegisterAction(string actionName)
        {
            NavActionBuilder builder = new NavActionBuilder();
            NavigationActionHandler.NavigationActions.Add(actionName, builder._actionList);
            return builder;
        }

        public void RegisterContainer<NavigationActionContainerType>() where NavigationActionContainerType : NavigationContainer, new()
        {
            var container = new NavigationActionContainerType();

            foreach (var action in container.Builder.NavigationActions)
            {
                NavigationActionHandler.NavigationActions.Add(action.Key, action.Value);
            }
        }

        public void RegisterContainer(NavigationContainer container)
        {
            foreach (var action in container.Builder.NavigationActions)
            {
                NavigationActionHandler.NavigationActions.Add(action.Key, action.Value);
            }
        }

        /// <summary>
        /// Starts the automatic page disposal. Call this once, so the NavigationPage will dispose
        /// any Pages and their BindingContext when they are popped.
        /// Safe to call when it is already started.
        /// </summary>
        /// <param name="self"></param>
        public void StartAutoPageDisposal()
        {
            /* Removing CleanUp from the Popped Event is safe, even if CleanUp hasnt been added.
             * This makes sure CleanUp is never added twice. Its not thread safe, but events never really are.
             * Also, this will probably only be called once at the start of the app anyway.
             */
            Popped -= CleanUp;
            Popped += CleanUp;
        }

        /// <summary>
        /// Stops the automatic page disposal.
        /// Safe to call when it hasnt been started.
        /// </summary>
        /// <param name="self"></param>
        public void StopAutoPageDisposal()
        {
            Popped -= CleanUp;
        }

        /// <summary>
        /// Disposes the Page and BindingContext.
        /// Is internally added to the Popped Event of the NavigationPage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanUp(object sender, NavigationEventArgs e)
        {
            if (e.Page.BindingContext is IDisposable disposableContext) disposableContext.Dispose();
            if (e.Page is IDisposable disposablePage) disposablePage.Dispose();
        }
    }
}
