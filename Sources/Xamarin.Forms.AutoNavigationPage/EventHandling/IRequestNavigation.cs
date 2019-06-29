using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.AutoNavigationPage
{
    public interface IRequestNavigation
    {
        event RequestNavigationEventHandler RequestNavigation;
    }
}
