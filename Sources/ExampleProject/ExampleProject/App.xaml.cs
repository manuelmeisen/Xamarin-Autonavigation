using Xamarin.Forms;
using Xamarin.Forms.AutoNavigationPage;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExampleProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var autoNavigation = new AutoNavigationPage(new MainPage());

            autoNavigation.RegisterAction("PrepareSub")
                .BufferPush<SubPage>();

            autoNavigation.RegisterAction("Open")
                .PushBufferedPages();

            autoNavigation.RegisterContainer<MyNavigationContainer>();
            MainPage = autoNavigation;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
