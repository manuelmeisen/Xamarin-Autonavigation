using System.Collections.Generic;

namespace Xamarin.Forms.AutoNavigationPage
{
    public interface INavigationActionInfo
    {
        IReadOnlyCollection<Page> PoppedPages { get; }
        IReadOnlyCollection<Page> PushedPages { get; }
    }
}