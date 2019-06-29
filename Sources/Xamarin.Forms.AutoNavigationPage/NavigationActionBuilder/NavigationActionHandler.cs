using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    internal static class NavigationActionHandler
    {
        //todo: create an object for the list
        internal static readonly Dictionary<string, LinkedList<NavigationAction>> NavigationActions
            = new Dictionary<string, LinkedList<NavigationAction>>();
    }
}
