using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Xamarin.Forms.AutoNavigationPage.Tests
{
    public class INavigationExtensionTests
    {
        [Fact]
        public async void ExecuteAction_ShouldExecuteRegisteredAction()
        {
            var navigationPage = new AutoNavigationPage(new Page());
            var page = new Page();
            var page2 = new TestPage();
            var vm = new TestViewModel();
            page2.BindingContext = vm;


            await navigationPage.PushAsync(page);
            await navigationPage.PushAsync(page2);
            navigationPage.RegisterAction("PopOnePage2")
                .Pop();

            //assert there are actually two pages on the stack
            Assert.True(page.Navigation.NavigationStack.Count == 3);

            await page.Navigation.ExecuteAction("PopOnePage2");
            //assert the func got executed, and the page isnt on the stack anymore
            //if a page isnt on the navigationstack, the count of its INavigation.NavigationStack is 0
            Assert.True(navigationPage.Navigation.NavigationStack.Count == 2);
        }

        [Fact]
        public async void ExecuteAction_ShouldOnlyCallExecuteForPageWithPublishingViewModel()
        {
            var navigationPage = new AutoNavigationPage(new Page());
            var page = new TestPage();
            var unusedVm = new TestViewModel();
            page.BindingContext = unusedVm;
            var page2 = new TestPage();
            var vm = new TestViewModel();
            page2.BindingContext = vm;


            await navigationPage.PushAsync(page);
            await navigationPage.PushAsync(page2);
            navigationPage.RegisterAction("PopOnePage")
                .Pop();

            //assert there are actually two pages on the stack
            Assert.True(page.Navigation.NavigationStack.Count == 3);

            await vm.InvokeRequest("PopOnePage");
            //assert the func got executed, and the page isnt on the stack anymore
            //if a page isnt on the navigationstack, the count of its INavigation.NavigationStack is 0
            Assert.True(navigationPage.Navigation.NavigationStack.Count == 2);
        }

        [Fact]
        public async void ExecuteAction_ShouldThrowOnNotRegisteredAction()
        {
            var page = new Page();
            var navigationPage = new AutoNavigationPage(page);
            Exception exc = await Record.ExceptionAsync(() => page.Navigation.ExecuteAction("UnregisteredAction"));

            Assert.IsType<ArgumentException>(exc);
            ArgumentException argExc = (ArgumentException)exc;
            Assert.True(argExc.ParamName == "actionName");
        }

        [Fact]
        public async void PopMultipleAsync_ShouldPopMultipleAsync()
        {
            var navigationPage = new AutoNavigationPage(new Page());
            var firstPage = new Page();
            var secondPage = new Page();

            await navigationPage.PushAsync(firstPage);
            await navigationPage.PushAsync(secondPage);

            //Assert  there are 3 pages on the navigationstack
            Assert.Equal(3, secondPage.Navigation.NavigationStack.Count);

            await secondPage.Navigation.PopMultipleAsync(2);

            //Assert that two pages got popped, and only one is left
            Assert.Equal(1, navigationPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async void PopUntoAsync_ShouldPopTillTheCurrentPageIsOfGivenType()
        {
            var navigationPage = new AutoNavigationPage(new Page());
            await navigationPage.PushAsync(new TestPage());
            await navigationPage.PushAsync(new Page());
            await navigationPage.PushAsync(new Page());
            var topPage = new Page();
            await navigationPage.PushAsync(topPage);
            await topPage.Navigation.PopUntoAsync<TestPage>();
            Assert.IsType<TestPage>(navigationPage.CurrentPage);
        }

    }
}
