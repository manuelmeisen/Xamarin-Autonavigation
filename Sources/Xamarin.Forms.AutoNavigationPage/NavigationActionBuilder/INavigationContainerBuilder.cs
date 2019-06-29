namespace Xamarin.Forms.AutoNavigationPage
{
    public interface INavigationContainerBuilder
    {
        INavActionBuilder RegisterAction(string actionName);
    }
}