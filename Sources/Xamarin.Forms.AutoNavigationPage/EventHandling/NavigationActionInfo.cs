using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.AutoNavigationPage
{
    internal class NavigationActionInfo : INavigationActionInfo
    {
        internal INavigation CurrentNavigation { get; set; }
        internal INavigation InitialNavigation { get; }
        public IReadOnlyCollection<Page> PoppedPages { get; internal set; } = new LinkedList<Page>();
        public IReadOnlyCollection<Page> PushedPages { get; internal set; } = new LinkedList<Page>();

        public NavigationActionInfo(INavigation callingNavigation)
        {
            CurrentNavigation = callingNavigation;
            InitialNavigation = callingNavigation;
        }

        internal void AddPoppedPage(Page page)
        {
            (PoppedPages as LinkedList<Page>).AddLast(page);
        }

        internal void AddPushedPage(Page page)
        {
            (PushedPages as LinkedList<Page>).AddLast(page);
        }

    }
}
