namespace AymedCaseMauiClient;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Route kayıtları
        Routing.RegisterRoute("Answer", typeof(Views.AnswerPage));
    }
}
