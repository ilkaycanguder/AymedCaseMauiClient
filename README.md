# AymedCase MAUI Client

Bu proje, .NET MAUI kullanarak geliştirilen ve gRPC protokolü üzerinden iletişim kuran bir istemci uygulamasıdır. Uygulama, gRPC sunucu ile iletişim kurarak iki farklı türde mesaj akışını (stream) görüntüleyebilir ve yönetebilir.

## Proje Yapısı

Proje, SOLID prensiplerine ve MVVM (Model-View-ViewModel) mimarisine uygun olarak tasarlanmıştır:

```
AymedCaseMauiClient/
├── Behaviors/                # UI davranışları (validasyon vb.)
├── Helpers/                  # Yardımcı sınıflar (ServiceHelper vb.)
├── Models/                   # Veri modelleri
├── Protos/                   # Protocol Buffers tanımlamaları
├── Services/                 # Servis katmanı (gRPC istemci ve sunucu servisleri)
├── ViewModels/               # Görünüm modelleri
├── Views/                    # XAML görünümleri
├── App.xaml                  # Uygulama tanımı
├── AppShell.xaml             # Shell navigasyon yapısı
└── MauiProgram.cs            # Uygulama başlangıç noktası ve DI yapılandırması
```

## Mimari

Bu proje aşağıdaki mimari prensiplere göre geliştirilmiştir:

### MVVM (Model-View-ViewModel)

- **Model**: Veri yapılarını tanımlar
- **View**: Kullanıcı arayüzünü tanımlar (XAML dosyaları)
- **ViewModel**: View ile Model arasında köprü görevi görür, komutları ve veri bağlamalarını yönetir

### Dependency Injection (DI)

- Servisler ve ViewModeller `MauiProgram.cs` içinde DI konteynerine kaydedilir
- `ServiceHelper` sınıfı, DI konteynerinden servislere erişim sağlar

### Shell Navigation

- `AppShell.xaml` dosyası, uygulamanın navigasyon yapısını tanımlar
- Sayfalar arası geçişler ve parametre aktarımı Shell navigasyonu ile yapılır

## Simüle Edilmiş gRPC

Bu projede, gerçek gRPC implementasyonu yerine simüle edilmiş bir gRPC yapısı kullanılmıştır. Bunun nedenleri:

1. Platformlar arası uyumluluk sorunları
2. Proto dosyalarının derlenmesi sırasında oluşan namespace çakışmaları
3. Asenkron stream işlemlerinde yaşanan teknik zorluklar

### Simülasyon Detayları

- `GrpcClientService`: Gerçek bir gRPC istemcisi yerine, benzer davranışı simüle eden bir yapı içerir

  - `StartEmptyStreamAsync`: Boş istek ile başlatılan bir akış simüle eder, "Message: X" formatında mesajlar üretir
  - `StartValueStreamAsync`: İçerik isteği ile başlatılan bir akış simüle eder, "{value}: Mesaj X" formatında mesajlar üretir

- `GrpcServerService`: Gerçek bir gRPC sunucusu yerine, sunucu başlatma/durdurma işlemlerini simüle eder
  - `StartServerAsync`: Sunucu başlatma işlemini simüle eder
  - `StopServerAsync`: Sunucu durdurma işlemini simüle eder

### Gerçek gRPC Entegrasyonu İçin Gerekli Adımlar

Projeyi gerçek gRPC ile çalıştırmak için:

1. `notification.proto` dosyasının doğru namespace ile derlenmesini sağlamak
2. `GrpcClientService` sınıfını gerçek gRPC istemcisi kullanacak şekilde güncellemek
3. `GrpcServerService` sınıfını gerçek gRPC sunucusu başlatacak şekilde güncellemek
4. HTTP/2 ve sertifika doğrulama ayarlarını yapılandırmak

## Uygulama Özellikleri

### Server Sayfası

- Port numarası girişi
- Sunucu başlatma/durdurma kontrolleri
- İstenilen sayfasına geçiş butonu

### Answer Sayfası

- İki farklı mesaj akışını (stream) başlatma/durdurma
- Her iki akıştan gelen son mesajları görüntüleme
- Son 10 mesajı liste halinde görüntüleme
- Belirli bir indeksteki mesajı silme (Kes özelliği)
- 0-9 arası sayısal giriş doğrulama

## Teknik Detaylar

### Semaphore Kontrolü

- İki eşzamanlı akış için `SemaphoreSlim(2)` kullanılır
- Her akış başlatıldığında bir slot alır, durdurulduğunda serbest bırakır
- "Kes" özelliği yalnızca her iki akış da çalışırken (semaphore.CurrentCount == 0) aktiftir

### Mesaj Formatları

- Boş istek akışı: "Message: X" formatında mesajlar üretir
- İçerik isteği akışı: "{girilen değer}: Mesaj X" formatında mesajlar üretir

### Asenkron Programlama

- gRPC stream işlemleri için `IAsyncEnumerable<T>` kullanılır
- İptal işlemleri için `CancellationTokenSource` ve `CancellationToken` kullanılır

## Gelecek Geliştirmeler

1. Gerçek gRPC entegrasyonunun tamamlanması
2. Daha kapsamlı hata işleme mekanizmaları
3. Birim testlerin eklenmesi
4. Kullanıcı arayüzü iyileştirmeleri
5. Performans optimizasyonları

## Kurulum ve Çalıştırma

1. Projeyi klonlayın
2. Visual Studio 2022 veya JetBrains Rider ile açın
3. NuGet paketlerini geri yükleyin
4. Projeyi derleyin ve çalıştırın

## Kullanılan Teknolojiler

- .NET MAUI
- C# 11
- Protocol Buffers (Proto3)
- gRPC (simüle edilmiş)
- CommunityToolkit.Mvvm
- XAML
"# AymedCaseMauiClient" 
