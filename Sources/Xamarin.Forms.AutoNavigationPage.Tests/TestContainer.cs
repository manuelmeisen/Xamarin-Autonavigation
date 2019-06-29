using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    public class TestContainer : NavigationContainer
    {
        public override void OnLoad(INavigationContainerBuilder builder)
        {
            builder.RegisterAction("ContainerAction")
                .Pop()
                .Pop();
        }
    }
}
