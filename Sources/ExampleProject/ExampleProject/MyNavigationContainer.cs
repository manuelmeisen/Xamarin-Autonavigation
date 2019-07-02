using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.AutoNavigationPage;

namespace ExampleProject
{
    public class MyNavigationContainer : NavigationContainer
    {
        public override void OnLoad(INavigationContainerBuilder builder)
        {
            builder.RegisterAction("Back")
                .Pop()
                .CustomAction(async (info) => Console.WriteLine($"Popped {info.PoppedPages.Count} pages"));
                
        }
    }
}
