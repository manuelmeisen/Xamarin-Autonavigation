using System;

namespace Xamarin.Forms.AutoNavigationPage
{
    public class RequestNavigationEventArgs : EventArgs
    {
        public string Request { get; }

        public RequestNavigationEventArgs(string request)
        {
            Request = request;
        }
    }
}
