using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    public abstract class NavigationContainer
    {
        internal NavigationContainerBuilder Builder { get; } = new NavigationContainerBuilder();

        public NavigationContainer()
        {
            OnLoad(Builder);
        }

        public abstract void OnLoad(INavigationContainerBuilder builder);

    }
}