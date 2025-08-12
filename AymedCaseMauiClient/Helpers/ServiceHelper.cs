using Microsoft.Extensions.DependencyInjection;

namespace AymedCaseMauiClient.Helpers;
public static class ServiceHelper
{
    private static IServiceProvider? _sp;
    public static void Init(IServiceProvider sp) => _sp = sp;
    public static T Get<T>() where T : notnull => _sp!.GetRequiredService<T>();
}
