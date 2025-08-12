using System.Windows.Input;
using AymedCaseMauiClient.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AymedCaseMauiClient.ViewModels;
public partial class ServerViewModel : ObservableObject
{
    private readonly GrpcServerService _serverService;
    private string _statusMessage = "Sunucu Hazır";
    
    public string Port { get; set; } = "5001";
    public ICommand StartServerCommand { get; }
    public ICommand StopServerCommand  { get; }
    public ICommand OpenAnswerCommand  { get; }
    
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ServerViewModel(GrpcServerService serverService)
    {
        _serverService = serverService;
        
        StartServerCommand = new Command(async ()=> 
        {
            if(!int.TryParse(Port, out int portNumber))
            { 
                await Shell.Current.DisplayAlert("Uyarı", "Geçerli port girin.", "Tamam"); 
                return; 
            }
            
            try
            {
                StatusMessage = "Sunucu başlatılıyor...";
                bool result = await _serverService.StartServerAsync(portNumber);
                
                if(result)
                {
                    StatusMessage = $"Sunucu {portNumber} portunda çalışıyor";
                    await Shell.Current.DisplayAlert("Bilgi", $"Sunucu {portNumber} portunda başlatıldı.", "Tamam");
                }
                else
                {
                    StatusMessage = "Sunucu başlatılamadı";
                    await Shell.Current.DisplayAlert("Hata", "Sunucu başlatılamadı.", "Tamam");
                }
            }
            catch(Exception ex)
            {
                StatusMessage = "Hata oluştu";
                await Shell.Current.DisplayAlert("Hata", $"Sunucu başlatılırken hata oluştu: {ex.Message}", "Tamam");
            }
        });
        
        StopServerCommand = new Command(async ()=> 
        {
            try
            {
                StatusMessage = "Sunucu durduruluyor...";
                bool result = await _serverService.StopServerAsync();
                
                if(result)
                {
                    StatusMessage = "Sunucu durduruldu";
                    await Shell.Current.DisplayAlert("Bilgi", "Sunucu durduruldu.", "Tamam");
                }
                else
                {
                    StatusMessage = "Sunucu durdurulamadı";
                    await Shell.Current.DisplayAlert("Hata", "Sunucu durdurulamadı.", "Tamam");
                }
            }
            catch(Exception ex)
            {
                StatusMessage = "Hata oluştu";
                await Shell.Current.DisplayAlert("Hata", $"Sunucu durdurulurken hata oluştu: {ex.Message}", "Tamam");
            }
        });
        
        OpenAnswerCommand = new Command(async () =>
        {
            if(!int.TryParse(Port, out _)){ await Shell.Current.DisplayAlert("Uyarı","Geçerli port girin.","Tamam"); return; }
            await Shell.Current.GoToAsync($"Answer?port={Port}");
        });
    }
}
