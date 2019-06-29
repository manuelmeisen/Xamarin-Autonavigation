using System;
using Xamarin.Forms;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    class TestPage : Page, IDisposable
    {
        public bool IsDisposed { get; private set; } = false;
        public void Dispose()
        {
            IsDisposed = true;
        }

        public TestPage()
        {
            BindingContext = new TestViewModel();
        }
    }
}
