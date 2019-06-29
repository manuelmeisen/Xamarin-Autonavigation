using System;
using System.Threading.Tasks;
using Xunit;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    public class AutoNavigationPageTests
    {
        /// <summary>
        /// Tests if the page and viewmodel call their Dispose method after StartAutoPageDisposal has been called.
        /// This tests an explicit call to PopAsync
        /// </summary>
        [Fact]
        public async void StartAutoPageDisposal_ShouldDisposeWhenPopped()
        {
            // creating the test page/viewmodel
            var page = new TestPage();
            var testViewModel = new TestViewModel();
            page.BindingContext = testViewModel;

            // adding a main page to the navigation stack, because the root will not be popped
            var navigation = new AutoNavigationPage(new Page());
            //push the test page onto the navigation stack
            await navigation.Navigation.PushAsync(page);
            //start the automatic disposal
            navigation.StartAutoPageDisposal();
            //pop the test page from the navigation stack
            await navigation.Navigation.PopAsync();

            // Assert that both page and viewmodel called their Dispose methods.
            Assert.True(testViewModel.IsDisposed);
            Assert.True(page.IsDisposed);
        }

        /// <summary>
        /// Tests if the page and viewmodel call their Dispose method after StartAutoPageDisposal has been called.
        /// This tests the back button.
        /// </summary>
        [Fact]
        public async void StartAutoPageDisposal_ShouldDisposeWhenPressedBack()
        {
            // creating the test page/viewmodel
            var page = new TestPage();
            var testViewModel = new TestViewModel();
            page.BindingContext = testViewModel;

            // adding a main page to the navigation stack, because the root will not be popped
            var navigation = new AutoNavigationPage(new Page());
            //push the test page onto the navigation stack
            await navigation.Navigation.PushAsync(page);
            //start the automatic disposal
            navigation.StartAutoPageDisposal();
            // simulates pressing the back button
            navigation.SendBackButtonPressed();

            // Assert that both page and viewmodel called their Dispose methods.
            Assert.True(testViewModel.IsDisposed);
            Assert.True(page.IsDisposed);
        }

        /// <summary>
        /// Tests if the page and viewmodel call their Dispose method after StartAutoPageDisposal has been called.
        /// This tests the RemovePage method.
        /// </summary>
        [Fact]
        public async void StartAutoPageDisposal_ShouldDisposeWhenRemoved()
        {
            // creating the test page/viewmodel
            var page = new TestPage();
            var testViewModel = new TestViewModel();
            page.BindingContext = testViewModel;

            // adding a main page to the navigation stack, because the root will not be popped
            var navigation = new AutoNavigationPage(new Page());
            //push the test page onto the navigation stack
            await navigation.PushAsync(page);
            //start the automatic disposal
            navigation.StartAutoPageDisposal();
            // remove the page
            navigation.Navigation.RemovePage(page);

            // Assert that both page and viewmodel called their Dispose methods.
            Assert.True(testViewModel.IsDisposed);
            Assert.True(page.IsDisposed);
        }


        /// <summary>
        /// Tests if the page and viewmodel do not call their Dispose method after StopAutoPageDisposal has been called.
        /// </summary>
        [Fact]
        public async void StopAutoPageDisposal_ShouldNotDisposeWhenPopped()
        {
            // creating the test page/viewmodel
            var page = new TestPage();
            var testViewModel = new TestViewModel();
            page.BindingContext = testViewModel;

            // adding a main page to the navigation stack, because the root will not be popped
            var navigation = new AutoNavigationPage(new Page());
            //start the automatic disposal
            navigation.StartAutoPageDisposal();
            //push the test page onto the navigation stack
            await navigation.PushAsync(page);
            // stop the autmatic disposal
            navigation.StopAutoPageDisposal();
            //pop the test page from the navigation stack
            await navigation.PopAsync();

            // Assert that both page and viewmodel did not call their Dispose method.
            Assert.False(testViewModel.IsDisposed);
            Assert.False(page.IsDisposed);
        }

        //todo: Implement with InternalsVisibleTo in the base project, and check the internal collection of actions directly
        [Fact]
        public async void RegisterAction_ShouldRegisterAction()
        {
            var navigationPage = new AutoNavigationPage(new Page());
            string actionName = "actionName";
            navigationPage.RegisterAction(actionName);
            Exception exc = await Record.ExceptionAsync(() => navigationPage.Navigation.ExecuteAction(actionName));
            Assert.Null(exc);
        }

        [Fact]
        public async void RegisterContainer_ShouldExecuteActionsFromContainer()
        {
            var navPage = new AutoNavigationPage(new Page());
            await navPage.Navigation.PushAsync(new Page());
            await navPage.Navigation.PushAsync(new Page());
            navPage.RegisterContainer<TestContainer>();

            Assert.Equal(3,navPage.Navigation.NavigationStack.Count);

            await navPage.Navigation.ExecuteAction("ContainerAction");

            Assert.Equal(1, navPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async void StartAutoSubscribing_ShouldAutoSubscribenewPages()
        {
            var navPage = new AutoNavigationPage(new Page());
            navPage.RegisterAction("TestAction")
                .Pop();

            var subPage = new Page();
            var testViewModel = new TestViewModel();
            subPage.BindingContext = testViewModel;

            await navPage.Navigation.PushAsync(subPage);

            Assert.Equal(2, navPage.Navigation.NavigationStack.Count);

            await testViewModel.InvokeRequest("TestAction");

            Assert.Equal(1, navPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async void StartAutoSubscribing_ShouldGarbageCollectPageAndBindingContext()
        {
            var navPage = new AutoNavigationPage(new Page());
            navPage.RegisterAction("TestAction2")
                .Pop();

            var subPage = new Page();
            var testViewModel = new TestViewModel();
            subPage.BindingContext = testViewModel;

            await navPage.Navigation.PushAsync(subPage);
            await testViewModel.InvokeRequest("TestAction2");

            WeakReference pageReference = new WeakReference(subPage);
            WeakReference contextReference = new WeakReference(testViewModel);

            subPage = null;
            testViewModel = null;
            //give controle to another thread to clean up the navigation stack properly,
            //if not the page/context wont be collected
            await Task.Delay(1);
            GC.Collect();

            Assert.False(contextReference.IsAlive);
            Assert.False(pageReference.IsAlive);
        }
    }
}
