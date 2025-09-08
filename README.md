# blogapp1web

**ITU MTAL öğrencileri** için geliştirilen basit ve öğretici bir **Blog Uygulaması**.
Amaç: Yazı oluşturma, listeleme, düzenleme ve silme gibi temel CRUD işlemleriyle birlikte (öğretmen/öğrenci) rolleri üzerinden temel yetkilendirmeyi göstermek.

> Lisans: **MIT** (açık kaynak, ticari kullanım serbest). ([GitHub][1])

## İçindekiler

* [Özellikler](#özellikler)
* [Mimari & Teknolojiler](#mimari--teknolojiler)
* [Dizin Yapısı](#dizin-yapısı)
* [Hızlı Başlangıç (Local)](#hızlı-başlangıç-local)
* [Veritabanı](#veritabanı)
* [Yapılandırma (App Settings)](#yapılandırma-app-settings)
* [Çalışma Mantığı](#çalışma-mantığı)
* [API Uç Noktaları (Varsa)](#api-uç-noktaları-varsa)
* [Roller ve Yetkilendirme](#roller-ve-yetkilendirme)
* [Örnek Kullanım Senaryosu](#örnek-kullanım-senaryosu)
* [Test](#test)
* [Geliştirme İpuçları](#geliştirme-ipuçları)
* [Dağıtım (IIS/Azure)](#dağıtım-iisazure)
* [SSS](#sss)
* [Katkı Rehberi](#katkı-rehberi)
* [Lisans](#lisans)

---

## Özellikler

> Not: Aşağıdaki maddeler **tipik bir blog uygulamasının** fonksiyonlarıdır. Depoda tek tek dosyaları görüntüleyemediğim için (araç kısıtı) doğrulanamayan kısımlar **TODO** ile işaretlendi.

* Yazı (post) CRUD: oluşturma, düzenleme, silme, listeleme, detay görüntüleme (**TODO: onayla**)
* Kategori/etiketleme desteği (**TODO: varsa onayla / yoksa kaldır**)
* Yorum sistemi (**TODO: varsa onayla / yoksa kaldır**)
* Basit arama ve sayfalama (**TODO: varsa onayla / yoksa kaldır**)
* Kimlik doğrulama (öğrenci/öğretmen) ve yetkilendirme (**TODO: rollerin isimlerini netleştir**)
* Yönetim paneli: yazı/kategori/yorum yönetimi (**TODO**)

## Mimari & Teknolojiler

* **.NET (C#)** ile **ASP.NET Web Application** (muhtemelen **MVC** ya da **Web Forms** şablonu; proje klasörü adı: `WebApplication1`) **TODO: hangisi kullanıldı—MVC mi Web Forms mu?**
* Görsel katman: **HTML** + (muhtemelen) **Razor** veya **ASPX** sayfaları. ([GitHub][1])
* İstemci: temel **JavaScript** ve az miktarda **CSS**. ([GitHub][1])
* Veritabanı: **SQL Server / LocalDB** (**TODO: kesinleştir**)
* Paket yönetimi: **NuGet** (gerekirse)
* Versiyon kontrol: **Git**

## Dizin Yapısı

> Standart ASP.NET projelerine göre açıklama. Dosya adları depoya göre uyarlanmıştır.

```
blogapp1web/
├─ blogapp1web.sln                # Çözüm dosyası (Visual Studio)   (kesin)
├─ WebApplication1/               # Ana web projesi                 (kesin)
│  ├─ Controllers/                # MVC ise: Controller sınıfları   (TODO)
│  ├─ Models/                     # Veri modelleri / ViewModel'ler  (TODO)
│  ├─ Views/                      # MVC ise: Razor .cshtml'ler      (TODO)
│  ├─ Pages/                      # Web Forms ise: .aspx sayfaları  (TODO)
│  ├─ App_Start/                  # RouteAuth, FilterConfig ...     (TODO MVC)
│  ├─ Content/                    # CSS/medya dosyaları             (olası)
│  ├─ Scripts/                    # JS dosyaları                    (olası)
│  ├─ web.config                  # Uygulama ayarları               (olası)
│  └─ ...                        
├─ .gitignore                     # Git ignore                      (kesin)
├─ .gitattributes                 # Git attributes                  (kesin)
└─ LICENSE.txt                    # MIT lisans metni                (kesin)
```

Kaynak doğrulaması: depo ana sayfası ve dil dağılımı. ([GitHub][1])

## Hızlı Başlangıç (Local)

### Gereksinimler

* **Windows + Visual Studio 2022/2019** (ASP.NET geliştirme iş yükü yüklü)
* **.NET Framework / .NET** (**TODO: hedef sürüm—VS’de Proje Özelliklerinden bak**)
* **SQL Server Express / LocalDB** (veya tam SQL Server)

### Kurulum

1. Depoyu klonla:

   ```bash
   git clone https://github.com/aFarukii/blogapp1web.git
   cd blogapp1web
   ```
2. **Visual Studio** ile `blogapp1web.sln` dosyasını aç.
3. İlk açılışta **NuGet paketlerini Restore** et (VS genelde otomatik yapar).
4. Veritabanı bağlantını **`Web.config`** (veya **`appsettings.json`**) içinden **ConnectionString** bölümünde ayarla. **TODO: dosya adını/doğru anahtarı onayla.**
5. (Migration kullanıyorsan) veritabanını oluştur:

   * **Entity Framework** ise: `Update-Database` (Package Manager Console).
   * Aksi halde SQL scriptini çalıştır. **TODO: migration/script durumu.**
6. F5 (IIS Express) ile çalıştır.

## Veritabanı

* **Connection String** örneği (LocalDB):

  ```xml
  <connectionStrings>
    <add name="DefaultConnection"
         connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BlogAppDb;Integrated Security=True;MultipleActiveResultSets=True"
         providerName="System.Data.SqlClient" />
  </connectionStrings>
  ```
* **Tablolar (muhtemel)**:

  * `Posts` (Id, Title, Slug, Content, AuthorId, CreatedAt, UpdatedAt, IsPublished, …)
  * `Categories` (Id, Name, Slug)
  * `PostCategories` (PostId, CategoryId)
  * `Comments` (Id, PostId, AuthorId/Name, Content, CreatedAt, IsApproved)
  * `AspNetUsers` / `Users` (kimlik doğrulama için)

**TODO:** Gerçek tablo şemasını doğrula (EF Code First ise `Models` ve `DbContext`’e bak).

## Yapılandırma (App Settings)

* **Uygulama Adı**: `blogapp1web` (**TODO:** varsa anahtar)
* **Sayfalama Boyutu**: `PageSize` (**TODO**)
* **Yükleme Klasörleri**: `Uploads/Images` gibi (**TODO**)
* **Kimlik Doğrulama**: Cookie ayarları / `Identity` opsiyonları (**TODO**)

## Çalışma Mantığı

1. **Giriş ekranı**: Kullanıcı/öğretmen giriş yapar (**TODO: login yolu**).
2. **Anasayfa**: Yayındaki yazılar listelenir; sayfalama/arama yapılır.
3. **Detay**: `/{slug}` ile yazı detayı; yorumlar görüntülenir.
4. **Yönetim**:

   * Yazı oluştur/düzenle/sil (sadece yetkili roller).
   * Kategori yönetimi.
   * Yorum onay/red.

**Rotalar (MVC varsayımı):**

* `GET /` → `HomeController.Index` (liste)
* `GET /post/{slug}` → `PostsController.Details`
* `GET /admin/posts` → `Admin/PostsController.Index`
  **TODO:** Web Forms ise `.aspx` sayfa yollarını yaz.

## Roller ve Yetkilendirme

* **Öğrenci**: Yazı/yorumları görüntüleme, gerekirse yorum ekleme.
* **Öğretmen/Admin**: Yazı CRUD, kategori/yorum yönetimi.
* ASP.NET Identity kullanılıyorsa: `Authorize(Roles="Admin")` attribute’leri ile korunur. **TODO: gerçek rol adlarını teyit et.**

## Örnek Kullanım Senaryosu

1. Öğretmen/admin giriş yapar.
2. **Yeni Yazı** → Başlık, içerik, kategori(ler), kapak görseli (varsa) girilir.
3. Kaydet → listeye düşer; yayıma alınır.
4. Öğrenciler anasayfadan görür, detayda okur ve **yorum** bırakır (yorumlar moderasyona düşebilir).
5. Admin gerektiğinde düzenler/siler.

## Test

* **Unit Test / Integration Test** projeleri yoksa:

  * Controller/Service seviyesinde **xUnit/NUnit + Moq** eklenebilir.
* Manuel test için:

  * CRUD akışlarını ve yetki kontrollerini kullanıcı rolleriyle tek tek dene.
  * Kimliği doğrulanmamış kullanıcıyla yönetim sayfalarına erişilemediğini kontrol et.

## Geliştirme İpuçları

* **Seed Data**: İlk kurulumda 1–2 örnek kullanıcı ve 5–10 yazı eklemek işini hızlandırır.
* **Slugs**: Başlıklardan URL dostu slug üret (Türkçe karakter dönüşümü).
* **Validations**: `[Required]`, `[StringLength]` vb. anotasyonlarla form doğrulama.
* **Güvenlik**:

  * CSRF korumaları (MVC’de `@Html.AntiForgeryToken()`),
  * XSS için `Html.Encode`,
  * Yüklemelerde dosya uzantısı/ölçü denetimi.
* **Performans**: Liste sayfalarında sayfalama + gerekli alanları seçerek veri çekme.

## Dağıtım (IIS/Azure)

### IIS (Windows Server)

1. **Release** modda publish (`Right Click Project > Publish`).
2. Hedef: **Folder** veya **IIS, FTP, etc.**
3. Sunucuda **IIS** rolü ve **.NET** runtime yüklü olmalı.
4. **App Pool** .NET versiyonunu doğru seç; **32/64-bit** ayarını projenle eşleştir.
5. `web.config`’te **ConnectionString**’i sunucu DB bilgileriyle güncelle.
6. Statik dosya izinleri ve `Uploads` gibi klasörlere yazma izni ver.

### Azure App Service

1. VS’den **Publish > Azure** yoluyla App Service oluştur.
2. App Service **.NET** yığınını proje hedefiyle eşleştir.
3. **Azure SQL** oluşturup bağlantıyı `ConnectionString`e ekle.
4. Gerekirse **App Settings** (Environment Variables) üzerinden yapılandır.

## SSS

* **MVC mi Web Forms mu?**
  Proje klasörü `WebApplication1`, dil dağılımı C#/HTML ağırlıklı—ASP.NET web uygulaması olduğu kesin. Kalıp, tek bir şablona indirgememek için README iki olasılığı da açıklıyor. **TODO: kesinleştirip bu bölümü güncelle.** ([GitHub][1])

* **Veritabanı migration var mı?**
  Eğitim projelerinde çoğunlukla **EF Code First** tercih edilir—`Migrations` klasörü varsa `Update-Database` ile kurulum yapılır. **TODO: kontrol et.**

## Lisans

Bu proje **MIT Lisansı** ile lisanslanmıştır. Ayrıntılar için `LICENSE.txt` dosyasına bakınız. ([GitHub][1])

