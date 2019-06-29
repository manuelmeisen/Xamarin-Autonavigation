using Xunit;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    public class NavActionBuilderTests
    {
        [Fact]
        public async void Pop_ShouldPopAndReturnPoppedPage()
        {
            var navPage = new AutoNavigationPage(new Page());
            var page = new Page();
            await navPage.Navigation.PushAsync(page);
            var page2 = new Page();
            await navPage.Navigation.PushAsync(page2);

            Assert.Equal(3, navPage.Navigation.NavigationStack.Count);

            navPage.RegisterAction("PopPage")
                .Pop();
            var info = await navPage.Navigation.ExecuteAction("PopPage");

            Assert.Equal(2, navPage.Navigation.NavigationStack.Count);
            Assert.Equal(1, info.PoppedPages.Count);
            Assert.Equal(0, info.PushedPages.Count);

            var enumerator = info.PoppedPages.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(page2, enumerator.Current);
        }

     

        [Fact]
        public async void Pop_ShouldPushAndReturnPushedPage()
        {
            var navPage = new AutoNavigationPage(new Page());

            navPage.RegisterAction("PushPage")
                .Push<TestPage>()
                .Push<TestPage>();
            var info = await navPage.Navigation.ExecuteAction("PushPage");

            Assert.Equal(3, navPage.Navigation.NavigationStack.Count);
            Assert.Equal(0, info.PoppedPages.Count);
            Assert.Equal(2, info.PushedPages.Count);

            var enumerator = info.PushedPages.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(navPage.Navigation.NavigationStack[1], enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(navPage.CurrentPage, enumerator.Current);
        }

        [Fact]
        public async void PopMultiple_ShouldPopMultipleAndReturnPoppedPagesInOrder()
        {
            var navPage = new AutoNavigationPage(new Page());
            var page = new Page();
            var page2 = new Page();
            var page3 = new Page();
            await navPage.Navigation.PushAsync(page);
            await navPage.Navigation.PushAsync(page2);
            await navPage.Navigation.PushAsync(page3);

            Assert.Equal(4, navPage.Navigation.NavigationStack.Count);

            navPage.RegisterAction("PopTwoPages")
                .PopMultiple(2);
            var info = await navPage.Navigation.ExecuteAction("PopTwoPages");

            Assert.Equal(2, navPage.Navigation.NavigationStack.Count);
            Assert.Equal(2, info.PoppedPages.Count);
            Assert.Equal(0, info.PushedPages.Count);

            var enumerator = info.PoppedPages.GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(page3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page2, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public async void PopToRoot_ShouldPopToRootAndReturnPoppedPagesInOrder()
        {
            var navPage = new AutoNavigationPage(new Page());
            var page = new Page();
            var page2 = new Page();
            var page3 = new Page();
            await navPage.Navigation.PushAsync(page);
            await navPage.Navigation.PushAsync(page2);
            await navPage.Navigation.PushAsync(page3);

            Assert.Equal(4, navPage.Navigation.NavigationStack.Count);

            navPage.RegisterAction("PopRoot")
                .PopToRoot();
            var info = await navPage.Navigation.ExecuteAction("PopRoot");

            Assert.Equal(1, navPage.Navigation.NavigationStack.Count);
            Assert.Equal(3, info.PoppedPages.Count);
            Assert.Equal(0, info.PushedPages.Count);

            var enumerator = info.PoppedPages.GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(page3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page2, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public async void PopUnto_ShouldPopUntoPageOfGivenTypeAndReturnPoppedPagesInOrder()
        {
            var navPage = new AutoNavigationPage(new Page());
            var page = new TestPage();
            var page2 = new Page();
            var page3 = new Page();
            await navPage.Navigation.PushAsync(page);
            await navPage.Navigation.PushAsync(page2);
            await navPage.Navigation.PushAsync(page3);

            Assert.Equal(4, navPage.Navigation.NavigationStack.Count);

            navPage.RegisterAction("PopToTestPage")
                .PopUnto<TestPage>();
            var info = await navPage.Navigation.ExecuteAction("PopToTestPage");

            Assert.Equal(2, navPage.Navigation.NavigationStack.Count);
            Assert.Equal(2, info.PoppedPages.Count);
            Assert.Equal(0, info.PushedPages.Count);
            Assert.Equal(page,navPage.CurrentPage);

            var enumerator = info.PoppedPages.GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(page3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page2, enumerator.Current);
            enumerator.MoveNext();
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public async void CustomAction_ShouldCallCustomAction()
        {
            var navPage = new AutoNavigationPage(new Page());
            bool calledCustomAction = false;
            navPage.RegisterAction("Custom")
                .CustomAction(async (nav) => calledCustomAction = true);
            await navPage.Navigation.ExecuteAction("Custom");
            Assert.True(calledCustomAction);
        }


        [Fact]
        public async void ShouldAddMultipleActionsAndExecuteInOrder()
        {
            var rootPage = new Page();
            var navPage = new AutoNavigationPage(rootPage);
            var page = new TestPage();
            var page2 = new Page();
            var page3 = new Page();
            await navPage.Navigation.PushAsync(page);
            await navPage.Navigation.PushAsync(page2);
            await navPage.Navigation.PushAsync(page3);

            Assert.Equal(4, navPage.Navigation.NavigationStack.Count);

            navPage.RegisterAction("multiple")
                .Pop()
                .CustomAction(async (nav) => Assert.Equal(3, navPage.Navigation.NavigationStack.Count))
                .PopUnto<TestPage>()
                .CustomAction(async (nav) => Assert.Equal(2, navPage.Navigation.NavigationStack.Count))
                .PopToRoot()
                .CustomAction(async (nav) => { Assert.Equal(1, navPage.Navigation.NavigationStack.Count); })
                .Push<TestPage>()
                .CustomAction(async (nav) => Assert.Equal(2, navPage.Navigation.NavigationStack.Count));

            var info = await navPage.Navigation.ExecuteAction("multiple");

            Assert.Equal(3, info.PoppedPages.Count);
            Assert.Equal(1, info.PushedPages.Count);

            var enumerator = info.PoppedPages.GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(page3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page2, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(page, enumerator.Current);
            enumerator.MoveNext();
            Assert.False(enumerator.MoveNext());

            enumerator = info.PushedPages.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(navPage.CurrentPage,enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public async void PushBufferedPages_ShouldPushBufferedPages()
        {
            var navigationPage = new AutoNavigationPage(new Page());

            navigationPage.RegisterAction("Prepare")
                .BufferPush<TestPage>()
                .BufferPush<TestPage>();

            navigationPage.RegisterAction("PushBuffer")
                .PushBufferedPages();

            await navigationPage.Navigation.ExecuteAction("Prepare");
            Assert.Equal(1, navigationPage.Navigation.NavigationStack.Count);
            await navigationPage.Navigation.ExecuteAction("PushBuffer");
            Assert.Equal(3, navigationPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async void Push_ShouldPushPage()
        {
            var navigationPage = new AutoNavigationPage(new Page());

            navigationPage.RegisterAction("Push")
                .Push<TestPage>();

            await navigationPage.Navigation.ExecuteAction("Push");

            Assert.Equal(2, navigationPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async void PushDelegated_ShouldPushPage()
        {
            var navigationPage = new AutoNavigationPage(new Page());

            navigationPage.RegisterAction("PushDelegated")
                .PushDelegated(() => new TestPage());

            await navigationPage.Navigation.ExecuteAction("PushDelegated");

            Assert.Equal(2, navigationPage.Navigation.NavigationStack.Count);
        }
    }
}
