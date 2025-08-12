using AymedCaseMauiClient.Helpers;
using AymedCaseMauiClient.ViewModels;

namespace AymedCaseMauiClient.Views;

[QueryProperty(nameof(Port), "port")]
public partial class AnswerPage : ContentPage
{
    private readonly AnswerViewModel _vm;
    public AnswerPage() { InitializeComponent(); _vm = ServiceHelper.Get<AnswerViewModel>(); BindingContext = _vm; }
    public string Port { set { if (int.TryParse(value, out var p)) _vm.InitPort(p); } }
}
