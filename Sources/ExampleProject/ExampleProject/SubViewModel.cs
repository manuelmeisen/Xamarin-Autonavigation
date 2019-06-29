using PubSub.Extension;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.AutoNavigationPage;

namespace ExampleProject
{
    public class SubViewModel : IRequestNavigation, IDisposable, INotifyPropertyChanged
    {
        public event RequestNavigationEventHandler RequestNavigation;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _message;
        public string Message {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public ICommand GoBack
        {
            get
            {
                return new Command(async () =>
                {
                    await RequestNavigation?.Invoke(this, new RequestNavigationEventArgs("Back"));
                });
            }
        }

        public SubViewModel()
        {
            this.Subscribe<string>(ReadArgument);
        }

        private void ReadArgument(string arg)
        {
            Message = arg;
        }



        public void Dispose()
        {
            Console.WriteLine("Disposed SubPage");
        }
    }
}
