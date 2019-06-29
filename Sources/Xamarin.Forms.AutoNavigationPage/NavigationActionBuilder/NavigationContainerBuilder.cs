using System.Collections.Generic;

namespace Xamarin.Forms.AutoNavigationPage
{
    internal class NavigationContainerBuilder : INavigationContainerBuilder
    {
        internal Dictionary<string, LinkedList<NavigationAction>> NavigationActions { get; } =
            new Dictionary<string, LinkedList<NavigationAction>>();

        public INavActionBuilder RegisterAction(string actionName)
        {
            NavActionBuilder builder = new NavActionBuilder();
            NavigationActions.Add(actionName, builder._actionList);
            return builder;
        }
    }
}