# ChannelsECommerceExample

System.Threading.Channels Kütüphanesi
Makale
27/06/2023
Katkıda Bulunanlar: 3 kişi
İçindekiler

Üretici/tüketici kavramsal programlama modeli
Sınırlandırma stratejileri
Üretici API'leri
Tüketici API'leri
Daha fazlasını göster
System.Threading.Channels ad alanı, üreticiler ve tüketiciler arasında veriyi eşzamansız olarak aktarmak için bir dizi senkronizasyon veri yapısı sağlar. Kütüphane .NET Standard'ı hedefler ve tüm .NET uygulamalarında çalışır.

Bu kütüphane System.Threading.Channels NuGet paketi olarak mevcuttur. Ancak, .NET Core 3.0 veya daha sonraki sürümleri kullanıyorsanız, bu paket çerçevenin bir parçası olarak dahildir.

Üretici/tüketici kavramsal programlama modeli
Kanallar, üretici/tüketici kavramsal programlama modelinin bir uygulamasıdır. Bu programlama modelinde, üreticiler veriyi eşzamansız olarak üretir ve tüketiciler bu veriyi eşzamansız olarak tüketir. Başka bir deyişle, bu model veriyi bir taraftan diğerine aktarır. Kanalları, bir List<T> gibi diğer yaygın genel koleksiyon türleri olarak düşünebilirsiniz. Ana fark, bu koleksiyonun senkronizasyonu yönetmesi ve çeşitli tüketim modelleri sunmasıdır. Bu seçenekler, kanalların nasıl davrandığını, kaç öğe depolayabildiklerini ve bu sınır aşıldığında ne olduğunu veya kanalın birden fazla üretici veya tüketici tarafından eşzamanlı olarak erişilip erişilemeyeceğini kontrol eder.

Sınırlandırma stratejileri
Bir Channel<T> oluşturulma şekline bağlı olarak, okuyucu ve yazıcı farklı davranır.

Maksimum kapasite belirten bir kanal oluşturmak için Channel.CreateBounded yöntemini çağırın. Herhangi bir sayıda okuyucu ve yazıcı tarafından eşzamanlı olarak kullanılacak bir kanal oluşturmak için Channel.CreateUnbounded yöntemini çağırın. Her sınırlandırma stratejisi, sırasıyla BoundedChannelOptions veya UnboundedChannelOptions olan çeşitli yaratıcı tanımlı seçenekleri sunar.

Not: Sınırlandırma stratejisine bakılmaksızın, kapatıldıktan sonra kullanılan bir kanal her zaman bir ChannelClosedException fırlatır.

Sınırsız Kanallar
Sınırsız bir kanal oluşturmak için, Channel.CreateUnbounded aşırı yüklemelerinden birini çağırın:

```csharp
var channel = Channel.CreateUnbounded<T>();
```
Bir sınırsız kanal oluşturduğunuzda, varsayılan olarak kanal, herhangi bir sayıda okuyucu ve yazıcı tarafından eşzamanlı olarak kullanılabilir. Alternatif olarak, UnboundedChannelOptions örneği sağlayarak sınırsız bir kanal oluştururken varsayılan olmayan davranışları belirtebilirsiniz. Kanalın kapasitesi sınırsızdır ve tüm yazma işlemleri eşzamanlı olarak gerçekleştirilir. Daha fazla örnek için, Sınırsız yaratım desenlerine bakın.

Sınırlı Kanallar
Sınırlı bir kanal oluşturmak için, Channel.CreateBounded aşırı yüklemelerinden birini çağırın:

```csharp
var channel = Channel.CreateBounded<T>(7);
```
Yukarıdaki kod, maksimum 7 öğe kapasitesine sahip bir kanal oluşturur. Sınırlı bir kanal oluşturduğunuzda, kanal maksimum bir kapasiteye bağlıdır. Bu sınır aşıldığında, varsayılan davranış, üreticinin boşluk olana kadar eşzamanlı olarak engellenmesidir. Kanalı oluştururken bir seçenek belirterek bu davranışı yapılandırabilirsiniz. Sınırlı kanallar, sıfırdan büyük herhangi bir kapasite değeri ile oluşturulabilir. Diğer örnekler için, Sınırlı yaratım desenlerine bakın.

Doluluk Modu Davranışı
Sınırlı bir kanal kullanırken, yapılandırılmış sınır aşıldığında kanalın uyduğu davranışı belirtebilirsiniz. Aşağıdaki tablo, her BoundedChannelFullMode değeri için doluluk modu davranışlarını listeler:

Değer	Davranış
BoundedChannelFullMode.Wait	Bu varsayılan değerdir. WriteAsync çağrıları, yazma işlemini tamamlamak için boşluk olana kadar bekler. TryWrite çağrıları hemen false döner.
BoundedChannelFullMode.DropNewest	Yazılmakta olan öğe için yer açmak amacıyla kanaldaki en yeni öğeyi kaldırır ve yok sayar.
BoundedChannelFullMode.DropOldest	Yazılmakta olan öğe için yer açmak amacıyla kanaldaki en eski öğeyi kaldırır ve yok sayar.
BoundedChannelFullMode.DropWrite	Yazılmakta olan öğeyi yok sayar.
Önemli: Bir Channel<TWrite,TRead>.Writer, bir Channel<TWrite,TRead>.Reader'dan daha hızlı ürettiğinde, kanalın yazıcısı geri basınçla karşılaşır.

Üretici API'leri
Üretici işlevselliği Channel<TWrite,TRead>.Writer üzerinde sunulur. Üretici API'leri ve beklenen davranışları aşağıdaki tabloda detaylandırılmıştır:

API	Beklenen Davranış
ChannelWriter<T>.Complete	Kanalı tamamlanmış olarak işaretler, yani kanala artık öğe yazılmaz.
ChannelWriter<T>.TryComplete	Kanalı tamamlanmış olarak işaretlemeye çalışır, yani kanala artık veri yazılmaz.
ChannelWriter<T>.TryWrite	Belirtilen öğeyi kanala yazmaya çalışır. Sınırsız bir kanal ile kullanıldığında, bu her zaman doğru döner, aksi takdirde ChannelWriter<T>.Complete veya ChannelWriter<T>.TryComplete ile sinyal verildiğinde.
ChannelWriter<T>.WaitToWriteAsync	Bir öğe yazmak için boşluk olduğunda tamamlanan bir ValueTask<TResult> döner.
ChannelWriter<T>.WriteAsync	Bir öğeyi kanala eşzamanlı olarak yazar.
Tüketici API'leri
Tüketici işlevselliği Channel<TWrite,TRead>.Reader üzerinde sunulur. Tüketici API'leri ve beklenen davranışları aşağıdaki tabloda detaylandırılmıştır:

API	Beklenen Davranış
ChannelReader<T>.ReadAllAsync	Kanaldan tüm veriyi okumaya imkan tanıyan bir IAsyncEnumerable<T> oluşturur.
ChannelReader<T>.ReadAsync	Kanaldan eşzamanlı olarak bir öğe okur.
ChannelReader<T>.TryPeek	Kanaldan bir öğeyi önizlemeye çalışır.
ChannelReader<T>.TryRead	Kanaldan bir öğe okumaya çalışır.
ChannelReader<T>.WaitToReadAsync	Okunacak veri olduğunda tamamlanan bir ValueTask<TResult> döner.
Yaygın Kullanım Desenleri
Kanallar için birkaç kullanım deseni vardır. API, mümkün olduğunca basit, tutarlı ve esnek olacak şekilde tasarlanmıştır. Tüm eşzamanlı yöntemler, operasyon eşzamanlı olarak tamamlandığında tahsisattan kaçınabilen hafif bir eşzamanlı operasyonu temsil eden bir ValueTask (veya ValueTask<bool>) döner. Ayrıca, API, kompozisyonlu olacak şekilde tasarlanmıştır; yani, bir kanalın yaratıcısı, amaçlanan kullanımı hakkında vaatlerde bulunur. Belirli parametrelerle bir kanal oluşturulduğunda, dahili uygulama bu vaatleri bilerek daha verimli çalışabilir.

Yaratım Desenleri
Bir küresel pozisyon sistemi (GPS) için bir üretici/tüketici çözümü oluşturduğunuzu hayal edin. Bir cihazın zaman içindeki koordinatlarını izlemek istersiniz. Örnek bir koordinatlar nesnesi şu şekilde olabilir:

```csharp
/// <summary>
/// Bir cihazın koordinatlarının temsili, 
/// enlem ve boylam içerir.
/// </summary>
/// <param name="DeviceId">Benzersiz bir cihaz tanımlayıcısı.</param>
/// <param name="Latitude">Cihazın enlemi.</param>
/// <param name="Longitude">Cihazın boylamı.</param>
public readonly record struct Coordinates(
    Guid DeviceId,
    double Latitude,
    double Longitude);
```
Sınırsız Yaratım Desenleri
Yaygın bir kullanım deseni, varsayılan bir sınırsız kanal oluşturmaktır:

```csharp
var channel = Channel.CreateUnbounded<Coordinates>();
```
Ancak, birden fazla üretici ve tüketiciye sahip bir sınırsız kanal oluşturmak istediğinizi hayal edin:

```csharp
var channel = Channel.CreateUnbounded<Coordinates>(
    new UnboundedChannelOptions
    {
        SingleWriter = false,
        SingleReader = false,
        AllowSynchronousContinuations = true
    });
```
Bu durumda, tüm yazma işlemleri eşzamanlıdır, hatta WriteAsync bile. Bunun nedeni, sınırsız bir kanalın yazma için her zaman anında kullanılabilir alana sahip olmasıdır. Ancak, AllowSynchronousContinuations ayarlandığında, yazmalar, okuyucu ile ilişkili işleri, devam eden işlemlerini yürüterek yapabilir. Bu, işlemin eşzamanlılığını etkilemez.

Sınırlı Yaratım Desenleri
Sınırlı kanallar ile, kanalın tüketim davranışını sağlamak için kanalın tüketiciye yapılandırılabilir olması gerektiği bilinmelidir. Yani, tüketici kanalın yapılandırılmış sınır aşıldığında ne tür bir davranış sergilediğini bilmelidir. Sınırlı yaratım desenlerinin bazılarını inceleyelim.

En basit sınırlı kanal oluşturma yolu, bir kapasite belirtmektir:

```csharp
var channel = Channel.CreateBounded<Coordinates>(1);
```
Yukarıdaki kod, maksimum 1 öğe kapasitesine sahip bir sınırlı kanal oluşturur. Diğer seçenekler de mevcuttur; bazı seçenekler sınırsız bir kanal ile aynıdır, bazıları ise sınırsız kanallara özeldir:

```csharp
var channel = Channel.CreateBounded<Coordinates>(
    new BoundedChannelOptions(1_000)
    {
        SingleWriter = true,
        SingleReader = false,
        AllowSynchronousContinuations = false,
        FullMode = BoundedChannelFullMode.DropWrite
    });
```
Yukarıdaki kodda, kanal 1,000 öğe ile sınırlı bir kanal olarak oluşturulmuştur, tek bir yazıcı ile ancak birçok okuyucu ile. Doluluk modu davranışı DropWrite olarak tanımlanmıştır; yani, kanal doluysa yazılan öğe düşürülür.

Sınırlı kanallar kullanıldığında düşürülen öğeleri gözlemlemek için bir itemDropped geri çağrısını kaydedin:

```csharp
var channel = Channel.CreateBounded(
    new BoundedChannelOptions(10)
    {
        AllowSynchronousContinuations = true,
        FullMode = BoundedChannelFullMode.DropOldest
    },
    static void (Coordinates dropped) =>
        Console.WriteLine($"Koordinatlar düşürüldü: {dropped}"));
```
Kanal dolu olduğunda ve yeni bir öğe eklendiğinde, itemDropped geri çağrısı çağrılır. Bu örnekte, sağlanan geri çağrı öğeyi konsola yazdırır, ancak istediğiniz herhangi bir başka eylemi gerçekleştirebilirsiniz.

Üretici Desenleri
Bu senaryoda üreticinin kanala yeni koordinatlar yazdığını hayal edin. Üretici bunu TryWrite çağrısı ile yapabilir:

```csharp
static void ProduceWithWhileAndTryWrite(
    ChannelWriter<Coordinates> writer, Coordinates coordinates)
{
    while (coordinates is { Latitude: < 90, Longitude: < 180 })
    {
        var tempCoordinates = coordinates with
        {
            Latitude = coordinates.Latitude + .5,
            Longitude = coordinates.Longitude + 1
        };

        if (writer.TryWrite(item: tempCoordinates))
        {
            coordinates = tempCoordinates;
        }
    }
}
```
Yukarıdaki üretici kodu:

Channel<Coordinates>.Writer (ChannelWriter<Coordinates>) ve başlangıç Coordinates kabul eder.
TryWrite kullanarak koordinatları hareket ettirmeye çalışan bir koşullu while döngüsü tanımlar.
Alternatif bir üretici, WriteAsync yöntemini kullanabilir:

```csharp
static async ValueTask ProduceWithWhileWriteAsync(
    ChannelWriter<Coordinates> writer, Coordinates coordinates)
{
    while (coordinates is { Latitude: < 90, Longitude: < 180 })
    {
        await writer.WriteAsync(
            item: coordinates = coordinates with
            {
                Latitude = coordinates.Latitude + .5,
                Longitude = coordinates.Longitude + 1
            });
    }

    writer.Complete();
}
```
Yine, Channel<Coordinates>.Writer bir while döngüsü içinde kullanılır. Ancak bu sefer, WriteAsync yöntemi çağrılır. Yöntem yalnızca koordinatlar yazıldıktan sonra devam eder. While döngüsü sona erdiğinde, Complete çağrısı yapılır ve bu, kanala artık veri yazılmadığını belirtir.

Başka bir üretici deseni, WaitToWriteAsync yöntemini kullanmaktır:

```csharp
static async ValueTask ProduceWithWaitToWriteAsync(
    ChannelWriter<Coordinates> writer, Coordinates coordinates)
{
    while (coordinates is { Latitude: < 90, Longitude: < 180 } &&
        await writer.WaitToWriteAsync())
    {
        var tempCoordinates = coordinates with
        {
            Latitude = coordinates.Latitude + .5,
            Longitude = coordinates.Longitude + 1
        };

        if (writer.TryWrite(item: tempCoordinates))
        {
            coordinates = tempCoordinates;
        }

        await Task.Delay(TimeSpan.FromMilliseconds(10));
    }

    writer.Complete();
}
```
Koşullu while döngüsünün bir parçası olarak, WaitToWriteAsync çağrısının sonucu döngünün devam edip etmeyeceğini belirlemek için kullanılır.

Tüketici Desenleri
Kanallar için birkaç yaygın tüketici deseni vardır. Bir kanal sonsuzsa, yani veri üretimi süresiz olarak devam ediyorsa, tüketici while (true) döngüsünü kullanabilir ve veri geldikçe okur:

```csharp
static async ValueTask ConsumeWithWhileAsync(
    ChannelReader<Coordinates> reader)
{
    while (true)
    {
        // Kanal kapatıldığında ChannelClosedException fırlatılabilir.
        Coordinates coordinates = await reader.ReadAsync();
        Console.WriteLine(coordinates);
    }
}
```
Bu kod kanal kapatıldığında bir istisna fırlatır.

Alternatif bir tüketici, bu endişeyi önlemek için aşağıdaki kodda gösterildiği gibi iç içe bir while döngüsü kullanabilir:

```csharp
static async ValueTask ConsumeWithNestedWhileAsync(
    ChannelReader<Coordinates> reader)
{
    while (await reader.WaitToReadAsync())
    {
        while (reader.TryRead(out Coordinates coordinates))
        {
            Console.WriteLine(coordinates);
        }
    }
}
```
Yukarıdaki kodda, tüketici veriyi okumak için bekler. Veri mevcut olduğunda, tüketici veriyi okumaya çalışır. Bu döngüler, kanalın üreticisi okunacak veri kalmadığını belirtene kadar devam eder. Bir üreticinin belirli sayıda öğe ürettiği ve tamamlandığını belirttiği biliniyorsa, tüketici öğeleri yinelemek için await foreach semantiğini kullanabilir:

```csharp
static async ValueTask ConsumeWithAwaitForeachAsync(
    ChannelReader<Coordinates> reader)
{
    await foreach (Coordinates coordinates in reader.ReadAllAsync())
    {
        Console.WriteLine(coordinates);
    }
}
```
Yukarıdaki kod, kanaldan tüm koordinatları okumak için ReadAllAsync yöntemini kullanır.
