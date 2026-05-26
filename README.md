# DermaSmart Backend API ⚙️

DermaSmart cilt bakım uygulamasının sunucu tarafı altyapısıdır. Bu proje, .NET ortamında geliştirilmiş olup veritabanı yönetimi için Entity Framework Core ve SQLite kullanmaktadır.

---

## 🚀 Teknolojiler

- **Framework:** .NET Web API
- **Veritabanı:** SQLite
- **ORM:** Entity Framework Core
- **Dokümantasyon:** Swagger (OpenAPI)

---

## 🛠️ Kurulum ve Çalıştırma Adımları

Projeyi kendi bilgisayarınızda (lokalde) çalıştırmak için aşağıdaki adımları sırasıyla terminalinizde uygulayın.

### 1️⃣ Eksik Paketleri İndir (Restore)

API projesinin bağımlılıklarını indirmek için:

```bash
dotnet restore DermaSmart.API.csproj
```

> Başarılı olduğunda terminalde `"Restore completed"` benzeri bir çıktı göreceksiniz.

---

### 2️⃣ Veritabanını Güncelle

Veritabanı şemasını oluşturmak ve migration’ları uygulamak için:

```bash
dotnet ef database update --project DermaSmart.API.csproj
```

---

### 3️⃣ Projeyi Çalıştır

Sunucuyu başlatmak için:

```bash
dotnet run --project DermaSmart.API.csproj
```

---

## 📖 Swagger API Dokümantasyonu

Proje başarıyla çalıştırıldıktan sonra API uç noktalarını (endpoints) test etmek ve incelemek için Swagger arayüzüne erişebilirsiniz.

Tarayıcınızda aşağıdaki adresleri kullanın:

### Lokal Geliştirme

```txt
http://localhost:<PORT>/swagger
```

### Canlı (Production)

```txt
http://<yakında_eklenecek>/swagger
```

Swagger arayüzü üzerinden:

- GET
- POST
- PUT
- DELETE

gibi tüm HTTP işlemlerini doğrudan tarayıcı üzerinden test edebilirsiniz.

## Dokümantasyon

API uç noktalarının (endpoints) detayları, giden ve dönen JSON veri yapıları (şemaları) takım çalışmasını kolaylaştırmak adına **`Docs`** klasöründe modüler olarak ayrıştırılmıştır:

* 🔐 **[Kimlik Doğrulama (Auth) API Dokümantasyonu](./Docs/AUTH_API_DOCUMENTATION.md)**
* 👤 **[Cilt Profili (Skin Profile) API Dokümantasyonu](./Docs/SKIN_PROFILE_API.md)**

---

## Geliştirici Notları ve İyileştirmeler (Haftalık Scrum Özeti)
* **Veritabanı ve Modellerin Birleştirilmesi (Güncel Değişiklik):** Merge işlemi sonrasında oluşan tablo karmaşasını gidermek adına, `AppUser` ve `AppSkinProfile` tabloları sistemden tamamen kaldırıldı. Proje baştan aşağıya standart `User` ve `SkinProfile` modellerine bağlandı. Bu sayede 500 hataları ve veritabanı uyuşmazlıkları giderildi.
* **JWT Entegrasyonu:** Backend tarafında güvenli oturum yönetimi için JWT altyapısı kuruldu.
* **Hata Kodları (Error Codes):** Mobil ekip (Geliştirici 4) ile uyumlu çalışmak adına, hata döndürülen tüm uç noktalara `errorCode` anahtarı eklenerek JSON şemaları standart hale getirildi. Mobil taraf artık string okumak yerine bu hata kodlarını (`EMAIL_ALREADY_EXISTS`, `INVALID_CREDENTIALS` vb.) baz alarak UI kontrolleri yapabilir.
* **Form Entegrasyonu:** Cilt tipi kayıtları, güncellenen veritabanında ilişkisel olarak tutulup `/api/skinprofile` üzerinden başarılı ve eksiksiz bir şekilde servis edilmektedir.

