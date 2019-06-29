using System;

namespace Xamarin.Forms.AutoNavigationPage
{
    public class AutoNavigationEventArgs : EventArgs
    {
        public INavigationActionInfo Data { get; }

        internal AutoNavigationEventArgs(INavigationActionInfo info)
        {
            Data = info;
        }
    }
}