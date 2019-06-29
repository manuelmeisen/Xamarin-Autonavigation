using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    class TestViewModel : IDisposable, IRequestNavigation
    {
        public event RequestNavigationEventHandler RequestNavigation;
        public bool IsDisposed { get; private set; } = false;

        public async Task InvokeRequest(string actionName)
        {
            await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs(actionName));
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
