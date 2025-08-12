using System.Collections.Generic;
using System.Threading;

namespace AymedCaseMauiClient.Services;
public interface IGrpcClientService
{
    IAsyncEnumerable<string> StartEmptyStreamAsync(int port, CancellationToken token);
    IAsyncEnumerable<string> StartValueStreamAsync(int port, string value, CancellationToken token);
}
