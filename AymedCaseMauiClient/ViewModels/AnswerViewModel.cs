using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AymedCaseMauiClient.Services;

namespace AymedCaseMauiClient.ViewModels;

public partial class AnswerViewModel : ObservableObject
{
    private readonly IGrpcClientService _grpc;
    private readonly SemaphoreSlim _semaphore = new(2);
    private CancellationTokenSource? _ctsEmpty;
    private CancellationTokenSource? _ctsValue;
    private int _port;

    public ObservableCollection<string> Messages { get; } = new();

    [ObservableProperty] private string _emptyStatus = "Beklemede";
    [ObservableProperty] private string _valueStatus = "Beklemede";
    [ObservableProperty] private string _streamValue = ""; // alttaki stream parametresi
    [ObservableProperty] private string _cutIndex = "";    // KES üstündeki numeric Entry
    [ObservableProperty] private string _lastEmptyMessage = ""; // Son gelen boş stream mesajı
    [ObservableProperty] private string _lastValueMessage = ""; // Son gelen değer stream mesajı

    public ICommand StartEmptyCommand { get; }
    public ICommand StopEmptyCommand { get; }
    public ICommand StartValueCommand { get; }
    public ICommand StopValueCommand { get; }
    public ICommand CutCommand { get; }

    public AnswerViewModel(IGrpcClientService grpc)
    {
        _grpc = grpc;

        StartEmptyCommand = new Command(async () =>
        {
            if (!await _semaphore.WaitAsync(0)) return;
            _ctsEmpty = new CancellationTokenSource();
            EmptyStatus = "İzleniyor…";
            _ = Task.Run(async () =>
            {
                try
                {
                    await foreach (var m in _grpc.StartEmptyStreamAsync(_port, _ctsEmpty.Token))
                    {
                        LastEmptyMessage = m; // Son mesajı güncelle
                        Append(m);
                    }
                }
                finally { _semaphore.Release(); }
            });
        });

        StopEmptyCommand = new Command(() =>
        {
            _ctsEmpty?.Cancel();
            EmptyStatus = "Durduruldu";
        });

        StartValueCommand = new Command(async () =>
        {
            if (!await _semaphore.WaitAsync(0)) return;
            _ctsValue = new CancellationTokenSource();
            ValueStatus = $"İzleniyor: {StreamValue}";
            _ = Task.Run(async () =>
            {
                try
                {
                    await foreach (var m in _grpc.StartValueStreamAsync(_port, StreamValue, _ctsValue.Token))
                    {
                        LastValueMessage = m; // Son mesajı güncelle
                        Append(m);
                    }
                }
                finally { _semaphore.Release(); }
            });
        });

        StopValueCommand = new Command(() =>
        {
            _ctsValue?.Cancel();
            ValueStatus = "Durduruldu";
        });

        CutCommand = new Command(() =>
        {
            // Her iki akış da çalışıyorsa (semaphore 0 ise) kesmeye izin ver
            if (_semaphore.CurrentCount != 0)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Uyarı", "Kesme işlemi için her iki akış da çalışıyor olmalı.", "Tamam");
                });
                return;
            }

            if (!int.TryParse(CutIndex, out var idx))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Uyarı", "Lütfen 0-9 arasında bir rakam girin.", "Tamam");
                });
                return;
            }

            if (idx >= 0 && idx < Messages.Count)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    // Silinecek mesajı kaydet
                    string removedMessage = Messages[idx];

                    // Mesajı sil
                    Messages.RemoveAt(idx);

                    // Kullanıcıya hangi mesajın silindiğini bildir
                    await Application.Current.MainPage.DisplayAlert("Bilgi", $"{idx}. indeksteki \"{removedMessage}\" mesajı silindi.", "Tamam");
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Uyarı", $"Geçersiz indeks: {idx}. İndeks 0-{Math.Max(0, Messages.Count - 1)} arasında olmalıdır.", "Tamam");
                });
            }
        });
    }

    public void InitPort(int port) => _port = port;

    private void Append(string text)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (Messages.Count >= 10) Messages.RemoveAt(0);
            Messages.Add(text);
        });
    }
}
