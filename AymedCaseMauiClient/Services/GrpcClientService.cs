using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace AymedCaseMauiClient.Services;

public class GrpcClientService : IGrpcClientService, IDisposable
{
    // Simüle edilmiş mesaj sayaçları
    private int _emptyCounter = 1;
    private int _valueCounter = 1;
    private readonly Random _random = new Random();

    public async IAsyncEnumerable<string> StartEmptyStreamAsync(
    int port,
    [EnumeratorCancellation] CancellationToken token)
    {
        // Simüle edilmiş stream
        while (!token.IsCancellationRequested)
        {
            // "Message: X" formatında mesaj üret
            yield return $"Message: {_emptyCounter}";

            _emptyCounter++;
            if (_emptyCounter > 10000) _emptyCounter = 1;

            // Gerçek stream gibi davranması için delay ekle
            await Task.Delay(1000, token);
        }
    }

    public async IAsyncEnumerable<string> StartValueStreamAsync(
    int port,
    string value,
    [EnumeratorCancellation] CancellationToken token)
    {
        // Simüle edilmiş stream
        while (!token.IsCancellationRequested)
        {
            // "{value}: Mesaj X" formatında mesaj üret
            yield return $"{value}: Mesaj {_valueCounter}";

            _valueCounter++;
            if (_valueCounter > 10000) _valueCounter = 1;

            // Gerçek stream gibi davranması için delay ekle
            await Task.Delay(1000, token);
        }
    }

    public void Dispose()
    {
        // Simüle edilmiş implementasyonda gerçek bir dispose işlemi yok
        GC.SuppressFinalize(this);
    }
}
