using PubSub.Extension;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.AutoNavigationPage;

namespace ExampleProject
{
    public class MainViewModel : IRequestNavigation
    {
        public event RequestNavigationEventHandler RequestNavigation;

        public ICommand Continue
        {
            get
            {
                return new Command(async() =>{
                    await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs("PrepareSub"));
                    this.Publish<string>("An argument sent with buffering and messaging.");
                    await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs("Open"));
                });
            }
        }

        public void Dispose()
        {
            
        }
    }
}
