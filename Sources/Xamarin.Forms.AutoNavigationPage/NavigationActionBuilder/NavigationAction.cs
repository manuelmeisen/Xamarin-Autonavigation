using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage
{
    internal delegate Task<NavigationActionInfo> NavigationAction(NavigationActionInfo info);
}
