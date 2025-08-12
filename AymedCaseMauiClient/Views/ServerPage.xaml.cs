using AymedCaseMauiClient.Helpers;
using AymedCaseMauiClient.ViewModels;

namespace AymedCaseMauiClient.Views;
public partial class ServerPage : ContentPage
{
    public ServerPage() { InitializeComponent(); BindingContext = ServiceHelper.Get<ServerViewModel>(); }
}
