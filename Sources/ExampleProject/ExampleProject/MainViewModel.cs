using PubSub.Extension;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.AutoNavigationPage;

namespace ExampleProject
{
    public class MainViewModel : IRequestNavigation, INotifyPropertyChanged, IDisposable
    {
        public event RequestNavigationEventHandler RequestNavigation;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Continue
        {
            get
            {
                return new Command(async() =>{
                    await RequestNavigation?.Invoke(this,new RequestNavigationEventArgs("PrepareSub"));
                    this.Publish<string>("A message");
                    await RequestNavigation?.Invoke(this, new RequestNavigationEventArgs("Open"));
                });
            }
        }

        public void Dispose()
        {
            PropertyChanged = null;
        }
    }
}
